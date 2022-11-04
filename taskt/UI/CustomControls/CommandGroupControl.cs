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

namespace taskt.UI.CustomControls
{
    public partial class CommandGroupControl : UserControl
    {
        public CommandGroupControl()
        {
            InitializeComponent();
            this.GroupName = "Category";
        }

        private void CommandSelectionControl_Load(object sender, EventArgs e)
        {
        }
        private string groupName;
        public string GroupName
        {
            get
            {
                return groupName;
            }
            set
            {
                groupName = value;
                this.Invalidate();
            }
        }

        private void CommandSelectionControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Properties.Resources.command_files, 5, 0, 32, 32);
            e.Graphics.DrawString(this.GroupName, this.Font, new SolidBrush(this.ForeColor), 39, 10);
        }

        private void CommandSelectionControl_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void CommandSelectionControl_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }
    }
}