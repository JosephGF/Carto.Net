using System;
using NetCarto.Core.Extensions;

namespace NetCarto.Map.Common.Layers
{
    public class WmsLayer : BaseLayer
    {
        public string Url { get; set; }

        public WmsLayer(WmsLayerOptions options)
        {
            this.Options = options;
        }

        public override ILayerOptions Options { get; protected set; }
        
        public override string Type { get { return "wms"; } }

        public override string Create()
        {
            return String.Format("L.tileLayer.wms('{0}', {1}).addTo(net.cartodb.map)", this.Url, this.Options.ToJson());
        }
    }
}
