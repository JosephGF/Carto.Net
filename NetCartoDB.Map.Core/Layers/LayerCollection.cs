using NetCarto.Core;
using System.Collections.Generic;
using System;

namespace NetCarto.Map.Common.Layers
{
    public class LayersCollection : Collection<BaseLayer>
    {
        public LayersCollection() { }

        public LayersCollection AddRange(ILayer[] data)
        {
            for (int i = 0; i < data.Length; i++)
                this.Add(data[i] as BaseLayer);

            return this;
        }
    }
}
