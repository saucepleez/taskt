//Copyright (c) 2019 Jason Bayldon
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using System;
using System.Drawing;
using System.Windows.Forms;

namespace taskt.UI.Forms.General
{
    public partial class frmDialog : DialogLikeThemedForm
    {
        #region fields
        public enum DialogType
        {
            YesNo,
            OkCancel,
            OkOnly
        }

        /// <summary>
        /// close sec
        /// </summary>
        private int closeTicks;

        /// <summary>
        /// current sec passed
        /// </summary>
        private int ticksPassed;

        #endregion
        public frmDialog(string message, string title, DialogType dialogType, int closeAfterSeconds, bool showTop = true, string fontName = "", float fontSize = 0F)
        {
            InitializeComponent();

            txtMessage.Text = message;
            this.Text = title;
            switch (dialogType)
            {
                case DialogType.YesNo:
                    uiBtnOk.DisplayText = "Yes";
                    uiBtnCancel.DisplayText = "No";
                    break;

                case DialogType.OkOnly:
                    uiBtnCancel.Hide();
                    break;

                default:
                    break;
            }

            if (closeAfterSeconds > 0)
            {
                closeTicks = closeAfterSeconds;
                CalculateCloseTime();
                lblAutoClose.Show();
                autoCloseTimer.Interval = 1000;
                autoCloseTimer.Enabled = true;
            }
            pnlControlContainer.BackColor = Color.SteelBlue;

            //this.txtMessage.SelectionStart = txtMessage.Text.Length;
            //txtMessage.SelectionStart = 0;
            //txtMessage.Font = new Font("MS Gothic", 14.25F);

            txtMessage.SelectionStart = (showTop) ? 0 : txtMessage.Text.Length;

            if (!string.IsNullOrEmpty(fontName))
            {
                if (fontSize <= 0F)
                {
                    fontSize = txtMessage.Font.Size;
                }
                txtMessage.Font = new Font(fontName, fontSize);
            }
        }

        private void CalculateCloseTime()
        {
            lblAutoClose.Text = $"closing in {(closeTicks - ticksPassed)} sec(s)";
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void autoCloseTimer_Tick(object sender, EventArgs e)
        {
            if (closeTicks == ticksPassed)
            {
                this.Close();
            }
            else
            {
                ticksPassed++;
                CalculateCloseTime();
            }
        }

        private void frmDialog_Load(object sender, EventArgs e)
        {
            this.Focus();
        }

        //private void frmDialog_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Escape)
        //    {
        //        this.Close();
        //    }
        //}
        //private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Escape)
        //    {
        //        this.Close();
        //    }
        //}
    }
}