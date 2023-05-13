using System;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmUpdate : ThemedForm
    {
        private Core.Update.UpdateManifest manifest = null;

        public frmUpdate(Core.Update.UpdateManifest manifest)
        {
            InitializeComponent();
            this.manifest = manifest;
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

        private void btnShowUpdateDetails_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Core.MyURLs.GitReleaseURL + "/v" + manifest.RemoteVersionProper.ToString());
        }
    }
}
