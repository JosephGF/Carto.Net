namespace NetCarto.Map.WinForms.MapControls
{
    partial class Zoom
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnZoomPlus = new System.Windows.Forms.Button();
            this.btnZoomMinus = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnZoomPlus
            // 
            this.btnZoomPlus.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnZoomPlus.ForeColor = System.Drawing.Color.DimGray;
            this.btnZoomPlus.Location = new System.Drawing.Point(0, 0);
            this.btnZoomPlus.Name = "btnZoomPlus";
            this.btnZoomPlus.Size = new System.Drawing.Size(24, 24);
            this.btnZoomPlus.TabIndex = 0;
            this.btnZoomPlus.Text = "+";
            this.btnZoomPlus.UseVisualStyleBackColor = false;
            this.btnZoomPlus.Click += new System.EventHandler(this.btnZoomPlus_Click);
            // 
            // btnZoomMinus
            // 
            this.btnZoomMinus.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnZoomMinus.ForeColor = System.Drawing.Color.DimGray;
            this.btnZoomMinus.Location = new System.Drawing.Point(0, 24);
            this.btnZoomMinus.Name = "btnZoomMinus";
            this.btnZoomMinus.Size = new System.Drawing.Size(24, 24);
            this.btnZoomMinus.TabIndex = 0;
            this.btnZoomMinus.Text = "-";
            this.btnZoomMinus.UseVisualStyleBackColor = false;
            this.btnZoomMinus.Click += new System.EventHandler(this.btnZoomMinus_Click);
            // 
            // Zoom
            // 
            this.Controls.Add(this.btnZoomMinus);
            this.Controls.Add(this.btnZoomPlus);
            this.Name = "Zoom";
            this.Size = new System.Drawing.Size(24, 48);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnZoomPlus;
        private System.Windows.Forms.Button btnZoomMinus;
    }
}
