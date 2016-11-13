using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace NetCartoDB.Map.WinForms.Designer
{
    public class UICustomFormEditor : UITypeEditor
    {
        public delegate void CollectionChangedEventHandler(object sender, object instance, object value);
        public event CollectionChangedEventHandler CollectionChanged;

        private ITypeDescriptorContext _context;

        private IWindowsFormsEditorService edSvc = null;

        public UICustomFormEditor() { }
        
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                object originalValue = value;
                _context = context;
                edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (edSvc != null)
                {
                    FormDesignerBase form = CreateForm();

                    form.ItemAdded += new FormDesignerBase.InstanceEventHandler(ItemAdded);
                    form.ItemRemoved += new FormDesignerBase.InstanceEventHandler(ItemRemoved);
                    form.Items = (System.Collections.IList)value;
                    
                    context.OnComponentChanging();
                    if (edSvc.ShowDialog(form) == DialogResult.OK)
                    {
                        OnCollectionChanged(context.Instance, value);
                        context.OnComponentChanged();
                    }
                }
            }

            return value;
        }

        protected virtual FormDesignerBase CreateForm()
        {
            return new Layers.FormDesignerLayers(_context.Instance);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return base.GetEditStyle(context);
        }

        private void ItemAdded(object sender, object item)
        {

            if (_context != null && _context.Container != null)
            {
                IComponent icomp = item as IComponent;
                if (icomp != null)
                {
                    _context.Container.Add(icomp);
                }
            }

        }

        private void ItemRemoved(object sender, object item)
        {
            if (_context != null && _context.Container != null)
            {
                IComponent icomp = item as IComponent;
                if (icomp != null)
                {
                    _context.Container.Remove(icomp);
                }
            }

        }
        
        protected virtual void OnCollectionChanged(object instance, object value)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, instance, value);
            }
        }

    }
}
