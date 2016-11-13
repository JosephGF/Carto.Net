using NetCartoDB.Map.Common.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCartoDB.Map.WinForms.Designer.Layers
{
    public class UICollectionLayersEditor : UICollectionSimpleEditorBase<BaseLayer>
    {
        public UICollectionLayersEditor() : base()
        {
            //this.Types = new Type[] { typeof(TileLayer), typeof(MapnikLayer) };
            this.Types = new Type[] { typeof(DemoLayer), typeof(DemoLayerSecond) };
        }
    }

    public class DemoLayer : LayersCollection { }

    public class DemoLayerSecond : LayersCollection
    {
        public string name { get; set; }
    }
}
