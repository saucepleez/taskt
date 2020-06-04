using System.Drawing;

namespace taskt.Core.Utilities.CommandUtilities
{
    /// <summary>
    /// Used by Image Recognition to generate and verify a unique fingerprint of an image
    /// </summary>
    public class ImageRecognitionFingerPrint
    {
        public int PixelId { get; set; }
        public Color PixelColor { get; set; }
        public int XLocation { get; set; }
        public int YLocation { get; set; }
        public bool MatchFound { get; set; }
    }
}
