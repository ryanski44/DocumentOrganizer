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
        private Task loadingTask;
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
        }

        private void LoadPicture()
        {
            if (files.Length > filesIndex)
            {
                FileInfo pdfFile = files[filesIndex];
                pictureBox.ImageLocation = Path.Combine(processed.FullName, "previews", "300", pdfFile.NameWithoutExtension() + ".jpg");
                toolStripStatusLabelText.Text = pdfFile.NameWithoutExtension();
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
            await LoadPreviews();
        }

        private async Task ProcessAsync()
        {
            var filesToProcess = selectedImages.ToList();
            selectedImages.Clear();
            await LoadPreviews();

            foreach(var fileId in filesToProcess)
            {
                var file = files[fileId];
                //TODO: window to enter new details
                //move file to output
                //delete previews
                //TODO: add text to previews
                //use text to guess file name
                //hardcode rules here
            }
        }

        private async Task LoadPreviews()
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
    }
}
