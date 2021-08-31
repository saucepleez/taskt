using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace taskt.UI.Forms
{
    public partial class frmSample : ThemedForm
    {
        private string samplePath;
        public UI.Forms.frmScriptBuilder parentForm;

        public frmSample(UI.Forms.frmScriptBuilder parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
        }

        private void frmSample_Load(object sender, EventArgs e)
        {
            samplePath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Samples";

            if (!System.IO.Directory.Exists(samplePath))
            {
                return;
            }

            IEnumerable<string> files = System.IO.Directory.EnumerateFiles(samplePath, "*.xml", System.IO.SearchOption.AllDirectories);

            int baseLen = samplePath.Length + 1;    // (+1) is \\

            string oldFolder;
            oldFolder = "----";

            TreeNode parentGroup = null;
            foreach(var file in files)
            {
                string absPath = file.Substring(baseLen);
                string[] absParts = absPath.Split('\\');
                if (absParts[0] == oldFolder)
                {
                    TreeNode newNode = new TreeNode(absParts[1]);
                    parentGroup.Nodes.Add(newNode);
                }
                else
                {
                    if (oldFolder != "----")
                    {
                        tvSamples.Nodes.Add(parentGroup);
                    }
                    oldFolder = absParts[0];
                    parentGroup = new TreeNode(absParts[0]);
                    TreeNode newNode = new TreeNode(absParts[1]);
                    parentGroup.Nodes.Add(newNode);
                }
            }
            tvSamples.Nodes.Add(parentGroup);
            //tvSamples.ExpandAll();
        }

        #region tvSample events
        private void tvSamples_DoubleClick(object sender, EventArgs e)
        {
            if (tvSamples.SelectedNode.Level == 0)
            {
                return;
            }
            else
            {
                tvContextMenuStrip.Show(Cursor.Position);
            }
        }
        private void tvSamples_MouseClick(object sender, MouseEventArgs e)
        {
            if (tvSamples.SelectedNode.Level == 0)
            {
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                tvContextMenuStrip.Show(Cursor.Position);
            }
        }

        #endregion

        #region footer buttons
        private void btnOpen_Click(object sender, EventArgs e)
        {
            openSampleScriptProcess();
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            importSampleScriptProcess();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Open/Import sample
        private string getSelectedScriptPath()
        {
            if (tvSamples.SelectedNode.Level != 1)
            {
                return "";
            }
            else
            {
                return samplePath + "\\" + tvSamples.SelectedNode.Parent.Text + "\\" + tvSamples.SelectedNode.Text;
            }
        }
        private void openSampleScriptProcess()
        {
            string targetFile = getSelectedScriptPath();
            string fileName = System.IO.Path.GetFileName(targetFile);
            if (targetFile != "")
            {
                parentForm.OpenSampleScript(targetFile);
                using (var fm = new UI.Forms.Supplemental.frmDialog(fileName + " is opened.\nClose this window ?", "Open script", Supplemental.frmDialog.DialogType.YesNo, 0))
                {
                    if (fm.ShowDialog() == DialogResult.OK)
                    {
                        this.Close();
                    }
                }
            }
        }
        private void importSampleScriptProcess()
        {
            string targetFile = getSelectedScriptPath();
            string fileName = System.IO.Path.GetFileName(targetFile);
            if (targetFile != "")
            {
                parentForm.ImportSampleScript(targetFile);
                using (var fm = new UI.Forms.Supplemental.frmDialog(fileName + " is imported.\nClose this window ?", "Open script", Supplemental.frmDialog.DialogType.YesNo, 0))
                {
                    if (fm.ShowDialog() == DialogResult.OK)
                    {
                        this.Close();
                    }
                }
            }
        }
        #endregion

        #region tvContextMenuStrip events
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openSampleScriptProcess();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            importSampleScriptProcess();
        }
        #endregion
    }
}
