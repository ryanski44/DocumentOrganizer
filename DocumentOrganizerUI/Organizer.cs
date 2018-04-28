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
        private FileInfo[] files;
        private int filesIndex;
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
            files = await Task.Run(() => processed.GetAllFiles().Where(x => x.Extension == ".pdf").ToArray());
            filesIndex = 0;
            LoadPicture();
            LoadPreviews();
        }

        private void LoadPicture()
        {
            if (files.Length > filesIndex)
            {
                FileInfo pdfFile = files[filesIndex];
                pictureBox.ImageLocation = Path.Combine(processed.FullName, "previews", "300", pdfFile.NameWithoutExtension() + ".jpg");
                toolStripStatusLabelText.Text = pdfFile.NameWithoutExtension();
            }
            else
            {
                pictureBox.ImageLocation = null;
            }
        }

        private async Task NextImageAsync()
        {
            if(filesIndex < files.Length - 1)
            {
                filesIndex++;
                LoadPicture();
            }
        }

        private async Task PrevImageAsync()
        {
            if(filesIndex > 0)
            {
                filesIndex--;
                LoadPicture();
            }
        }

        private async Task SelectCurrentImage()
        {
            if (selectedImages.Contains(filesIndex))
            {
                selectedImages.Remove(filesIndex);
            }
            else
            {
                selectedImages.Add(filesIndex);
            }
            LoadPreviews();
        }

        private async Task ProcessAsync()
        {
            if (selectedImages.Any())
            {
                var filesToProcess = selectedImages.Select(i => files[i]).ToList();
                selectedImages.Clear();
                LoadPreviews();

                Task<string> processingTask = Task.Run(() =>
                {
                    if (filesToProcess.Count > 1)
                    {
                        //combine
                        var paths = filesToProcess.Select(f => f.FullName).ToList();
                        string inputFilesCommandLine = String.Join(" ", paths.Select(p => $"\"{p}\""));
                        var combinedFilePath = Path.GetTempFileName();
                        OCRProcessor.RunSilentProcess(ghostScriptPath, processed.FullName, $"-q -dNOPAUSE -dBATCH -sDEVICE=pdfwrite -dPDFSETTINGS=/prepress -sOutputFile=\"{combinedFilePath}\" {inputFilesCommandLine}");
                        
                        return combinedFilePath;
                    }
                    else
                    {
                        return filesToProcess.First().FullName;
                    }
                });

                string text = File.ReadAllText(Path.Combine(processed.FullName, "previews", "text", filesToProcess.First().NameWithoutExtension() + ".txt"));
                text = text.Replace("\n", "\r\n");

                var dlg1 = new FilterAndSaveDialog
                {
                    PdfText = text,
                    BaseTargetDir = target.FullName,
                    OutputFilePath = Path.Combine(target.FullName, filesToProcess.First().Name)
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
                        foreach (var file in filesToProcess)
                        {
                            File.Delete(file.FullName);
                            //delete previews
                            File.Delete(Path.Combine(processed.FullName, "previews", "50", file.NameWithoutExtension() + ".jpg"));
                            File.Delete(Path.Combine(processed.FullName, "previews", "300", file.NameWithoutExtension() + ".jpg"));
                            File.Delete(Path.Combine(processed.FullName, "previews", "text", file.NameWithoutExtension() + ".txt"));
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
                pb.ImageLocation = Path.Combine(processed.FullName, "previews", "50", files[index].NameWithoutExtension() + ".jpg");
                pb.Click += new EventHandler((o, i) => { filesIndex = index; LoadPicture(); });
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
            else if(e.KeyCode == Keys.Enter)
            {
                await ProcessAsync();
            }
            else if(e.KeyCode == Keys.Delete)
            {
                var file = files[filesIndex];
                File.Delete(file.FullName);
                //delete previews
                File.Delete(Path.Combine(processed.FullName, "previews", "50", file.NameWithoutExtension() + ".jpg"));
                File.Delete(Path.Combine(processed.FullName, "previews", "300", file.NameWithoutExtension() + ".jpg"));
                File.Delete(Path.Combine(processed.FullName, "previews", "text", file.NameWithoutExtension() + ".txt"));

                await LoadFolderAsync();
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
            });

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
    }
}
