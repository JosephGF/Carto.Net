using NetCarto.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCarto.Map.Common.Layers
{
    public class MapnikLayer : BaseLayer
    {
        public MapnikLayer(MapnikLayerOptions options)
        {
            this.Options = options;
        }

        public MapnikLayer() { }

        public override string Type
        {
            get { return "mapnik"; }
        }
        
        public override ILayerOptions Options { get; protected set; }

        public override string Create()
        {
            object options = new { type = this.Type, options = this.Options };

            return String.Format("cartodb.createLayer(net.cartodb.map, {0})", options.ToJson());
        }
    }
}
