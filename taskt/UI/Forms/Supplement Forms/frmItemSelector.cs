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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplemental
{
    public partial class frmItemSelector : ThemedForm
    {
        public object selectedItem { get; private set; }

        private string[] bufferdItems;

        #region form events
        public frmItemSelector(List<string> listItems)
        {
            InitializeComponent();
            this.bufferdItems = listItems.ToArray();
        }
        public frmItemSelector(List<string> listItems, string title, string headerText)
        {
            InitializeComponent();
            this.bufferdItems = listItems.ToArray();
            this.Text = title;
            this.lblHeader.Text = headerText;
        }
        private void frmVariableSelector_Load(object sender, EventArgs e)
        {
            lstVariables.BeginUpdate();
            lstVariables.Items.AddRange(bufferdItems);
            lstVariables.EndUpdate();
        }
        #endregion

        #region footer buttons event
        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            if (lstVariables.SelectedItem == null)
            {
                MessageBox.Show("There are no item(s) selected! Select an item and Ok or select Cancel");
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region lstVariables events
        private void lstVariables_DoubleClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        private void lstVariables_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.selectedItem = lstVariables.SelectedItem;
        }
        #endregion

        #region variable filter
        private void picSearch_Click(object sender, EventArgs e)
        {
            BeginFilterVariableProcess();
        }
        private void picClear_Click(object sender, EventArgs e)
        {
            txtSearchBox.Text = "";
            showAllVariables();
        }
        private void txtSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                BeginFilterVariableProcess();
            }
        }
        private void BeginFilterVariableProcess()
        {
            string keyword = txtSearchBox.Text.ToLower().Trim();
            if (keyword.Length > 0)
            {
                FilterVariableProcess(keyword);
            }
            else
            {
                showAllVariables();
            }
        }
        private void FilterVariableProcess(string keyword)
        {
            var matchedList = new List<string>();
            foreach (var item in bufferdItems)
            {
                if (item.ToLower().Contains(keyword))
                {
                    matchedList.Add(item);
                }
            }
            lstVariables.BeginUpdate();
            lstVariables.Items.Clear();
            lstVariables.Items.AddRange(matchedList.ToArray());
            lstVariables.EndUpdate();
            lstVariables.Focus();
        }
        private void showAllVariables()
        {
            lstVariables.BeginUpdate();
            lstVariables.Items.Clear();
            lstVariables.Items.AddRange(bufferdItems);
            lstVariables.EndUpdate();
            lstVariables.Focus();
        }

        #endregion

    }
}