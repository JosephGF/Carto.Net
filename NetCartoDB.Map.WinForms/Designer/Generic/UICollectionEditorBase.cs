using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Security.Permissions;

namespace NetCarto.Map.WinForms.Designer
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class UICollectionComplexEditorBase<T> : UICollectionSimpleEditorBase<T> where T : class, new()
    {
        private CollectionForm collectionForm;

        public UICollectionComplexEditorBase() : base() { }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (this.collectionForm != null && this.collectionForm.Visible)
                return new UICollectionComplexEditorBase<T>().EditValue(context, provider, value);
            else
                return base.EditValue(context, provider, value);
        }

        protected override CollectionForm CreateCollectionForm()
        {
            return this.collectionForm = base.CreateCollectionForm();
        }

        protected override object CreateInstance(Type ItemType)
        {
            return base.CreateInstance(ItemType) as T;
        }
    }
}
