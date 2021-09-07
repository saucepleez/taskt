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

        private SearchReplaceMode _currentMode = SearchReplaceMode.Search;
        public SearchReplaceMode CurrentMode
        {
            set
            {
                switch (value)
                {
                    case SearchReplaceMode.Search:
                        searchTab.SelectedIndex = 0;
                        txtSearchKeyword.Focus();
                        break;
                    case SearchReplaceMode.Replace:
                        searchTab.SelectedIndex = 1;
                        txtReplaceSearch.Focus();
                        break;
                }
            }
            get
            {
                return this._currentMode;
            }
        }

        public enum SearchReplaceMode
        {
            Search,
            Replace
        }

        public frmSearchCommands()
        {
            InitializeComponent();
        }


        #region form load, close, activate
        private void frmSearchCommands_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
        }
        private void frmSearchCommands_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            parentForm.currentScriptEditorMode = frmScriptBuilder.CommandEditorState.Normal;
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
        private void frmSearchCommands_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                parentForm.currentScriptEditorMode = frmScriptBuilder.CommandEditorState.Normal;
                this.Hide();
            }
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
            parentForm.currentScriptEditorMode = frmScriptBuilder.CommandEditorState.Normal;
            this.Hide();
        }
        #endregion

        #region search
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

            if (!(chkSearchTargetIsParameter.Checked || chkSearchTargetIsName.Checked || chkSearchTargetIsComment.Checked || chkSearchTargetIsDisplayText.Checked || chkSearchTargetIsInstance.Checked))
            {
                using (var frm = new taskt.UI.Forms.Supplemental.frmDialog("No target.", "Search Commands", Supplemental.frmDialog.DialogType.OkOnly, 0))
                {
                    frm.ShowDialog();
                    return;
                }
            }

            this.Enabled = false;
            int matchNum = parentForm.AdvancedSearchItemInCommands(kwd, chkSearchCaseSensitive.Checked, chkSearchTargetIsParameter.Checked, chkSearchTargetIsName.Checked, chkSearchTargetIsComment.Checked, chkSearchTargetIsDisplayText.Checked, chkSearchTargetIsInstance.Checked, cmbSearchInstance.Text);
            this.Enabled = true;

            parentForm.Activate();
        }

        private void btnSearchNext_Click(object sender, EventArgs e)
        {
            parentForm.MoveMostNearMatchedLine(chkSearchBackToTop.Checked);
        }

        private void txtSearchKeyword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                btnSearchSearch_Click(null, null);
            }
        }

        private void chkSearchTargetIsInstance_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSearchTargetIsInstance.Checked)
            {
                cmbSearchInstance.Enabled = true;
                cmbSearchInstance.Focus();
                if (cmbSearchInstance.Text == "")
                {
                    cmbSearchInstance.Text = "Excel";
                }
            }
            else
            {
                cmbSearchInstance.Enabled = false;
            }
        }
        #endregion

        #region replace
        private void radioTargetIsInstance_CheckedChanged(object sender, EventArgs e)
        {
            if (radioTargetIsInstance.Checked)
            {
                cmbReplaceInstance.Enabled = true;
                cmbReplaceInstance.Focus();
                if (cmbReplaceInstance.Text == "")
                {
                    cmbReplaceInstance.Text = "Excel";
                }
            }
            else
            {
                cmbReplaceInstance.Enabled = false;
            }
        }
        private void btnReplaceSearch_Click(object sender, EventArgs e)
        {
            string kwd = txtReplaceSearch.Text;
            if (kwd.Length == 0)
            {
                using (var frm = new taskt.UI.Forms.Supplemental.frmDialog("Keyword is empty.", "Search Commands", Supplemental.frmDialog.DialogType.OkOnly, 0))
                {
                    frm.ShowDialog();
                    return;
                }
            }
            this.Enabled = false;
            int matchNum = parentForm.ReplaceSearchInItemCommands(kwd, chkReplaceCaseSensitive.Checked, cmbReplaceInstance.Text, radioTargetIsAll.Checked, radioTargetIsInstance.Checked, radioTargetIsComment.Checked);
            this.Enabled = true;

            parentForm.Activate();
        }
        private void btnReplaceReplace_Click(object sender, EventArgs e)
        {
            string kwd = txtReplaceSearch.Text;
            if (kwd.Length == 0)
            {
                using (var frm = new taskt.UI.Forms.Supplemental.frmDialog("Keyword is empty.", "Search Commands", Supplemental.frmDialog.DialogType.OkOnly, 0))
                {
                    frm.ShowDialog();
                    return;
                }
            }
            this.Enabled = false;
            bool res = parentForm.ReplaceInItemCommands(kwd, txtReplaceReplace.Text, chkReplaceCaseSensitive.Checked, chkReplaceBackToTop.Checked, cmbReplaceInstance.Text, radioTargetIsAll.Checked, radioTargetIsInstance.Checked, radioTargetIsComment.Checked);
            this.Enabled = true;

            parentForm.Activate();
        }
        private void btnReplaceReplaceAll_Click(object sender, EventArgs e)
        {
            string kwd = txtReplaceSearch.Text;
            if (kwd.Length == 0)
            {
                using (var frm = new taskt.UI.Forms.Supplemental.frmDialog("Keyword is empty.", "Search Commands", Supplemental.frmDialog.DialogType.OkOnly, 0))
                {
                    frm.ShowDialog();
                    return;
                }
            }
            this.Enabled = false;
            parentForm.ReplaceAllInItemCommands(kwd, txtReplaceReplace.Text, chkReplaceCaseSensitive.Checked, cmbReplaceInstance.Text, radioTargetIsAll.Checked, radioTargetIsInstance.Checked, radioTargetIsComment.Checked);
            this.Enabled = true;

            parentForm.Activate();
        }




        #endregion

        
    }
}
