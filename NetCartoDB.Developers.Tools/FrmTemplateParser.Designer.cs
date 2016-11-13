namespace NetCarto.Developers.Tools
{
    partial class FrmTemplateParser
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
            this.lbTemplates = new System.Windows.Forms.ListBox();
            this.rtbPreview = new System.Windows.Forms.RichTextBox();
            this.txPath = new System.Windows.Forms.TextBox();
            this.btnLoadCsv = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbTemplates
            // 
            this.lbTemplates.FormattingEnabled = true;
            this.lbTemplates.Location = new System.Drawing.Point(12, 12);
            this.lbTemplates.Name = "lbTemplates";
            this.lbTemplates.Size = new System.Drawing.Size(246, 615);
            this.lbTemplates.TabIndex = 0;
            // 
            // rtbPreview
            // 
            this.rtbPreview.Location = new System.Drawing.Point(265, 39);
            this.rtbPreview.Name = "rtbPreview";
            this.rtbPreview.Size = new System.Drawing.Size(750, 588);
            this.rtbPreview.TabIndex = 1;
            this.rtbPreview.Text = "";
            // 
            // txPath
            // 
            this.txPath.Location = new System.Drawing.Point(265, 12);
            this.txPath.Name = "txPath";
            this.txPath.Size = new System.Drawing.Size(669, 20);
            this.txPath.TabIndex = 2;
            // 
            // btnLoadCsv
            // 
            this.btnLoadCsv.Location = new System.Drawing.Point(940, 10);
            this.btnLoadCsv.Name = "btnLoadCsv";
            this.btnLoadCsv.Size = new System.Drawing.Size(75, 23);
            this.btnLoadCsv.TabIndex = 3;
            this.btnLoadCsv.Text = "Load";
            this.btnLoadCsv.UseVisualStyleBackColor = true;
            this.btnLoadCsv.Click += new System.EventHandler(this.btnLoadCsv_Click);
            // 
            // FrmTemplateParser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 644);
            this.Controls.Add(this.btnLoadCsv);
            this.Controls.Add(this.txPath);
            this.Controls.Add(this.rtbPreview);
            this.Controls.Add(this.lbTemplates);
            this.Name = "FrmTemplateParser";
            this.Text = "Template Parser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbTemplates;
        private System.Windows.Forms.RichTextBox rtbPreview;
        private System.Windows.Forms.TextBox txPath;
        private System.Windows.Forms.Button btnLoadCsv;
    }
}

