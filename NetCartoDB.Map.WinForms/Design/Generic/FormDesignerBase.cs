using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;

namespace NetCartoDB.Map.WinForms.Design
{
    public partial class FormDesignerBase : Form
    {
        private IList _items;

        public IList Items {
            get {
                return _items;
            }
            set {
                _items = value;
                this.InitialItems = value;
            }
        }
        public Type[] AllowedTypes { get; protected set; }
        protected IList InitialItems { get; set; }
        protected UITypeEditor Editor { get; set; }

        public delegate void InstanceEventHandler(object sender, object instance);
        public event InstanceEventHandler InstanceCreated;
        public event InstanceEventHandler DestroyingInstance;
        public event InstanceEventHandler ItemRemoved;
        public event InstanceEventHandler ItemAdded;

        public FormDesignerBase()
        {
            InitializeComponent();
            this.Editor = new UITypeEditor();
        }

        public FormDesignerBase(UITypeEditor editor)
        {
            InitializeComponent();
            this.Editor = editor;
        }

        protected virtual void OnDestroyingInstance(object instance)
        {
            if (DestroyingInstance != null)
                DestroyingInstance(this, instance);
        }

        protected virtual void OnInstanceCreated(object instance)
        {
            if (InstanceCreated != null)
                InstanceCreated(this, instance);
        }

        protected virtual void OnItemRemoved(object item)
        {
            if (ItemRemoved != null)
                ItemRemoved(this, item);
        }

        protected virtual void OnItemAdded(object Item)
        {
            if (ItemAdded != null)
                ItemAdded(this, Item);
        }

        protected virtual object CreateInstance(Type type)
        {
            var instance = Activator.CreateInstance(type);
            OnInstanceCreated(instance);
            return instance;
        }

        protected virtual void DestroyInstance(object instance)
        {
            OnDestroyingInstance(instance);
            if (instance is IDisposable) { ((IDisposable)instance).Dispose(); }
            instance = null;
        }

        protected virtual Type GetItemType(IList coll)
        {
            PropertyInfo pi = coll.GetType().GetProperty("Item", new Type[] { typeof(int) });
            return pi.PropertyType;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        protected virtual void Add(object item)
        {
            this.Items.Add(item);
            OnItemAdded(item);
        }
    }
}
