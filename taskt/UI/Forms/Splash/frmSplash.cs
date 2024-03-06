using System;
using System.Windows.Forms;
using System.IO;
using taskt.Core.Script;

namespace taskt.UI.Forms.Splash
{
    public partial class frmSplash : Form
    {
        public frmSplash()
        {
            InitializeComponent();
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {
            // check exist or create AutoSave, RunWithoutSaving folders
            var autoSavePath = Script.GetAutoSaveFolderPath();
            if (!Directory.Exists(autoSavePath))
            {
                Directory.CreateDirectory(autoSavePath);
            }

            var runPath = Script.GetRunWithoutSavingFolderPath();
            if (!Directory.Exists(runPath))
            {
                Directory.CreateDirectory(runPath);
            }

            var befPath = Script.GetBeforeConvertedFolderPath();
            if (!Directory.Exists(befPath))
            {
                Directory.CreateDirectory(befPath);
            }
        }
    }
}
