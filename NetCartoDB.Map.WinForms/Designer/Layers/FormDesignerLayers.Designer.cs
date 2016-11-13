namespace NetCarto.Map.WinForms.Designer.Layers
{
    partial class FormDesignerLayers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDesignerLayers));
            NetCarto.Map.WinForms.Designer.Layers.Objects.CartoLayerDesigner cartoDBLayerDesigner1 = new NetCarto.Map.WinForms.Designer.Layers.Objects.CartoLayerDesigner();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Main");
            this.tabContainer = new System.Windows.Forms.TabControl();
            this.tabOptions = new System.Windows.Forms.TabPage();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.tabPreview = new System.Windows.Forms.TabPage();
            this.cdbMapPreview = new NetCarto.Map.WinForms.CartoMap();
            this.pnLeftTop = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.treeItems = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnBottom.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabContainer.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.tabPreview.SuspendLayout();
            this.pnLeftTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeItems);
            this.splitContainer1.Panel1.Controls.Add(this.pnLeftTop);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabContainer);
            // 
            // tabContainer
            // 
            this.tabContainer.Controls.Add(this.tabOptions);
            this.tabContainer.Controls.Add(this.tabPreview);
            this.tabContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabContainer.Location = new System.Drawing.Point(0, 0);
            this.tabContainer.Name = "tabContainer";
            this.tabContainer.SelectedIndex = 0;
            this.tabContainer.Size = new System.Drawing.Size(554, 509);
            this.tabContainer.TabIndex = 1;
            this.tabContainer.SelectedIndexChanged += new System.EventHandler(this.tabContainer_SelectedIndexChanged);
            // 
            // tabOptions
            // 
            this.tabOptions.Controls.Add(this.propertyGrid1);
            this.tabOptions.Location = new System.Drawing.Point(4, 22);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabOptions.Size = new System.Drawing.Size(546, 483);
            this.tabOptions.TabIndex = 0;
            this.tabOptions.Text = "Options";
            this.tabOptions.UseVisualStyleBackColor = true;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(540, 477);
            this.propertyGrid1.TabIndex = 0;
            // 
            // tabPreview
            // 
            this.tabPreview.Controls.Add(this.cdbMapPreview);
            this.tabPreview.Location = new System.Drawing.Point(4, 22);
            this.tabPreview.Name = "tabPreview";
            this.tabPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPreview.Size = new System.Drawing.Size(546, 483);
            this.tabPreview.TabIndex = 1;
            this.tabPreview.Text = "Preview";
            this.tabPreview.UseVisualStyleBackColor = true;
            // 
            // cdbMapPreview
            // 
            this.cdbMapPreview.BackColor = System.Drawing.SystemColors.ControlDark;
            this.cdbMapPreview.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cdbMapPreview.BackgroundImage")));
            this.cdbMapPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.cdbMapPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cdbMapPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cdbMapPreview.Location = new System.Drawing.Point(3, 3);
            this.cdbMapPreview.Name = "cdbMapPreview";

            this.cdbMapPreview.Options.DoubleClickZoom = true;
            cartoDBLayerDesigner1.Name = "Layer_01";
            cartoDBLayerDesigner1.Type = NetCarto.Map.Common.Layers.TileLayer.CartoLayers.Carto_Positron;
            this.cdbMapPreview.Options.Layers.AddRange(new NetCarto.Map.Common.Layers.ILayer[] {
            cartoDBLayerDesigner1});
            this.cdbMapPreview.Options.MaxZoom = 18;
            this.cdbMapPreview.Options.MinZoom = 0;
            this.cdbMapPreview.Options.ScrollWheelZoom = true;
            this.cdbMapPreview.Options.SRID = "3857";
            this.cdbMapPreview.Options.TouchZoom = true;
            this.cdbMapPreview.Options.Version = "1.0.0";
            this.cdbMapPreview.Options.Zoom = 6;
            this.cdbMapPreview.Options.ZoomControl = false;
            this.cdbMapPreview.Size = new System.Drawing.Size(540, 477);
            this.cdbMapPreview.TabIndex = 0;
            // 
            // pnLeftTop
            // 
            this.pnLeftTop.Controls.Add(this.btnAdd);
            this.pnLeftTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnLeftTop.Location = new System.Drawing.Point(0, 0);
            this.pnLeftTop.Name = "pnLeftTop";
            this.pnLeftTop.Size = new System.Drawing.Size(239, 31);
            this.pnLeftTop.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(5, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // treeItems
            // 
            this.treeItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeItems.Location = new System.Drawing.Point(5, 37);
            this.treeItems.Name = "treeItems";
            treeNode1.Name = "Main";
            treeNode1.Text = "Main";
            this.treeItems.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeItems.Size = new System.Drawing.Size(229, 466);
            this.treeItems.TabIndex = 1;
            // 
            // FormDesignerLayers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 539);
            this.Name = "FormDesignerLayers";
            this.Text = "FormDesignerLayers";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnBottom.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabContainer.ResumeLayout(false);
            this.tabOptions.ResumeLayout(false);
            this.tabPreview.ResumeLayout(false);
            this.pnLeftTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabContainer;
        private System.Windows.Forms.TabPage tabOptions;
        private System.Windows.Forms.TabPage tabPreview;
        private CartoMap cdbMapPreview;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TreeView treeItems;
        private System.Windows.Forms.Panel pnLeftTop;
        private System.Windows.Forms.Button btnAdd;
    }
}