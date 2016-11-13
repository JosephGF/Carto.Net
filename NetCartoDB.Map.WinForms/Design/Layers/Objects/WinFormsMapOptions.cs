using NetCartoDB.Core.Extensions;
using NetCartoDB.Map.Common;
using NetCartoDB.Map.Common.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCartoDB.Map.WinForms.Design.Layers.Objects
{

    //[TypeConverter(typeof(UIExpandibleEditorBase<Options>))]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WinFormsMapOptions : MapOptions/*, ITConvertSerializer*/
    {
        [Browsable(true), DisplayName("Map Layers")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //[Editor(typeof(DesignUIEditorLayers), typeof(System.Drawing.Design.UITypeEditor))]
        [Editor(typeof(Generic.UICustomFormEditor), typeof(System.Drawing.Design.UITypeEditor))]
        new public LayersCollection Layers
        {
            get {  return base.Layers; }
            set { base.Layers = value; }
        }
        /*= new ILayer[] {
        //    new TileLayer(TileLayer.CartoDBLayers.CartoDB_PositronLiteRainbow, new TileLayerOptions() { Attribution = "Cartodb" }),
        //};*/

        [Browsable(true), Description("Set map lat/lng center.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        new public double[] Center { get { return base.Center; } }

        /*public string Serialize(object data)
        {
            return data.ToJson();
        }

        public object Deserialize(string data)
        {
            return data.FromJson<Options>();
        }

        public object Initialize(object current)
        {
            return current;
        }*/

        public override string ToString()
        {
            return "Map Options";
        }
    }
}
