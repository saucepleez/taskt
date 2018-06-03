using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmImageCapture : Form
    {
        public frmImageCapture()
        {
            InitializeComponent();
        }
        //These variables control the mouse position
        int selectX;
        int selectY;
        int selectWidth;
        int selectHeight;
        public Pen selectPen;
        public Bitmap userSelectedBitmap;
        //This variable control when you start the right click
        bool start = false;
        private void frmImageCapture_Load(object sender, EventArgs e)
        {

            if (pbTaggedImage.Image != null)
            {
                pnlMouseContainer.Hide();
                return;
            }
            else
            {
                tabTestMode.Hide();
            }




            this.FormBorderStyle = FormBorderStyle.None;
            this.Location = new Point(0, 0);
            this.WindowState = FormWindowState.Maximized;

            //Hide the Form
            this.Hide();
            //Create the Bitmap
            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                     Screen.PrimaryScreen.Bounds.Height);
            //Create the Graphic Variable with screen Dimensions
            Graphics graphics = Graphics.FromImage(printscreen as Image);
            //Copy Image from the screen
            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);
            //Create a temporal memory stream for the image
            using (MemoryStream s = new MemoryStream())
            {
                //save graphic variable into memory
                printscreen.Save(s, ImageFormat.Bmp);
                pictureBox1.Size = new System.Drawing.Size(this.Width, this.Height);
                //set the picture box with temporary stream
                pictureBox1.Image = Image.FromStream(s);
            }
            //Show Form
            this.Show();
            //Cross Cursor
            Cursor = Cursors.Cross;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

            if (uiAccept.Visible == false)
            {
                return;
            }

            //validate if there is an image
            if (pictureBox1.Image == null)
                return;
            //validate if right-click was trigger
            if (start)
            {
                //refresh picture box
                pictureBox1.Refresh();
                //set corner square to mouse coordinates
                selectWidth = e.X - selectX;
                selectHeight = e.Y - selectY;
                //draw dotted rectangle
                pictureBox1.CreateGraphics().DrawRectangle(selectPen,
                          selectX, selectY, selectWidth, selectHeight);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            if (uiAccept.Visible == false)
            {
                return;
            }

            //validate when user right-click
            if (!start)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    //starts coordinates for rectangle
                    selectX = e.X;
                    selectY = e.Y;
                    selectPen = new Pen(Color.Red, 1);
                    selectPen.DashStyle = DashStyle.Solid;
                }
                //refresh picture box
                pictureBox1.Refresh();
                //start control variable for draw rectangle
                start = true;
            }
            else
            {
                //validate if there is image
                if (pictureBox1.Image == null)
                    return;
                //same functionality when mouse is over
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    pictureBox1.Refresh();
                    selectWidth = e.X - selectX;
                    selectHeight = e.Y - selectY;
                    pictureBox1.CreateGraphics().DrawRectangle(selectPen, selectX,
                             selectY, selectWidth, selectHeight);

                }
                start = false;
                //function save image to clipboard
                //SaveToClipboard();
            }
        }
        private void CreateCaptureImage()
        {
            //validate if something selected


                Rectangle rect = new Rectangle(selectX, selectY, selectWidth, selectHeight);
                //create bitmap with original dimensions
                Bitmap OriginalImage = new Bitmap(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);
                //create bitmap with selected dimensions
                Bitmap _img = new Bitmap(selectWidth, selectHeight);
                //create graphic variable
                Graphics g = Graphics.FromImage(_img);
                //set graphic attributes
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(OriginalImage, 0, 0, rect, GraphicsUnit.Pixel);
                //insert image stream into clipboard
                //Clipboard.SetImage(_img);
                userSelectedBitmap = _img;


        }

        private void uiClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void uiAccept_Click(object sender, EventArgs e)
        {

            if (selectWidth == 0)
            {
                MessageBox.Show("Please capture before accepting!");
                return;
            }

            if (selectWidth > 0)
            {
                CreateCaptureImage();
            }

            this.DialogResult = DialogResult.OK;
        }

        private void pnlMouseContainer_MouseEnter(object sender, EventArgs e)
        {
            if (!start)
            {
                return;
            }

            if (pnlMouseContainer.Location == new Point(12,12))
            {
                pnlMouseContainer.Location = new Point(12, this.Height - (48 + pnlMouseContainer.Height));
            }
            else
            {
                pnlMouseContainer.Location = new Point(12, 12);
            }
        }

        private void frmImageCapture_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}
