using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetCartoDB.Map.WinForms.Controls
{
    public partial class MapZoom : BaseMapControl
    {
        public MapZoom()
        {
            InitializeComponent();
        }

        private void btnAddZoom_Click(object sender, EventArgs e)
        {
            Map.Map_SetZoom(-1);
        }
    }
}
