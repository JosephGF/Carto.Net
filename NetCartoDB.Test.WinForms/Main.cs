using NetCarto.Map.Common;
using NetCarto.Map.Common.Layers;
using System;
using System.Windows.Forms;

namespace NetCarto.Test.WinForms
{
    public partial class Main : Form
    {
        /*private NetCarto.Map.WinForms.Map map;*/
        /*private NetCarto.Map.WinForms.MapControls.Zoom zoom;*/

        public Main()
        {
            InitializeComponent();

            /*MapOptions options = new MapOptions() {
                Layers = new ILayer[] {
                            new TileLayer(TileLayer.CartoLayers.Carto_PositronLiteRainbow, new TileLayerOptions() { Attribution = "Cartodb" }),
                            new TileLayer("https://stamen-tiles-{s}.a.ssl.fastly.net/toner-labels/{z}/{x}/{y}.png"),
                            new TileLayer("http://{s}.tile.openweathermap.org/map/pressure/{z}/{x}/{y}.png", new TileLayerOptions() { Opacity = 0.25}),
                       },
                ZoomControl = false,
            };*/

            /*map = new NetCarto.Map.WinForms.Map(options) { Dock = DockStyle.Fill };
            this.Controls.Add(map);*/
           //zoom = new NetCarto.Map.WinForms.Controls.Zoom(map);
           // this.Controls.Add(zoom);
           //zoom.BringToFront();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            map1.ShowDevTools();
        }
    }
}
