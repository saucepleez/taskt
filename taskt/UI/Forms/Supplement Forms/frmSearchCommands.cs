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
    public partial class frmSearchCommands : ThemedForm
    {
        public taskt.UI.Forms.frmScriptBuilder parentForm = null;

        public frmSearchCommands()
        {
            InitializeComponent();
        }


        #region form load, close, activate
        private void frmSearchCommands_Load(object sender, EventArgs e)
        {

        }
        private void frmSearchCommands_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            parentForm.currentScriptEditMode = frmScriptBuilder.EditMode.Normal;
            this.Hide();
        }
        private void frmSearchCommands_Activated(object sender, EventArgs e)
        {
            int opa = barOpacity.Value;
            this.Opacity = (double)(opa * 0.01);
        }

        private void frmSearchCommands_Deactivate(object sender, EventArgs e)
        {
            int opa = barOpacity.Value;
            double newOpa = (opa * 0.008);  // * 0.01 * 0.8
            if (newOpa < 0.1)
            {
                newOpa = 0.1;
            }
            this.Opacity = newOpa;
        }
        #endregion


        #region footer controls
        private void barOpacity_Scroll(object sender, EventArgs e)
        {
            int opa = barOpacity.Value;
            this.Opacity = (double)(opa * 0.01);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void btnClearAndClose_Click(object sender, EventArgs e)
        {
            parentForm.currentScriptEditMode = frmScriptBuilder.EditMode.Normal;
            this.Hide();
        }
        #endregion

        private void btnSearchSearch_Click(object sender, EventArgs e)
        {
            string kwd = txtSearchKeyword.Text;
            if (kwd.Length == 0)
            {
                using (var frm = new taskt.UI.Forms.Supplemental.frmDialog("Keyword is empty.", "Search Commands", Supplemental.frmDialog.DialogType.OkOnly, 0))
                {
                    frm.ShowDialog();
                    return;
                }
            }

            if (!(chkSearchTargetIsParameter.Checked || chkSearchTargetIsName.Checked || chkSearchTargetIsComment.Checked || chkSearchTargetIsDisplayText.Checked))
            {
                using (var frm = new taskt.UI.Forms.Supplemental.frmDialog("No target.", "Search Commands", Supplemental.frmDialog.DialogType.OkOnly, 0))
                {
                    frm.ShowDialog();
                    return;
                }
            }

            this.Enabled = false;
            int matchNum = parentForm.AdvancedSearchItemInCommands(kwd, chkSearchCaseSensitive.Checked, chkSearchTargetIsParameter.Checked, chkSearchTargetIsName.Checked, chkSearchTargetIsComment.Checked, chkSearchTargetIsDisplayText.Checked);
            this.Enabled = true;
        }

        private void btnSearchNext_Click(object sender, EventArgs e)
        {
            parentForm.MoveMostNearMatchedLine();
        }
    }
}
