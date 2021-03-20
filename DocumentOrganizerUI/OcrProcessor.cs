using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using DocumentOrganizerUI.Extensions;
using System.Drawing;
using System.Drawing.Imaging;

namespace DocumentOrganizerUI
{
    public class OCRProcessor
    {
        private DirectoryInfo source;
        private DirectoryInfo working;
        private DirectoryInfo processed;
        private DirectoryInfo preview50Dir;
        private DirectoryInfo preview300Dir;
        private DirectoryInfo textDir;
        private string ghostScriptPath;
        private string tesseractPath;
        private CancellationTokenSource cts;
        private List<Task> runners;

        public OCRProcessor(FileProcessorConfiguration config)
        {
            runners = new List<Task>();

            source = new DirectoryInfo(config.SourceDir);
            processed = new DirectoryInfo(config.ProcessedDir);
            working = new DirectoryInfo(config.WorkingDir);
            ghostScriptPath = config.GhostScriptPath;
            tesseractPath = config.TesseractPath;
            cts = new CancellationTokenSource();

            preview50Dir = new DirectoryInfo(Path.Combine(processed.FullName, "previews", "50"));
            if (!preview50Dir.Exists)
            {
                preview50Dir.Create();
            }
            preview300Dir = new DirectoryInfo(Path.Combine(processed.FullName, "previews", "300"));
            if (!preview300Dir.Exists)
            {
                preview300Dir.Create();
            }
            textDir = new DirectoryInfo(Path.Combine(processed.FullName, "previews", "text"));
            if (!textDir.Exists)
            {
                textDir.Create();
            }
        }

        public void Start()
        {
            var ct = cts.Token;
            for (int i = 0; i < 6; i++)
            {
                runners.Add(Task.Run(async () =>
                {
                    while (true)
                    {
                        bool processed = await ProcessNextFile();

                        //if there was nothing to process, let's delay the next check for work
                        if (!processed)
                        {
                            try
                            {
                                await Task.Delay(1000, ct);
                            }
                            catch (OperationCanceledException)
                            {
                                return;
                            }
                        }
                        if (ct.IsCancellationRequested) return;
                    }
                }, ct));
            }
        }

        public async Task StopAsync()
        {
            cts.Cancel();
            foreach(var runner in runners)
            {
                await runner;
            }
        }

        private static SemaphoreSlim createLock = new SemaphoreSlim(1);
        private static SemaphoreSlim fetchLock = new SemaphoreSlim(1);

