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

        private string firstGroup;
        private string firstCommand;

        private string selectedFullCommand;

        public frmCommandList(TreeNode[] commands, ImageList commandsImage, string selectedCommand)
        {
            InitializeComponent();

            treeAllCommands = (TreeNode[])commands.Clone();
            treeAllCommandsImage = commandsImage;

            var spt = selectedCommand.Split('-');
            if (spt.Length == 2)
            {
                firstGroup = spt[0].Trim();
                firstCommand = spt[1].Trim();
            }
            else
            {
                firstGroup = "";
                firstCommand = "";
            }
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

            if ((firstGroup != "") && (firstCommand != ""))
            {
                taskt.Core.CommandsTreeControls.FocusCommand(firstGroup, firstCommand, tvCommands);
            }
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

        private void tvCommands_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (tvCommands.SelectedNode != null)
            {
                if (tvCommands.SelectedNode.Nodes.Count == 0)
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
        }

        public string FullCommandName
        {
            get
            {
                return taskt.Core.CommandsTreeControls.GetSelectedFullCommandName(tvCommands);
            }
        }
    }
}
