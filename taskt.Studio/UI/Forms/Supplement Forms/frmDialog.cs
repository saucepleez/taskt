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
using taskt.Core.Enums;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmDialog : Form
    {
        public int CloseTicks { get; set; }
        public int TicksPassed { get; set; }

        public frmDialog(string message, string title, DialogType dialogType, int closeAfterSeconds)
        {
            InitializeComponent();

            txtMessage.Text = message;
            Text = title;
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
                CloseTicks = closeAfterSeconds;
                CalculateCloseTime();
                lblAutoClose.Show();
                autoCloseTimer.Interval = 1000;
                autoCloseTimer.Enabled = true;
            }
            pnlControlContainer.BackColor = Color.SteelBlue;
            txtMessage.SelectionStart = txtMessage.Text.Length;
        }

        private void CalculateCloseTime()
        {
            lblAutoClose.Text = "closing in " + (CloseTicks - TicksPassed) + " sec(s)";
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void autoCloseTimer_Tick(object sender, EventArgs e)
        {
            if (CloseTicks == TicksPassed)
            {
                Close();
            }
            else
            {
                TicksPassed++;
                CalculateCloseTime();
            }
        }

        private void frmDialog_Load(object sender, EventArgs e)
        {
            Focus();
        }

        private void frmDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }      

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}