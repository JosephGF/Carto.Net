using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace NetCartoDB.Map.WinForms.Designer
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public abstract class UITypeEditorBase : UITypeEditor
    {
        protected IWindowsFormsEditorService IEditorService;
        protected Control EditControl;
        protected bool m_EscapePressed;
        protected internal string Caption { get; set; } = "CartoDB";

        abstract protected Control GetEditControl(String PropertyName, Object CurrentValue);
        abstract protected Object GetEditedValue(Control EditControl, String PropertyName, Object OldValue);
        abstract protected void LoadValues(ITypeDescriptorContext context, IServiceProvider provider, Object value);
        abstract protected UITypeEditorEditStyle SetEditStyle(ITypeDescriptorContext context);

        public override sealed UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return SetEditStyle(context);
        }

        //Displays the Custom UI (a DropDown Control or a Modal Form) for value selection.
        public override Object EditValue(ITypeDescriptorContext context, IServiceProvider provider, Object value)
        {
            try
            {
                if (context != null && provider != null)
                {
                    //Uses the IWindowsFormsEditorService to display a drop-down UI in the Properties window:
                    IEditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                    if (IEditorService != null)
                    {
                        String PropName = context.PropertyDescriptor.Name;
                        EditControl = this.GetEditControl(PropName, value);        //get Edit Control from driven class

                        this.LoadValues(context, provider, value);
                        
                        if (EditControl != null)
                        {
                            //we should set this flag to False before showing the control
                            m_EscapePressed = false;
                            //show given EditControl
                            // => it will be closed if user clicks on outside area or we invoke IEditorService.CloseDropDown()

                            if (EditControl is Form)
                                m_EscapePressed = IEditorService.ShowDialog((Form)EditControl) == DialogResult.Cancel;
                            else
                                IEditorService.DropDownControl(EditControl);

                            return (m_EscapePressed) ? value : GetEditedValue(EditControl, PropName, value); //return the Old Value (if user press Escape)
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }

            return base.EditValue(context, provider, value);
        }
        
        public IWindowsFormsEditorService GetIWindowsFormsEditorService()
        {
            return IEditorService;
        }

        private void m_EditControl_PreviewKeyDown(Object sender, PreviewKeyDownEventArgs e)
        {
            EditControl.PreviewKeyDown += EditControl_PreviewKeyDown;
        }

        private void EditControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) m_EscapePressed = true;
        }

        protected virtual void DisplayError(String msg)
        {
            MessageBox.Show(msg, Caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }   
}
