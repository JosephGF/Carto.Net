using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCartoDB.Map.Common.Layers
{
    public class MapnikLayer : ILayer
    {
        public MapnikLayer(LayerOptions options)
        {
            this.Options = options;
        }

        public MapnikLayer()
        {
            this.Type = "mapnik";
        }

        public string Type
        {
            get; protected set;
        }

        public string Name
        {
            get; set;
        }

        public LayerOptions Options
        {
            get; protected set;
        }

        public void ILayer(LayerOptions Options)
        {
            throw new NotImplementedException();
        }
    }
}
