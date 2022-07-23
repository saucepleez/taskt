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
    public partial class frmKeysBuilder : ThemedForm
    {
        public frmKeysBuilder()
        {
            InitializeComponent();

            var keys = Enum.GetValues(typeof(Keys));
            string[] keysList = new string[keys.Length];
            int i = 0;
            foreach (var key in keys)
            {
                keysList[i++] = key.ToString();
            }

            cmbKey.Items.AddRange(keysList);
        }

        #region shift, ctrl, alt, win
        private void chkShift_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWin.Checked)
            {
                return;
            }

            ShowSendKey();
        }

        private void chkCtrl_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWin.Checked)
            {
                return;
            }

            ShowSendKey();
        }

        private void chkAlt_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWin.Checked)
            {
                return;
            }

            ShowSendKey();
        }

        private void chkWin_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWin.Checked)
            {
                chkShift.Enabled = false;
                chkCtrl.Enabled = false;
                chkAlt.Enabled = false;

                chkShift.Checked = false;
                chkCtrl.Checked = false;
                chkAlt.Checked = false;
            }
            else
            {
                chkShift.Enabled = true;
                chkCtrl.Enabled = true;
                chkAlt.Enabled = true;
            }

            ShowSendKey();
        }
        #endregion

        #region key, times
        private void cmbKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSendKey();
        }

        private void txtTimes_TextChanged(object sender, EventArgs e)
        {
            int times;
            if (int.TryParse(txtTimes.Text, out times))
            {
                if (times >= 0)
                {
                    ShowSendKey();
                }
            }
        }
        #endregion

        private void ShowSendKey()
        {
            string result = "";

            bool isHotkey = false;
            if (chkShift.Checked)
            {
                result += "+";
                isHotkey = true;
            }
            if (chkCtrl.Checked)
            {
                result += "^";
                isHotkey = true;
            }
            if (chkAlt.Checked)
            {
                result += "%";
                isHotkey = true;
            }

            int times;
            if (int.TryParse(txtTimes.Text, out times))
            {
                if (times > 1)
                {
                    result += "{" + cmbKey.Text + " [" + times + "]}";
                }
                else
                {
                    if (chkWin.Checked)
                    {
                        result = "{WINKEY_" + cmbKey.Text + "}";
                    }
                    else if (isHotkey)
                    {
                        result += "{" + cmbKey.Text + "}";
                    }
                    else
                    {
                        result = cmbKey.Text;
                    }
                }
                txtResult.Text = result;
            }
            else
            {
                if (chkWin.Checked)
                {
                    result = "{WINKEY_" + cmbKey.Text + "}";
                }
                else if (isHotkey)
                {
                    result += "{" + cmbKey.Text + "}";
                }
                else
                {
                    result = cmbKey.Text;
                }
                txtResult.Text = result;
            }
        }

    }
}
