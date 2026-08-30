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
using System.Windows.Forms;

namespace taskt.UI.Forms.ScriptBuilder.CommandEditor.Supplemental
{
    public partial class frmItemSelector : DialogLikeThemedForm
    {
        public object SelectedItem { get; private set; }

        private string[] bufferdItems;

        #region form events
        private frmItemSelector()
        {
            InitializeComponent();
            this.FormClosed += SupplementFormsEvents.SupplementFormClosed;
        }

        public frmItemSelector(List<string> listItems) : this()
        {
            this.bufferdItems = listItems.ToArray();
        }

        public frmItemSelector(List<string> listItems, string title, string headerText) : this()
        {
            this.bufferdItems = listItems.ToArray();
            this.Text = title;
            this.lblHeader.Text = headerText;
        }

        private void frmVariableSelector_Load(object sender, EventArgs e)
        {
            SupplementFormsEvents.SupplementFormLoad(this);
            lstItems.BeginUpdate();
            lstItems.Items.AddRange(bufferdItems);
            lstItems.EndUpdate();
        }
        #endregion

        #region footer buttons event
        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            if (lstItems.SelectedItem == null)
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
            this.SelectedItem = lstItems.SelectedItem;
        }
        #endregion

        #region variable filter
        private void picSearch_Click(object sender, EventArgs e)
        {
            BeginFilterItemsProcess();
        }

        private void picClear_Click(object sender, EventArgs e)
        {
            txtSearchBox.Text = "";
            ShowAllItems();
        }

        private void txtSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                BeginFilterItemsProcess();
            }
        }

        /// <summary>
        /// begin filter matched items process
        /// </summary>
        private void BeginFilterItemsProcess()
        {
            string keyword = txtSearchBox.Text.ToLower().Trim();
            if (keyword.Length > 0)
            {
                FilterItemsProcess(keyword);
            }
            else
            {
                ShowAllItems();
            }
        }

        /// <summary>
        /// filter matched items process
        /// </summary>
        /// <param name="keyword"></param>
        private void FilterItemsProcess(string keyword)
        {
            var matchedList = new List<string>();
            foreach (var item in bufferdItems)
            {
                if (item.ToLower().Contains(keyword))
                {
                    matchedList.Add(item);
                }
            }
            lstItems.BeginUpdate();
            lstItems.Items.Clear();
            lstItems.Items.AddRange(matchedList.ToArray());
            lstItems.EndUpdate();
            lstItems.Focus();
        }

        /// <summary>
        /// show all items in lstItems
        /// </summary>
        private void ShowAllItems()
        {
            lstItems.BeginUpdate();
            lstItems.Items.Clear();
            lstItems.Items.AddRange(bufferdItems);
            lstItems.EndUpdate();
            lstItems.Focus();
        }
        #endregion
    }
}