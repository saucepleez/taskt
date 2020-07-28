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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmShowCursorPosition : UIForm
    {
        public MousePoint LPPoint;
        public int XPosition { get; set; }
        public int YPosition { get; set; }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out MousePoint lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct MousePoint
        {
            public int X;
            public int Y;

            public static implicit operator Point(MousePoint point)
            {
                return new Point(point.X, point.Y);
            }
        }

        public frmShowCursorPosition()
        {
            InitializeComponent();
        }

        private void ShowCursorPosition_Load(object sender, EventArgs e)
        {
        }

        private void tmrGetPosition_Tick(object sender, EventArgs e)
        {
            GetCursorPos(out LPPoint);
            lblXPosition.Text = "X Position: " + LPPoint.X;
            lblYPosition.Text = "Y Position: " + LPPoint.Y;
            XPosition = LPPoint.X;
            YPosition = LPPoint.Y;
        }

        private void ShowCursorPosition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void frmShowCursorPosition_MouseEnter(object sender, EventArgs e)
        {
            //move to bottom right if form is in the way
            MoveFormToBottomRight(this);
        }
    }
}