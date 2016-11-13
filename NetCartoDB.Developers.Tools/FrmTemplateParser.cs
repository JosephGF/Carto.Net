using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetCarto.Developers.Tools
{
    public partial class FrmTemplateParser : Form
    {
        public FrmTemplateParser()
        {
            InitializeComponent();
            this.lbTemplates.Items.AddRange(System.IO.Directory.EnumerateFiles(Application.StartupPath + "\\Templates", "*.cdb.temp.txt", System.IO.SearchOption.TopDirectoryOnly).ToArray());
        }

        private void btnLoadCsv_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {

            }
        }
    }
}
