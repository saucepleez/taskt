using System.Windows.Forms;

namespace taskt.UI.CustomControls.CustomUIControls
{
    public class UIPictureBox : PictureBox
    {
        private string _encodedimage;
        public string EncodedImage
        {
            get
            {
                return _encodedimage;
            }
            set
            {
                _encodedimage = value;
            }
        }
    }
}
