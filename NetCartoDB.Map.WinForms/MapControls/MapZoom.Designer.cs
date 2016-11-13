namespace NetCartoDB.Map.WinForms.Controls
{
    partial class MapZoom
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAddZoom = new System.Windows.Forms.Button();
            this.btnMinusZoom = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAddZoom
            // 
            this.btnAddZoom.BackColor = System.Drawing.Color.White;
            this.btnAddZoom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAddZoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddZoom.Location = new System.Drawing.Point(1, 1);
            this.btnAddZoom.Name = "btnAddZoom";
            this.btnAddZoom.Size = new System.Drawing.Size(26, 26);
            this.btnAddZoom.TabIndex = 0;
            this.btnAddZoom.Text = "+";
            this.btnAddZoom.UseVisualStyleBackColor = false;
            this.btnAddZoom.Click += new System.EventHandler(this.btnAddZoom_Click);
            // 
            // btnMinusZoom
            // 
            this.btnMinusZoom.BackColor = System.Drawing.Color.White;
            this.btnMinusZoom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnMinusZoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinusZoom.Location = new System.Drawing.Point(1, 28);
            this.btnMinusZoom.Name = "btnMinusZoom";
            this.btnMinusZoom.Size = new System.Drawing.Size(26, 26);
            this.btnMinusZoom.TabIndex = 1;
            this.btnMinusZoom.Text = "-";
            this.btnMinusZoom.UseVisualStyleBackColor = false;
            // 
            // MapZoom
            // 
            this.Controls.Add(this.btnMinusZoom);
            this.Controls.Add(this.btnAddZoom);
            this.Name = "MapZoom";
            this.Size = new System.Drawing.Size(28, 55);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddZoom;
        private System.Windows.Forms.Button btnMinusZoom;
    }
}
