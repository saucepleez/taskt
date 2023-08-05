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
    public partial class CommandItemControl : UserControl
    {
        public CommandItemControl()
        {
            InitializeComponent();
            this.CommandImage = Properties.Resources.command_comment;
            this.DrawIcon = taskt.Properties.Resources.taskt_command_helper;
        }
        public Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType HelperType { get; set; }
        public object DataSource { get; set; }
        public string FunctionalDescription { get; set; }
        public string ImplementationDescription { get; set; }
        public Image DrawIcon { get; set; }
        private string commandDisplay;
        public string CommandDisplay
        {
            get
            {
                return commandDisplay;
            }
            set
            {
                commandDisplay = value;
                this.Invalidate();
            }
        }
        private Image commandImage;
        public Image CommandImage
        {
            get
            {
                return commandImage;
            }
            set
            {
                commandImage = value;
                this.Invalidate();
            }
        }

        private bool isMouseOver = false;
        private SolidBrush mouseOverColor = null;
        private SolidBrush normalColor = null;

        private void CommandItemControl_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            this.isMouseOver = true;
            this.Invalidate();
        }
        private void CommandItemControl_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            this.isMouseOver = false;
            this.Invalidate();
        }

        private void CommandItemControl_Paint(object sender, PaintEventArgs e)
        {
            var strSize = TextRenderer.MeasureText(this.commandDisplay, this.Font);
            this.Size = new Size(strSize.Width + 20, strSize.Height + 2);

            e.Graphics.FillRectangle(
                (isMouseOver ? this.mouseOverColor : this.normalColor),
                0, 0, strSize.Width + 20, strSize.Height);

            e.Graphics.DrawImage(this.DrawIcon, 0, 0, 16, 16);
            e.Graphics.DrawString(this.CommandDisplay, this.Font, new SolidBrush(this.ForeColor), 18, 0);
            
            //Console.WriteLine("## paint!" + DateTime.Now);
        }

        private void CommandItemControl_Load(object sender, EventArgs e)
        {
            this.mouseOverColor = new SolidBrush(Color.FromArgb(64, 255, 255, 255));
            this.normalColor = new SolidBrush(Color.FromArgb(0, 255, 255, 255));
        }
    }
}