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
            string[] keysList = new string[]
            {
                "",
                "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
                "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
                "U", "V", "W", "X", "Y", "Z",
                "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10",
                "F11", "F12", "F13", "F14", "F15", "F16",

                "Enter", "BackSpace", "Delete", "ESC", "Tab",
                "↑", "→", "↓", "←",
                "Home", "End", "Page Up", "Page Down",
                "+ (Plus)", "- (Minus)", "* (Mul)", "/ (Divide)",
                "Insert",
                "Break",
                "Caps Lock",
                "Help",
                "Num Lock",
                "Scroll Lock",
            };
            cmbKey.Items.AddRange(keysList);
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

                txtTimes.Text = "1";
                txtTimes.Enabled = false;
            }
            else
            {
                chkShift.Enabled = true;
                chkCtrl.Enabled = true;
                chkAlt.Enabled = true;

                txtTimes.Enabled = true;
            }

            // show message
            lblWinKeyMessage.Visible = chkWin.Checked;

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
            if (chkWin.Checked)
            {
                return;
            }

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

            bool needBlacket;
            string key = convertKeyCode(out needBlacket);

            // not support
            if (chkWin.Checked && needBlacket)
            {
                txtResult.Text = "Error not supported keys.";
                return;
            }

            int times;
            if (int.TryParse(txtTimes.Text, out times))
            {
                if (times > 1)
                {
                    result += "{" + key + " [" + times + "]}";
                }
                else
                {
                    if (chkWin.Checked)
                    {
                        if (String.IsNullOrEmpty(key))
                        {
                            result = "{WIN_KEY}";
                        }
                        else
                        {
                            result = "{WIN_KEY+" + key.ToUpper() + "}";
                        }
                    }
                    else if (isHotkey || needBlacket)
                    {
                        result += "{" + key + "}";
                    }
                    else
                    {
                        result = key;
                    }
                }
                txtResult.Text = result;
            }
            else
            {
                if (chkWin.Checked)
                {
                    if (String.IsNullOrEmpty(key))
                    {
                        result = "{WIN_KEY}";
                    }
                    else
                    {
                        result = "{WIN_KEY+" + key.ToUpper() + "}";
                    }
                }
                else if (isHotkey || needBlacket)
                {
                    result += "{" + key + "}";
                }
                else
                {
                    result = key;
                }
                txtResult.Text = result;
            }
        }

        private string convertKeyCode(out bool needBracket)
        {
            needBracket = false;

            if (cmbKey.SelectedIndex > 36)
            {
                needBracket = true;
            }

            switch (cmbKey.Text)
            {
                case "Caps Lock":
                    return "CAPSLOCK";

                case "↑":
                    return "UP";

                case "→":
                    return "RIGHT";

                case "↓":
                    return "DOWN";

                case "←":
                    return "LEFT";

                case "Num Lock":
                    return "NUMLOCK";

                case "Page Down":
                    return "PGDN";

                case "Page Up":
                    return "PGUP";

                case "Scroll Lock":
                    return "SCROLLLOCK";

                case "+ (Plus)":
                    return "ADD";

                case "- (Minus)":
                    return "SUBTRACT";

                case "* (Mul)":
                    return "MULTIPLY";

                case "/ (Divide)":
                    return "DIVIDE";

                default:
                    return (needBracket ? cmbKey.Text.ToUpper() : cmbKey.Text.ToLower());
            }
        }

        #region properties
        public string Result
        {
            get
            {
                return txtResult.Text;
            }
        }
        #endregion

    }
}