        public async Task<bool> ProcessNextFile()
        {
            FileInfo nextFile;
            DirectoryInfo tempDir = new DirectoryInfo(Path.Combine(working.FullName, Guid.NewGuid().ToString()));
            tempDir.Create();
            try
            {
                await fetchLock.WaitAsync();
                try
                {
                    nextFile = source.GetFiles().FirstOrDefault();
                    if (nextFile != null)
                    {
                        var oldPath = nextFile.FullName;
                        var workingPath = Path.Combine(tempDir.FullName, nextFile.Name);
                        nextFile.MoveTo(workingPath);
                        while (File.Exists(oldPath))
                        {
                            //wait for the delete to go through
                            await Task.Delay(100);
                        }
                        nextFile = new FileInfo(workingPath);
                    }
                }
                finally
                {
                    fetchLock.Release();
                }

                if (nextFile != null)
                {
                    FileInfo processedFile;
                    await createLock.WaitAsync();
                    try
                    {
                        processedFile = new FileInfo(GetUniqueOutputFilePath(Path.Combine(processed.FullName, nextFile.NameWithoutExtension() + ".pdf")));
                        //create an empty file to reserve this name
                        processedFile.Create().Close();
                    }
                    finally
                    {
                        createLock.Release();
                    }

                    if (nextFile.Extension.Equals(".pdf", StringComparison.InvariantCultureIgnoreCase))
                    {
                        bool hasText = false;

                        //if pdf, open the pdf and determine if it has a text layer
                        using (PdfReader reader = new PdfReader(nextFile.FullName))
                        {
                            iTextSharp.text.pdf.parser.PdfReaderContentParser parser = new iTextSharp.text.pdf.parser.PdfReaderContentParser(reader);
                            iTextSharp.text.pdf.parser.ITextExtractionStrategy strategy;
                            for (int i = 1; i <= reader.NumberOfPages; i++)
                            {
                                //byte[] pageContent = reader.GetPageContent(i);
                                strategy = parser.ProcessContent(i, new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy());
                                string text = strategy.GetResultantText();
                                if (!text.IsNullOrWhiteSpaceOrGibberish())
                                {
                                    hasText = true;
                                    break;
                                }
                            }
                        }

                        if (!hasText)//If it does not have a text layer, convert to images with ghostscript
                        {
                            RunSilentProcess(ghostScriptPath, tempDir.FullName, $"-q -dNOPAUSE -dBATCH -sDEVICE=pngalpha -dUseCropBox -sOutputFile=output-%d.png -r600 \"{nextFile.Name}\"");

                            List<string> pages = new List<string>();

                            foreach (var pageImageFile in tempDir.GetFiles("output-*.png"))
                            {
                                var imageFile = pageImageFile;
                                //verify image size is reasonable
                                try
                                {
                                    bool converted = false;
                                    using (var i = Bitmap.FromFile(pageImageFile.FullName))
                                    {
                                        //5100 width for 600 dpi 8.5 inch
                                        if (i.Size.Width > 6000 || i.Size.Height > 12000)
                                        {
                                            using (var outputImage = new Bitmap(5100, (int)(((double)i.Size.Height / i.Size.Width) * 5100), PixelFormat.Format24bppRgb))
                                            {
                                                outputImage.SetResolution(600, 600);
                                                using (var g = Graphics.FromImage(outputImage))
                                                {
                                                    g.DrawImage(i, 0, 0, outputImage.Width, outputImage.Height);
                                                }
                                                imageFile = new FileInfo(Path.Combine(pageImageFile.Directory.FullName, pageImageFile.NameWithoutExtension() + "F.png"));
                                                outputImage.Save(imageFile.FullName, ImageFormat.Png);
                                                converted = true;
                                            }
                                        }
                                    }
                                    if (converted)
                                    {
                                        pageImageFile.Delete();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.ToString());
                                }
                                
                                //run tesseract.exe source output(without extension) -l eng PDF to get PDF file
                                RunSilentProcess(tesseractPath, tempDir.FullName, $"{imageFile.Name} {pageImageFile.NameWithoutExtension()} --oem 1 -l eng PDF");

                                pages.Add(pageImageFile.NameWithoutExtension() + ".pdf");
                                imageFile.Delete();
                            }

                            if (pages.Count == 1)
                            {
                                MoveAndOverwrite(Path.Combine(tempDir.FullName, pages[0]), processedFile.FullName);
                            }
                            else if (pages.Count > 1)
                            {
                                //if multiple pages use ghostscript to combine to one pdf
                                string inputFilesCommandLine = String.Join(" ", pages);
                                RunSilentProcess(ghostScriptPath, tempDir.FullName, $"-q -dNOPAUSE -dBATCH -sDEVICE=pdfwrite -dPDFSETTINGS=/prepress -sOutputFile=output.pdf {inputFilesCommandLine}");

                                MoveAndOverwrite(Path.Combine(tempDir.FullName, "output.pdf"), processedFile.FullName);
                                foreach (var page in pages)
                                {
                                    File.Delete(Path.Combine(tempDir.FullName, page));
                                }
                            }
                            nextFile.Delete();
                        }
                        else//If it has a text layer, leave it as is. 
                        {
                            MoveAndOverwrite(nextFile.FullName, processedFile.FullName);
                        }
                    }
                    else//assume image
                    {
                        //run tesseract.exe source output(without extension) -l eng PDF to get PDF file
                        RunSilentProcess(tesseractPath, tempDir.FullName, $"\"{nextFile.Name}\" \"{nextFile.NameWithoutExtension()}\" --oem 1 -l eng PDF");

                        MoveAndOverwrite(Path.Combine(tempDir.FullName, nextFile.NameWithoutExtension() + ".pdf"), processedFile.FullName);
                        nextFile.Delete();
                    }

                    //make previews
                    RenderPDFToJpegFile(processedFile.FullName, 50, Path.Combine(preview50Dir.FullName, processedFile.NameWithoutExtension() + ".jpg"));
                    RenderPDFToJpegFile(processedFile.FullName, 300, Path.Combine(preview300Dir.FullName, processedFile.NameWithoutExtension() + ".jpg"));
                    //output text
                    File.WriteAllText(Path.Combine(textDir.FullName, processedFile.NameWithoutExtension() + ".txt"), ExtractText(processedFile.FullName));
                }
                tempDir.Delete(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return false;
        }

        private static string ExtractText(string pdfFilePath)
        {
            StringBuilder sb = new StringBuilder();
            //if pdf, open the pdf and determine if it has a text layer
            try
            {
                using (PdfReader reader = new PdfReader(pdfFilePath))
                {
                    iTextSharp.text.pdf.parser.PdfReaderContentParser parser = new iTextSharp.text.pdf.parser.PdfReaderContentParser(reader);
                    iTextSharp.text.pdf.parser.ITextExtractionStrategy strategy;
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        strategy = parser.ProcessContent(i, new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy());
                        string text = strategy.GetResultantText();
                        if (!String.IsNullOrEmpty(text))
                        {
                            sb.AppendLine(text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return sb.ToString();
        }

        public static void RunSilentProcess(string imagePath, string workingDir, string arguments)
        {
            var psi = new ProcessStartInfo(imagePath, arguments);
            psi.WorkingDirectory = workingDir;
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi).WaitForExit();
        }

        private static void MoveAndOverwrite(string source, string target)
        {
            File.Copy(source, target, true);
            File.Delete(source);
        }

        private void RenderPDFToJpegFile(string pdfPath, int resolution, string targetPath)
        {
            DirectoryInfo tempDir = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
            tempDir.Create();
            try
            {
                //convert to images with ghostscript
                RunSilentProcess(ghostScriptPath, tempDir.FullName, $"-q -dNOPAUSE -dBATCH -sDEVICE=jpeg -dUseCropBox -sOutputFile=%d.jpg -r{resolution} \"{pdfPath}\"");

                foreach (var pageImageFile in tempDir.GetFiles("*.jpg"))
                {
                    File.Move(pageImageFile.FullName, targetPath);
                    //TODO: handle multiple pages
                    return;
                }
            }
            finally
            {
                tempDir.Delete(true);
            }
        }

        public static string GetUniqueOutputFilePath(string desiredOutputFilePath)
        {
            while(File.Exists(desiredOutputFilePath))
            {
                FileInfo desiredOutputFile = new FileInfo(desiredOutputFilePath);
                desiredOutputFilePath = Path.Combine(desiredOutputFile.Directory.FullName, desiredOutputFile.NameWithoutExtension() + "D" + desiredOutputFile.Extension);
            }
            return desiredOutputFilePath;
        }
    }

    public class FileProcessorConfiguration
    {
        public string SourceDir { get; set; }
        public string ProcessedDir { get; set; }
        public string WorkingDir { get; set; }
        public string GhostScriptPath { get; set; }
        public string TesseractPath { get; set; }
    }
}
