using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NetCarto.Map.WinForms.MapControls
{
    [Designer(typeof(Zoom))]
    [ToolboxBitmap(typeof(Button))]
    public partial class Zoom : UserControl, IMapControl
    {
        private CartoMap Map { get; set; }

        public Zoom()
        {
            InitializeComponent();
        }

        public Zoom(CartoMap map) : this()
        {
            Map = map;
        }

        public void AddToMap(CartoMap map)
        {
            Map = map;
        }

        private void btnZoomPlus_Click(object sender, EventArgs e)
        {
            this.Map.Map_SumZoom(1);
        }

        private void btnZoomMinus_Click(object sender, EventArgs e)
        {
            this.Map.Map_SumZoom(-1);
        }
    }
}
