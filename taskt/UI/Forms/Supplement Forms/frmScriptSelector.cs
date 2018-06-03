//Copyright (c) 2017 Jason Bayldon
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
    public partial class frmScriptSelector : UIForm
    {
   
        public string[] fileList;
        public frmScriptSelector()
        {
            InitializeComponent();
        }

        private void frmScriptSelector_Load(object sender, EventArgs e)
        {
         

            foreach (var fil in fileList)
            {
                System.IO.FileInfo newFileInfo = new System.IO.FileInfo(fil);
                cboSelectFile.Items.Add(newFileInfo.Name);
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void uiBtnOpen_Click(object sender, EventArgs e)
        {
            if (cboSelectFile.Text == "")
            {
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}