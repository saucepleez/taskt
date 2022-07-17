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
        private TreeNode[] treeCommands;
        public frmCommandList(TreeNode[] commands)
        {
            InitializeComponent();

            treeCommands = (TreeNode[])commands.Clone();
        }
        private void frmCommandList_Load(object sender, EventArgs e)
        {
            tvCommands.Nodes.Clear();
            //tvCommands.Nodes.AddRange((TreeNode[])treeCommands.Clone());
            foreach(TreeNode command in treeCommands)
            {
                tvCommands.Nodes.Add((TreeNode)command.Clone());
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


    }
}
