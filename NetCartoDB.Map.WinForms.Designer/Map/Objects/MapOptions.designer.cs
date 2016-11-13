using NetCartoDB.Map.Common;
using NetCartoDB.Map.Common.Layers;
using NetCartoDB.Map.WinForms.Designer;
using System.ComponentModel;

namespace NetCartoDB.Map.WinForms.Design.Map.Objects
{
    //[TypeConverter(typeof(UIExpandibleEditorBase<Options>))]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class MapOptionsDesigner : MapOptions /*, ITConvertSerializer*/
    {
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
            return "CartoDB Map Options";
        }
    }
}
