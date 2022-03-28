using DocumentOrganizerUI.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocumentOrganizerUI
{
    public partial class RotateDialog : Form
    {
        private Document doc;
        private Image image;
        private int rotation;

        public RotateDialog(Document doc)
        {
            InitializeComponent();

            this.doc = doc;
            this.rotation = 0;
        }

        public int Rotation { get { return rotation; } }

        private void RotateDialog_Load(object sender, EventArgs e)
        {
            image = Image.FromFile(doc.Preview300File.FullName);
            pictureBoxPreview.Image = image;
            pictureBoxPreview.Disposed += (Object sndr, EventArgs evnt) => image.Dispose();
        }

        private void buttonRotate90_Click(object sender, EventArgs e)
        {
            rotation += 90;
            rotation = rotation % 360;
            pictureBoxPreview.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBoxPreview.Refresh();
        }

        private void buttonRotate180_Click(object sender, EventArgs e)
        {
            rotation += 180;
            rotation = rotation % 360;
            pictureBoxPreview.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
            pictureBoxPreview.Refresh();
        }

        private void buttonRotate270_Click(object sender, EventArgs e)
        {
            rotation += 270;
            rotation = rotation % 360;
            pictureBoxPreview.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            pictureBoxPreview.Refresh();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
