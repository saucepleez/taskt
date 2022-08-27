using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmUpdate : ThemedForm
    {
        public frmUpdate(Core.UpdateManifest manifest)
        {
            InitializeComponent();
            lblLocal.Text = "your version: " + manifest.LocalVersionProper.ToString();
            lblRemote.Text = "latest version: " + manifest.RemoteVersionProper.ToString();
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
