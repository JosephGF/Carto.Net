using System;

namespace NetCartoDB.Map.WinForms.Design.Layers
{
    public partial class FormDesignerLayers : FormDesignerBase
    {
        public FormDesignerLayers() : base()
        {
            InitializeComponent();
        }

        protected Layers.Objects.WinFormsCartoDBLayer CreateInstance()
        {
            var item = new Layers.Objects.WinFormsCartoDBLayer() { Type = Common.Layers.TileLayer.CartoDBLayers.CartoDB_Positron };
            OnInstanceCreated(item);
            return item;
            //return base.CreateInstance(type);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Add(this.CreateInstance());
            System.Windows.Forms.MessageBox.Show("Items " + this.Items.Count);
        }
    }
}
