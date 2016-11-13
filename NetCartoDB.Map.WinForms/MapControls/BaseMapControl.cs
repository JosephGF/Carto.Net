using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetCartoDB.Map.WinForms;
using System.Windows.Forms;

namespace NetCartoDB.Map.WinForms.Controls
{
    public class BaseMapControl : Control
    {
        public Map Map { get; protected set; }

        public BaseMapControl(): base() { }

        public void AddToMap(Map map)
        {
            this.Map = map;
        }
    }
}
