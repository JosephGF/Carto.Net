using System;
using NetCarto.Core.Extensions;
using NetCarto.Core.ComponentModel;
using System.Reflection;
namespace NetCarto.Map.Common.Layers
{
    public class TileLayer : BaseLayer
    {
        public enum CartoLayers
        {
            [Data("http://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png")]
            Carto_Positron,
            [Data("http://{s}.basemaps.cartocdn.com/dark_all/{z}/{x}/{y}.png")]
            Carto_DarkMatter,
            [Data("http://{s}.basemaps.cartocdn.com/light_nolabels/{z}/{x}/{y}.png")]
            Carto_PositronLiteRainbow,
            [Data("http://{s}.basemaps.cartocdn.com/dark_nolabels/{z}/{x}/{y}.png")]
            Carto_DarkMatterLiteRainbow,
            [Data("https://cartocdn_{s}.global.ssl.fastly.net/base-antique/{z}/{x}/{y}.png")]
            Carto_DefaultAntique,
            [Data("https://cartocdn_{s}.global.ssl.fastly.net/base-eco/{z}/{x}/{y}.png")]
            Carto_Eco,
            [Data("https://cartocdn_{s}.global.ssl.fastly.net/base-flatblue/{z}/{x}/{y}.png")]
            Carto_FlatBlue,
            [Data("https://cartocdn_{s}.global.ssl.fastly.net/base-midnight/{z}/{x}/{y}.png")]
            Carto_Midnight
        }
        
        public string Url { get; set; }
        public override ILayerOptions Options { get; protected set; }
        public override string Type { get { return "tile"; } }

        public TileLayer()
        {
            this.Options = new TileLayerOptions();
        }

        public TileLayer(string url)
        {
            this.Url = url;
            this.Options = new TileLayerOptions();
        }

        public TileLayer(string url, TileLayerOptions options)
        {
            this.Url = url;
            this.Options = options;
        }

        public TileLayer(CartoLayers layerType, TileLayerOptions options)
        {
            this.Options = options;
            this.Url = GetDescription(layerType);
        }

        public TileLayer(CartoLayers layerType)
        {
            this.Options = new TileLayerOptions();
            this.Url = GetDescription(layerType);
        }

        private string GetDescription(CartoLayers layerType)
        {
            FieldInfo fi = layerType.GetType().GetRuntimeField(layerType.ToString());
            return fi.GetCustomAttribute<DataAttribute>(true).Data;
        }

        public override string Create()
        {
            return String.Format("L.tileLayer('{0}', {1}).addTo(net.cartodb.map)", this.Url, this.Options.ToJson());
        }

        public override string ToString()
        {
            return "Tile Layers";
        }
    }
}
