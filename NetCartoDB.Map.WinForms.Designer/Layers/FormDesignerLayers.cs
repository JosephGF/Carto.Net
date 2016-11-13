using NetCartoDB.Map.WinForms.Designer.Layers.Objects;
using NetCartoDB.Map.Common;
using System;

namespace NetCartoDB.Map.WinForms.Designer.Layers
{
    public partial class FormDesignerLayers : FormDesignerBase
    {
        public MapOptions _parent = new MapOptions();

        public FormDesignerLayers(object mapOptions) : base()
        {
            this._parent = mapOptions as MapOptions;
            InitializeComponent();
        }

        protected CartoDBLayerDesigner CreateInstance()
        {
            var item = new CartoDBLayerDesigner() { Type = Common.Layers.TileLayer.CartoDBLayers.CartoDB_Positron };
            OnInstanceCreated(item);
            return item;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Add(this.CreateInstance());
        }

        private void tabContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabContainer.SelectedIndex == 1)
            {
                this.mapPreview.Refresh(_parent);
            }
        }
    }
}
