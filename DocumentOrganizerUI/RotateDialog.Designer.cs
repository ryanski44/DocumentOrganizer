namespace DocumentOrganizerUI
{
    partial class RotateDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonRotate90 = new System.Windows.Forms.Button();
            this.buttonRotate180 = new System.Windows.Forms.Button();
            this.buttonRotate270 = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBoxPreview);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 35;
            this.splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.buttonRotate90, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonRotate180, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonRotate270, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonSave, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 35);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonRotate90
            // 
            this.buttonRotate90.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRotate90.Location = new System.Drawing.Point(3, 3);
            this.buttonRotate90.Name = "buttonRotate90";
            this.buttonRotate90.Size = new System.Drawing.Size(194, 29);
            this.buttonRotate90.TabIndex = 0;
            this.buttonRotate90.Text = "Rotate 90";
            this.buttonRotate90.UseVisualStyleBackColor = true;
            this.buttonRotate90.Click += new System.EventHandler(this.buttonRotate90_Click);
            // 
            // buttonRotate180
            // 
            this.buttonRotate180.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRotate180.Location = new System.Drawing.Point(203, 3);
            this.buttonRotate180.Name = "buttonRotate180";
            this.buttonRotate180.Size = new System.Drawing.Size(194, 29);
            this.buttonRotate180.TabIndex = 1;
            this.buttonRotate180.Text = "Rotate 180";
            this.buttonRotate180.UseVisualStyleBackColor = true;
            this.buttonRotate180.Click += new System.EventHandler(this.buttonRotate180_Click);
            // 
            // buttonRotate270
            // 
            this.buttonRotate270.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRotate270.Location = new System.Drawing.Point(403, 3);
            this.buttonRotate270.Name = "buttonRotate270";
            this.buttonRotate270.Size = new System.Drawing.Size(194, 29);
            this.buttonRotate270.TabIndex = 2;
            this.buttonRotate270.Text = "Rotate 270";
            this.buttonRotate270.UseVisualStyleBackColor = true;
            this.buttonRotate270.Click += new System.EventHandler(this.buttonRotate270_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSave.Location = new System.Drawing.Point(603, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(194, 29);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxPreview.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new System.Drawing.Size(800, 411);
            this.pictureBoxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxPreview.TabIndex = 0;
            this.pictureBoxPreview.TabStop = false;
            // 
            // RotateDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "RotateDialog";
            this.Text = "Rotate";
            this.Load += new System.EventHandler(this.RotateDialog_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonRotate90;
        private System.Windows.Forms.Button buttonRotate180;
        private System.Windows.Forms.Button buttonRotate270;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.PictureBox pictureBoxPreview;
    }
}