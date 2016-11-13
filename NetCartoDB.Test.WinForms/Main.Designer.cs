namespace NetCarto.Test.WinForms
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            NetCarto.Map.WinForms.Designer.Layers.Objects.CartoLayerDesigner cartoDBLayerDesigner1 = new NetCarto.Map.WinForms.Designer.Layers.Objects.CartoLayerDesigner();
            this.button1 = new System.Windows.Forms.Button();
            this.map1 = new NetCarto.Map.WinForms.CartoMap();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(846, 581);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Console";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // map1
            // 
            this.map1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.map1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.map1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("map1.BackgroundImage")));
            this.map1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.map1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.map1.Location = new System.Drawing.Point(12, 12);
            this.map1.Name = "map1";
            this.map1.Options.DoubleClickZoom = true;
            cartoDBLayerDesigner1.Name = "Layer_02";
            cartoDBLayerDesigner1.Type = NetCarto.Map.Common.Layers.TileLayer.CartoLayers.Carto_DarkMatterLiteRainbow;
            this.map1.Options.Layers.AddRange(new NetCarto.Map.Common.Layers.ILayer[] {
            cartoDBLayerDesigner1});
            this.map1.Options.MaxZoom = 18;
            this.map1.Options.MinZoom = 0;
            this.map1.Options.ScrollWheelZoom = false;
            this.map1.Options.SRID = "3857";
            this.map1.Options.TouchZoom = true;
            this.map1.Options.Version = "1.0.0";
            this.map1.Options.Zoom = 6;
            this.map1.Options.ZoomControl = false;
            this.map1.Size = new System.Drawing.Size(828, 592);
            this.map1.TabIndex = 1;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 608);
            this.Controls.Add(this.map1);
            this.Controls.Add(this.button1);
            this.Name = "Main";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private Map.WinForms.CartoMap map1;
    }
}

