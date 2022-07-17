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
    public partial class frmCommandList : ThemedForm
    {
        private TreeNode[] treeAllCommands;
        private ImageList treeAllCommandsImage;

        public frmCommandList(TreeNode[] commands, ImageList commandsImage)
        {
            InitializeComponent();

            treeAllCommands = (TreeNode[])commands.Clone();
            treeAllCommandsImage = commandsImage;
        }
        private void frmCommandList_Load(object sender, EventArgs e)
        {
            tvCommands.SuspendLayout();
            tvCommands.BeginUpdate();

            tvCommands.Nodes.Clear();
            foreach (TreeNode command in treeAllCommands)
            {
                tvCommands.Nodes.Add((TreeNode)command.Clone());
            }

            //taskt.Core.CommandsTreeControls.ShowCommandsTree(tvCommands, treeAllCommands);

            tvCommands.ImageList = treeAllCommandsImage;

            tvCommands.EndUpdate();
            tvCommands.ResumeLayout();
        }

        #region footer buttons
        private void uiBtnAdd_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        #endregion


    }
}
