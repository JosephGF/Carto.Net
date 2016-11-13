using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace NetCartoDB.Map.WinForms.Design
{
    [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
    public class UICollectionSimpleEditorBase<T> : CollectionEditor where T : class, new()
    {
        protected virtual Type[] Types { get; set; } = new Type[] { typeof(T) };

        public UICollectionSimpleEditorBase() : base(typeof(T)) { }

        protected override Type[] CreateNewItemTypes()
        {
            return Types;
        }
    }
}
