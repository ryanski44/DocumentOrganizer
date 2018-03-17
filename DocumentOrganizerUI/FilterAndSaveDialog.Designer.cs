namespace DocumentOrganizerUI
{
    partial class FilterAndSaveDialog
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelPdfText = new System.Windows.Forms.Label();
            this.textBoxPdfText = new System.Windows.Forms.TextBox();
            this.labelFilterRegex = new System.Windows.Forms.Label();
            this.textBoxRegexFilter = new System.Windows.Forms.TextBox();
            this.labelResultingFileName = new System.Windows.Forms.Label();
            this.textBoxResultingFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxOutputFilePath = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.labelFilterOutputPath = new System.Windows.Forms.Label();
            this.textBoxFilterOutputPath = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSaveFilter = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.labelPdfText, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxPdfText, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelFilterRegex, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxRegexFilter, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelResultingFileName, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.textBoxResultingFilePath, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.labelFilterOutputPath, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.textBoxFilterOutputPath, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 10);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 11;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(954, 985);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // labelPdfText
            // 
            this.labelPdfText.AutoSize = true;
            this.labelPdfText.Location = new System.Drawing.Point(4, 0);
            this.labelPdfText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPdfText.Name = "labelPdfText";
            this.labelPdfText.Size = new System.Drawing.Size(92, 25);
            this.labelPdfText.TabIndex = 0;
            this.labelPdfText.Text = "Pdf Text";
            // 
            // textBoxPdfText
            // 
            this.textBoxPdfText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPdfText.Location = new System.Drawing.Point(4, 29);
            this.textBoxPdfText.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxPdfText.Multiline = true;
            this.textBoxPdfText.Name = "textBoxPdfText";
            this.textBoxPdfText.ReadOnly = true;
            this.textBoxPdfText.Size = new System.Drawing.Size(946, 433);
            this.textBoxPdfText.TabIndex = 1;
            // 
            // labelFilterRegex
            // 
            this.labelFilterRegex.AutoSize = true;
            this.labelFilterRegex.Location = new System.Drawing.Point(4, 466);
            this.labelFilterRegex.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFilterRegex.Name = "labelFilterRegex";
            this.labelFilterRegex.Size = new System.Drawing.Size(128, 25);
            this.labelFilterRegex.TabIndex = 2;
            this.labelFilterRegex.Text = "Filter Regex";
            // 
            // textBoxRegexFilter
            // 
            this.textBoxRegexFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRegexFilter.Location = new System.Drawing.Point(4, 495);
            this.textBoxRegexFilter.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxRegexFilter.Multiline = true;
            this.textBoxRegexFilter.Name = "textBoxRegexFilter";
            this.textBoxRegexFilter.Size = new System.Drawing.Size(946, 212);
            this.textBoxRegexFilter.TabIndex = 3;
            this.textBoxRegexFilter.TextChanged += new System.EventHandler(this.textBoxRegexFilter_TextChanged);
            // 
            // labelResultingFileName
            // 
            this.labelResultingFileName.AutoSize = true;
            this.labelResultingFileName.Location = new System.Drawing.Point(4, 775);
            this.labelResultingFileName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelResultingFileName.Name = "labelResultingFileName";
            this.labelResultingFileName.Size = new System.Drawing.Size(193, 25);
            this.labelResultingFileName.TabIndex = 4;
            this.labelResultingFileName.Text = "Resulting File Path";
            // 
            // textBoxResultingFilePath
            // 
            this.textBoxResultingFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxResultingFilePath.Location = new System.Drawing.Point(4, 804);
            this.textBoxResultingFilePath.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxResultingFilePath.Name = "textBoxResultingFilePath";
            this.textBoxResultingFilePath.ReadOnly = true;
            this.textBoxResultingFilePath.Size = new System.Drawing.Size(946, 31);
            this.textBoxResultingFilePath.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 839);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Output File Path";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.textBoxOutputFilePath, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonBrowse, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 868);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(946, 52);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // textBoxOutputFilePath
            // 
            this.textBoxOutputFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOutputFilePath.Location = new System.Drawing.Point(4, 4);
            this.textBoxOutputFilePath.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxOutputFilePath.Name = "textBoxOutputFilePath";
            this.textBoxOutputFilePath.Size = new System.Drawing.Size(797, 31);
            this.textBoxOutputFilePath.TabIndex = 0;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(809, 4);
            this.buttonBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(133, 44);
            this.buttonBrowse.TabIndex = 1;
            this.buttonBrowse.Text = "Browse";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // labelFilterOutputPath
            // 
            this.labelFilterOutputPath.AutoSize = true;
            this.labelFilterOutputPath.Location = new System.Drawing.Point(4, 711);
            this.labelFilterOutputPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFilterOutputPath.Name = "labelFilterOutputPath";
            this.labelFilterOutputPath.Size = new System.Drawing.Size(180, 25);
            this.labelFilterOutputPath.TabIndex = 8;
            this.labelFilterOutputPath.Text = "Filter Output Path";
            // 
            // textBoxFilterOutputPath
            // 
            this.textBoxFilterOutputPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFilterOutputPath.Location = new System.Drawing.Point(4, 740);
            this.textBoxFilterOutputPath.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxFilterOutputPath.Name = "textBoxFilterOutputPath";
            this.textBoxFilterOutputPath.Size = new System.Drawing.Size(946, 31);
            this.textBoxFilterOutputPath.TabIndex = 9;
            this.textBoxFilterOutputPath.TextChanged += new System.EventHandler(this.textBoxFilterOutputPath_TextChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.buttonSave);
            this.flowLayoutPanel1.Controls.Add(this.buttonCancel);
            this.flowLayoutPanel1.Controls.Add(this.buttonSaveFilter);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 928);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(946, 53);
            this.flowLayoutPanel1.TabIndex = 10;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(4, 4);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(133, 44);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(145, 4);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(133, 44);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSaveFilter
            // 
            this.buttonSaveFilter.Enabled = false;
            this.buttonSaveFilter.Location = new System.Drawing.Point(286, 4);
            this.buttonSaveFilter.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSaveFilter.Name = "buttonSaveFilter";
            this.buttonSaveFilter.Size = new System.Drawing.Size(133, 44);
            this.buttonSaveFilter.TabIndex = 2;
            this.buttonSaveFilter.Text = "Save Filter";
            this.buttonSaveFilter.UseVisualStyleBackColor = true;
            this.buttonSaveFilter.Click += new System.EventHandler(this.buttonSaveFilter_Click);
            // 
            // FilterAndSaveDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 985);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FilterAndSaveDialog";
            this.Text = "Save File";
            this.Load += new System.EventHandler(this.FilterAndSaveControl_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelPdfText;
        private System.Windows.Forms.TextBox textBoxPdfText;
        private System.Windows.Forms.Label labelFilterRegex;
        private System.Windows.Forms.TextBox textBoxRegexFilter;
        private System.Windows.Forms.Label labelResultingFileName;
        private System.Windows.Forms.TextBox textBoxResultingFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox textBoxOutputFilePath;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Label labelFilterOutputPath;
        private System.Windows.Forms.TextBox textBoxFilterOutputPath;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSaveFilter;
    }
}
