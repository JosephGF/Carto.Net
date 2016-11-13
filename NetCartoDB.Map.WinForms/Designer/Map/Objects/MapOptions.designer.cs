using NetCarto.Map.Common;
using NetCarto.Map.Common.Layers;
using NetCarto.Map.WinForms.Designer;
using NetCarto.Core.Extensions;
using System.ComponentModel;
using NetCarto.Map.WinForms.Designer.Layers;

namespace NetCarto.Map.WinForms.Designer.Map.Objects
{
    //[TypeConverter(typeof(UIExpandibleEditorBase<Options>))]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class MapOptionsDesigner : MapOptions /*, ITConvertSerializer*/
    {
        public static MapOptionsDesigner From(MapOptions o)
        {
            return o.ConvertTo<MapOptionsDesigner>();
        }

        [Browsable(true), DisplayName("Map Layers")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //[Editor(typeof(DesignUIEditorLayers), typeof(System.Drawing.Design.UITypeEditor))] //Default Editor
        [Editor(typeof(UICustomFormEditor), typeof(System.Drawing.Design.UITypeEditor))] //Custom Editor
        new public LayersCollection Layers
        {
            get {  return base.Layers; }
            set { base.Layers = value; }
        }

        [Browsable(true), Description("Set map lat/lng center.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        new public double[] Center { get { return base.Center; } }
        
        public override string ToString()
        {
            return "Carto Map Options";
        }

    }
}
