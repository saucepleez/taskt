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
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmVariableSelector : ThemedForm
    {
        public frmVariableSelector()
        {
            InitializeComponent();
        }

        private void frmVariableSelector_Load(object sender, EventArgs e)
        {
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            if (lstVariables.SelectedItem == null)
            {
                MessageBox.Show("There are no item(s) selected! Select an item and Ok or select Cancel");
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void lstVariables_DoubleClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}