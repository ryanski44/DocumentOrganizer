using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace DocumentOrganizerUI
{
    public partial class FilterAndSaveDialog : Form
    {
        private static FileInfo filterFile = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DocOrganizer", "filter.json"));

        private List<Filter> filters;
        public FilterAndSaveDialog()
        {
            InitializeComponent();
        }

        public string PdfText { get { return textBoxPdfText.Text; } set { textBoxPdfText.Text = value; } }

        public string BaseTargetDir { get; set; }

        public string OutputFilePath { get { return textBoxOutputFilePath.Text; } set { textBoxOutputFilePath.Text = value; } }

        private void FilterAndSaveControl_Load(object sender, EventArgs e)
        {
            filters = new List<Filter>();
            
            if(filterFile.Exists)
            {
                filters = JsonConvert.DeserializeObject<List<Filter>>(File.ReadAllText(filterFile.FullName));
            }

            foreach(var filter in filters)
            {
                List<string> captures = new List<string>();
                
                if (CheckFilter(filter.Regexes) != null)
                {
                    textBoxRegexFilter.Text = String.Join(Environment.NewLine, filter.Regexes);
                    textBoxFilterOutputPath.Text = filter.FilterOutputPath;
                    textBoxOutputFilePath.Text = textBoxResultingFilePath.Text;
                    break;
                }
            }

            this.AcceptButton = buttonSave;
        }

        private void textBoxRegexFilter_TextChanged(object sender, EventArgs e)
        {
            CalculateResulting();
        }

        private void textBoxFilterOutputPath_TextChanged(object sender, EventArgs e)
        {
            CalculateResulting();
        }

        private List<string> CheckFilter(string filter)
        {
            List<string> captures = new List<string>();
            foreach (var regexPattern in filter.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                Match m = Regex.Match(PdfText, regexPattern);
                if (m.Success)
                {
                    foreach (var capture in m.Groups.OfType<Group>().Skip(1).Select(g => g.Value))
                    {
                        captures.Add(capture);
                    }
                }
                else
                {
                    return null;
                }
            }
            return captures;
        }

        private void CalculateResulting()
        {
            try
            {
                var captures = CheckFilter(textBoxRegexFilter.Text);

                if (captures != null)
                {
                    textBoxResultingFilePath.Text = Path.Combine(BaseTargetDir, String.Format(textBoxFilterOutputPath.Text, captures.Cast<object>().ToArray()));
                    buttonSaveFilter.Enabled = true;
                }
                else
                {
                    textBoxResultingFilePath.Text = "No Match";
                    buttonSaveFilter.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                textBoxResultingFilePath.Text = ex.Message;
                buttonSaveFilter.Enabled = false;
            }
        }

        private void buttonSaveFilter_Click(object sender, EventArgs e)
        {
            buttonSaveFilter.Enabled = false;
            filters.Add(new Filter() { FilterOutputPath = textBoxFilterOutputPath.Text, Regexes = textBoxRegexFilter.Text });

            if (!filterFile.Directory.Exists)
            {
                filterFile.Directory.Create();
            }
            File.WriteAllText(filterFile.FullName, JsonConvert.SerializeObject(filters));
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            
            var dlg1 = new SaveFileDialog();
            if (!String.IsNullOrEmpty(textBoxOutputFilePath.Text))
            {
                FileInfo targetFile = new FileInfo(textBoxOutputFilePath.Text);
                dlg1.FileName = targetFile.Name;
                dlg1.InitialDirectory = targetFile.Directory.FullName;
            }

            var result = dlg1.ShowDialog();

            if (result == DialogResult.OK)
            {
                textBoxOutputFilePath.Text = dlg1.FileName;
            }
        }
    }
}
