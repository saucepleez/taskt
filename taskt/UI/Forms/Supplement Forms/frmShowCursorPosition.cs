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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplemental
{
    public partial class frmShowCursorPosition : UIForm
    {
        public POINT lpPoint;
        public int xPos { get; set; }
        public int yPos { get; set; }
        public frmShowCursorPosition()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);
        private void tmrGetPosition_Tick(object sender, EventArgs e)
        {
            GetCursorPos(out lpPoint);
            lblXPosition.Text = "X Position: " + lpPoint.X;
            lblYPosition.Text = "Y Position: " + lpPoint.Y;
            xPos = lpPoint.X;
            yPos = lpPoint.Y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        private void ShowCursorPosition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void ShowCursorPosition_Load(object sender, EventArgs e)
        {
        }

        private void frmShowCursorPosition_MouseEnter(object sender, EventArgs e)
        {
            //move to bottom right if form is in the way
            MoveFormToBottomRight(this);
        }
    }
}