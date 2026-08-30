using System.Drawing;

namespace taskt.Core
{
    /// <summary>
    /// Used by Image Recognition to generate and verify a unique fingerprint of an image
    /// </summary>
    public class ImageRecognitionFingerPrint
    {
        public int pixelID { get; set; }
        public Color PixelColor { get; set; }
        public int xLocation { get; set; }
        public int yLocation { get; set; }
        bool matchFound { get; set; }
    }
}
