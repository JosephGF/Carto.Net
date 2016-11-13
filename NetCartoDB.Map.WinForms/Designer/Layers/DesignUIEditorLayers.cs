using NetCarto.Map.Common.Layers;
using NetCarto.Map.WinForms.Design.Layers.Objects;
using NetCarto.Map.WinForms.Designer.Layers.Objects;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Security.Permissions;

namespace NetCarto.Map.WinForms.Designer.Layers
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public partial class DesignUIEditorLayers : CollectionEditor // UITypeEditorBase
    {
        public new Type[] NewItemTypes { get; } = new Type[] { typeof(TileLayer_Designer), typeof(CartoLayerDesigner) };

        public DesignUIEditorLayers() : base(typeof(BaseLayer)) { }

        public DesignUIEditorLayers(Type type) : base(type) { }

        private CollectionForm collectionForm;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (this.collectionForm != null && this.collectionForm.Visible)
            {
                DesignUIEditorLayers editor = new DesignUIEditorLayers(this.CollectionType);
                return editor.EditValue(context, provider, value);
            }

            else
                return base.EditValue(context, provider, value);

        }

        protected override CollectionForm CreateCollectionForm()
        {
            this.collectionForm = base.CreateCollectionForm();
            return this.collectionForm;
        }

        protected override object CreateInstance(Type ItemType)
        {
            return (TileLayer)base.CreateInstance(ItemType);
        }

        new public UITypeEditorEditStyle GetEditStyle()
        {
            return UITypeEditorEditStyle.Modal;
        }

        protected override Type[] CreateNewItemTypes()
        {
            return NewItemTypes;
        }
    }
}
