using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmImageCapture : Form
    {
        //These variables control the mouse position
        public Pen SelectPen;
        public Bitmap UserSelectedBitmap;
        private int _selectX;
        private int _selectY;
        private int _selectWidth;
        private int _selectHeight;

        //This variable control when you start the right click
        private bool _start = false;

        public frmImageCapture()
        {
            InitializeComponent();
        }

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

            FormBorderStyle = FormBorderStyle.None;
            Location = new Point(0, 0);
            WindowState = FormWindowState.Maximized;

            //Hide the Form
            Hide();

            //Create the Bitmap
            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            //Create the Graphic Variable with screen Dimensions
            Graphics graphics = Graphics.FromImage(printscreen as Image);

            //Copy Image from the screen
            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);

            //Create a temporal memory stream for the image
            using (MemoryStream s = new MemoryStream())
            {
                //save graphic variable into memory
                printscreen.Save(s, ImageFormat.Bmp);

                pbMainImage.Size = new Size(Width, Height);

                //set the picture box with temporary stream
                pbMainImage.Image = Image.FromStream(s);
            }
            //Show Form
            Show();

            //Cross Cursor
            Cursor = Cursors.Cross;
        }

        private void pbMainImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (uiAccept.Visible == false)
            {
                return;
            }

            //validate if there is an image
            if (pbMainImage.Image == null)
                return;

            //validate if right-click was trigger
            if (_start)
            {
                //refresh picture box
                pbMainImage.Refresh();

                //set corner square to mouse coordinates
                _selectWidth = e.X - _selectX;
                _selectHeight = e.Y - _selectY;

                //draw dotted rectangle
                pbMainImage.CreateGraphics().DrawRectangle(SelectPen, _selectX, _selectY, _selectWidth, _selectHeight);
            }
        }

        private void pbMainImage_MouseDown(object sender, MouseEventArgs e)
        {

            if (uiAccept.Visible == false)
            {
                return;
            }

            //validate when user right-click
            if (!_start)
            {
                if (e.Button == MouseButtons.Left)
                {
                    //starts coordinates for rectangle
                    _selectX = e.X;
                    _selectY = e.Y;
                    SelectPen = new Pen(Color.Red, 1);
                    SelectPen.DashStyle = DashStyle.Solid;
                }
                //refresh picture box
                pbMainImage.Refresh();

                //start control variable for draw rectangle
                _start = true;
            }
            else
            {
                //validate if there is image
                if (pbMainImage.Image == null)
                    return;

                //same functionality when mouse is over
                if (e.Button == MouseButtons.Left)
                {
                    pbMainImage.Refresh();
                    _selectWidth = e.X - _selectX;
                    _selectHeight = e.Y - _selectY;
                    pbMainImage.CreateGraphics().DrawRectangle(SelectPen, _selectX, _selectY, _selectWidth, _selectHeight);
                }

                _start = false;
                //function save image to clipboard
                //SaveToClipboard();
            }
        }

        private void CreateCaptureImage()
        {
            //validate if something selected
            Rectangle rect = new Rectangle(_selectX, _selectY, _selectWidth, _selectHeight);

            //create bitmap with original dimensions
            Bitmap OriginalImage = new Bitmap(pbMainImage.Image, pbMainImage.Width, pbMainImage.Height);

            //create bitmap with selected dimensions
            Bitmap _img = new Bitmap(_selectWidth, _selectHeight);

            //create graphic variable
            Graphics g = Graphics.FromImage(_img);

            //set graphic attributes
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.DrawImage(OriginalImage, 0, 0, rect, GraphicsUnit.Pixel);

            //insert image stream into clipboard
            //Clipboard.SetImage(_img);
            UserSelectedBitmap = _img;
        }

        private void uiClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void uiAccept_Click(object sender, EventArgs e)
        {

            if (_selectWidth == 0)
            {
                MessageBox.Show("Please capture before accepting!");
                return;
            }

            if (_selectWidth > 0)
            {
                CreateCaptureImage();
            }

            DialogResult = DialogResult.OK;
        }

        private void pnlMouseContainer_MouseEnter(object sender, EventArgs e)
        {
            if (!_start)
            {
                return;
            }

            if (pnlMouseContainer.Location == new Point(12,12))
            {
                pnlMouseContainer.Location = new Point(12, Height - (48 + pnlMouseContainer.Height));
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
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}
