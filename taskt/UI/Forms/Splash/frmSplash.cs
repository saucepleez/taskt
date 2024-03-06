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
            // check exist or create AutoSave, RunWithoutSaving, BeforeConverted folders
            //var autoSavePath = Script.GetAutoSaveFolderPath();
            //if (!Directory.Exists(autoSavePath))
            //{
            //    Directory.CreateDirectory(autoSavePath);
            //}
            CreateFolderProcess(Script.GetAutoSaveFolderPath);

            //var runPath = Script.GetRunWithoutSavingFolderPath();
            //if (!Directory.Exists(runPath))
            //{
            //    Directory.CreateDirectory(runPath);
            //}
            CreateFolderProcess(Script.GetRunWithoutSavingFolderPath);

            //var befPath = Script.GetBeforeConvertedFolderPath();
            //if (!Directory.Exists(befPath))
            //{
            //    Directory.CreateDirectory(befPath);
            //}
            CreateFolderProcess(Script.GetBeforeConvertedFolderPath);
        }

        /// <summary>
        /// create folder process when not exists
        /// </summary>
        /// <param name="folderFunc"></param>
        private static void CreateFolderProcess(Func<string> folderFunc)
        {
            var path = folderFunc();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
