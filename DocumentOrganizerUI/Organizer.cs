using DocumentOrganizerUI.Extensions;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocumentOrganizerUI
{
    public partial class Organizer : Form
    {
        private DirectoryInfo source;
        private DirectoryInfo processed;
        private DirectoryInfo target;
        private List<Document> documents;
        private int documentsIndex;
        private List<int> selectedImages;
        private string ghostScriptPath;

        public Organizer()
        {
            InitializeComponent();
            selectedImages = new List<int>();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async Task LoadFolderAsync()
        {
            selectedImages.Clear();
            var tasks = new List<Task<Document>>();
            foreach(var directory in processed.GetDirectories())
            {
                tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        Document doc = Document.FromDirectory(directory);
                        return doc;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return null;
                    }
                }));
            }
            documents = new List<Document>();
            foreach (var task in tasks)
            {
                Document doc = await task;
                if(doc != null)
                {
                    documents.Add(doc);
                }
            }
            documents = documents.OrderBy(x => x.OriginalFile.Name).ToList();
            documentsIndex = 0;
            LoadPicture();
            LoadPreviews();
        }

        private void LoadPicture()
        {
            if (documents.Count > documentsIndex)
            {
                Document doc = documents[documentsIndex];
                pictureBox.ImageLocation = doc.Preview300File.FullName;
                toolStripStatusLabelText.Text = doc.OriginalFile.NameWithoutExtension();
            }
            else
            {
                pictureBox.ImageLocation = null;
            }
        }

        private Task NextImageAsync()
        {
            if(documentsIndex < documents.Count - 1)
            {
                documentsIndex++;
                LoadPicture();
            }
            return Task.CompletedTask;
        }

        private Task PrevImageAsync()
        {
            if(documentsIndex > 0)
            {
                documentsIndex--;
                LoadPicture();
            }
            return Task.CompletedTask;
        }

        private Task SelectCurrentImage()
        {
            if (selectedImages.Contains(documentsIndex))
            {
                selectedImages.Remove(documentsIndex);
            }
            else
            {
                selectedImages.Add(documentsIndex);
            }
            LoadPreviews();
            return Task.CompletedTask;
        }

        private async Task ProcessAsync()
        {
            if (selectedImages.Any())
            {
                var docsToProcess = selectedImages.Select(i => documents[i]).ToList();
                selectedImages.Clear();
                LoadPreviews();

                Task<string> processingTask = Task.Run(() =>
                {
                    if (docsToProcess.Count > 1)
                    {
                        //combine
                        var paths = docsToProcess.Select(doc => doc.ProcessedFile.FullName).ToList();
                        string inputFilesCommandLine = String.Join(" ", paths.Select(p => $"\"{p}\""));
                        var combinedFilePath = Path.GetTempFileName();
                        OCRProcessor.RunSilentProcess(ghostScriptPath, processed.FullName, $"-q -dNOPAUSE -dBATCH -sDEVICE=pdfwrite -dPDFSETTINGS=/prepress -sOutputFile=\"{combinedFilePath}\" {inputFilesCommandLine}");
                        
                        return combinedFilePath;
                    }
                    else
                    {
                        return docsToProcess.First().ProcessedFile.FullName;
                    }
                });

                StringBuilder textBuilder = new StringBuilder();

                foreach(var doc in docsToProcess)
                {
                    string text = File.ReadAllText(doc.PreviewTextFile.FullName);
                    text = text.Replace("\n", "\r\n");
                    textBuilder.AppendLine(text);
                }

                var dlg1 = new FilterAndSaveDialog
                {
                    PdfText = textBuilder.ToString(),
                    BaseTargetDir = target.FullName,
                    OutputFilePath = Path.Combine(target.FullName, docsToProcess.First().ProcessedFile.Name)
                };

                var result = dlg1.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        var targetFile = new FileInfo(dlg1.OutputFilePath);
                        if (!targetFile.Directory.Exists)
                        {
                            targetFile.Directory.Create();
                        }
                        string sourceFilePath = await processingTask;
                        string uniqueOutputPath = OCRProcessor.GetUniqueOutputFilePath(targetFile.FullName);
                        File.Move(sourceFilePath, uniqueOutputPath);

                        //cleanup
                        foreach (var doc in docsToProcess)
                        {
                            doc.Delete();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    await LoadFolderAsync();
                }
            }
        }

        private void LoadPreviews()
        {
            flowLayoutPanelLeft.Controls.Clear();
            
            foreach(int index in selectedImages)
            {
                PictureBox pb = new PictureBox();
                pb.SizeMode = PictureBoxSizeMode.Zoom;
                pb.Width = flowLayoutPanelLeft.Width;
                pb.Height = (int)(pb.Width * 1.3);
                pb.ImageLocation = documents[index].Preview50File.FullName;
                pb.Click += new EventHandler((o, i) => { documentsIndex = index; LoadPicture(); });
                flowLayoutPanelLeft.Controls.Add(pb);
            }
        }

        private async void Organizer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D)
            {
                await NextImageAsync();
            }
            else if (e.KeyCode == Keys.A)
            {
                await PrevImageAsync();
            }
            else if (e.KeyCode == Keys.S)
            {
                await SelectCurrentImage();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                await ProcessAsync();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                pictureBox.ImageLocation = null;
                var doc = documents[documentsIndex];
                doc.Delete();

                await LoadFolderAsync();
            }
            else if (e.KeyCode == Keys.R)
            {
                var doc = documents[documentsIndex];
                //only supported for image sources
                if (doc.OriginalFile.Extension != ".pdf")
                {
                    RotateDialog rotateDialog = new RotateDialog(doc);

                    var result = rotateDialog.ShowDialog(this);

                    if (result == DialogResult.OK)
                    {
                        int rotation = rotateDialog.Rotation;
                        if (rotation != 0)
                        {
                            try
                            {
                                using (Image i = Image.FromFile(doc.OriginalFile.FullName))
                                {
                                    if (rotation == 90)
                                    {
                                        i.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                    }
                                    else if (rotation == 180)
                                    {
                                        i.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                    }
                                    else if (rotation == 270)
                                    {
                                        i.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                    }
                                    else
                                    {
                                        throw new Exception($"Unexpected rotation of {rotation}");
                                    }
                                    i.Save(Path.Combine(source.FullName, doc.OriginalFile.NameWithoutExtension() + ".png"));
                                }
                                pictureBox.ImageLocation = null;
                                doc.Delete();
                                await LoadFolderAsync();
                            } 
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
            }
        }

        private OCRProcessor processor;

        private async void Organizer_Load(object sender, EventArgs e)
        {
            source = new DirectoryInfo(ConfigurationManager.AppSettings["sourceDir"]);
            target = new DirectoryInfo(ConfigurationManager.AppSettings["outputDir"]);
            processed = new DirectoryInfo(ConfigurationManager.AppSettings["processedDir"]);
            var working = new DirectoryInfo(ConfigurationManager.AppSettings["workingDir"]);
            toolStripStatusLabelText.Text = "Loading...";
            var progress = new Progress<string>(s => toolStripStatusLabelText.Text = s);
            await LoadFolderAsync();

            ghostScriptPath = ConfigurationManager.AppSettings["gsPath"];

            processor = new OCRProcessor(new FileProcessorConfiguration()
            {
                SourceDir = source.FullName,
                ProcessedDir = processed.FullName,
                WorkingDir = working.FullName,
                GhostScriptPath = ghostScriptPath,
                TesseractPath = ConfigurationManager.AppSettings["tesseractPath"]
            },
            progress: new Progress<Tuple<int, string>>(statusUpdate =>
            {
                int runnerId = statusUpdate.Item1;
                string updateText = statusUpdate.Item2;
                try
                {
                    Invoke((MethodInvoker)delegate
                    {
                        while (listViewStatus.Items.Count < runnerId)
                        {
                            listViewStatus.Items.Add("");
                        }
                        if (listViewStatus.Items.Count == runnerId)
                        {
                            listViewStatus.Items.Add(updateText);
                        }
                        else
                        {
                            listViewStatus.Items[runnerId].Text = updateText;
                        }
                        listViewStatus.Update();
                    });
                }
                catch { }
            }));

            processor.Start();
        }

        private async void Organizer_FormClosing(object sender, FormClosingEventArgs e)
        {
            await processor.StopAsync();
        }

        private async void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await LoadFolderAsync();
        }

        private void toggleStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
        }
    }
}
