using System;
using NetCarto.Map.Common;
using NetCarto.Map.WinForms.Designer.Layers.Objects;
using NetCarto.Map.WinForms.Designer.Map.Objects;

namespace NetCarto.Map.WinForms.Designer.Layers
{
    public partial class FormDesignerLayers : FormDesignerBase
    {
        public MapOptionsDesigner _instance = new MapOptionsDesigner();

        public FormDesignerLayers(object mapOptions) : base()
        {
            //this.cdbMapPreview.IgnoreDesignMode = true;
            this._instance = mapOptions as MapOptionsDesigner;
            InitializeComponent();
        }

        protected CartoLayerDesigner CreateInstance()
        {
            System.Windows.Forms.MessageBox.Show(System.Windows.Forms.Application.StartupPath);
            var item = new CartoLayerDesigner() { Type = Common.Layers.TileLayer.CartoLayers.Carto_Positron };
            OnInstanceCreated(item);
            this.Add(item);
            return item;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var l = this.CreateInstance();
            this.treeItems.Nodes["Main"].Nodes.Add(l.Name);
        }

        private void tabContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabContainer.SelectedIndex == 1)
            {
                this.cdbMapPreview.Refresh(this._instance);
            }
        }
    }
}