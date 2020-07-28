using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Common;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommandUtilities;
using taskt.UI.CustomControls;
using taskt.UI.CustomControls.CustomUIControls;

namespace taskt.Commands
{

    [Serializable]
    [Group("Image Commands")]
    [Description("This command attempts to find an existing image on screen.")]
    [UsesDescription("Use this command when you want to attempt to locate an image on screen.  You can subsequently take actions such as move the mouse to the location or perform a click.  This command generates a fingerprint from the comparison image and searches for it in on the desktop.")]
    [ImplementationDescription("TBD")]
    public class ImageRecognitionCommand : ScriptCommand
    {

        [XmlAttribute]
        [PropertyDescription("Capture the search image")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowImageRecogitionHelper)]
        [InputSpecification("Use the tool to capture an image")]
        [SampleUsage("")]
        [Remarks("The image will be used as the image to be found on screen.")]
        public string v_ImageCapture { get; set; }

        [XmlAttribute]
        [PropertyDescription("Offset X Coordinate - Optional")]
        [InputSpecification("Specify if an offset is required.")]
        [SampleUsage("0 or 100")]
        [Remarks("This will move the mouse X pixels to the right of the location of the image")]
        public int v_xOffsetAdjustment { get; set; }

        [XmlAttribute]
        [PropertyDescription("Offset Y Coordinate - Optional")]
        [InputSpecification("Specify if an offset is required.")]
        [SampleUsage("0 or 100")]
        [Remarks("This will move the mouse X pixels down from the top of the location of the image")]
        public int v_YOffsetAdjustment { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate mouse click type if required")]
        [PropertyUISelectionOption("None")]
        [PropertyUISelectionOption("Left Click")]
        [PropertyUISelectionOption("Middle Click")]
        [PropertyUISelectionOption("Right Click")]
        [PropertyUISelectionOption("Left Down")]
        [PropertyUISelectionOption("Middle Down")]
        [PropertyUISelectionOption("Right Down")]
        [PropertyUISelectionOption("Left Up")]
        [PropertyUISelectionOption("Middle Up")]
        [PropertyUISelectionOption("Right Up")]
        [PropertyUISelectionOption("Double Left Click")]
        [InputSpecification("Indicate the type of click required")]
        [SampleUsage("Select from **Left Click**, **Middle Click**, **Right Click**, **Double Left Click**, **Left Down**, **Middle Down**, **Right Down**, **Left Up**, **Middle Up**, **Right Up** ")]
        [Remarks("You can simulate custom click by using multiple mouse click commands in succession, adding **Pause Command** in between where required.")]
        public string v_MouseClick { get; set; }

        [XmlAttribute]
        [PropertyDescription("Timeout (seconds, 0 for unlimited search time)")]
        [InputSpecification("Enter a timeout length if required.")]
        [SampleUsage("")]
        [Remarks("Search times become excessive for colors such as white. For best results, capture a large color variance on screen, not just a white block.")]
        public double v_TimeoutSeconds { get; set; }

        public bool TestMode = false;

        public ImageRecognitionCommand()
        {
            CommandName = "ImageRecognitionCommand";
            SelectionName = "Image Recognition";
            CommandEnabled = true;
            CustomRendering = true;

            v_xOffsetAdjustment = 0;
            v_YOffsetAdjustment = 0;
            v_TimeoutSeconds = 30;
        }

        public override void RunCommand(object sender)
        {
            bool testMode = TestMode;
           
            //user image to bitmap
            Bitmap userImage = new Bitmap(Common.Base64ToImage(v_ImageCapture));

            //take screenshot
            Size shotSize = Screen.PrimaryScreen.Bounds.Size;
            Point upperScreenPoint = new Point(0, 0);
            Point upperDestinationPoint = new Point(0, 0);
            Bitmap desktopImage = new Bitmap(shotSize.Width, shotSize.Height);
            Graphics graphics = Graphics.FromImage(desktopImage);
            graphics.CopyFromScreen(upperScreenPoint, upperDestinationPoint, shotSize);

            //create desktopOutput file
            Bitmap desktopOutput = new Bitmap(desktopImage);

            //get graphics for drawing on output file
            Graphics screenShotUpdate = Graphics.FromImage(desktopOutput);

            //declare maximum boundaries
            int userImageMaxWidth = userImage.Width - 1;
            int userImageMaxHeight = userImage.Height - 1;
            int desktopImageMaxWidth = desktopImage.Width - 1;
            int desktopImageMaxHeight = desktopImage.Height - 1;

            //newfingerprinttechnique

            //create desktopOutput file
            Bitmap sampleOut = new Bitmap(userImage);

            //get graphics for drawing on output file
            Graphics sampleUpdate = Graphics.FromImage(sampleOut);

            List<ImageRecognitionFingerPrint> uniqueFingerprint = new List<ImageRecognitionFingerPrint>();
            Color lastcolor = Color.Transparent;

            //create fingerprint
            var pixelDensity = (userImage.Width * userImage.Height);

            int iteration = 0;
            Random random = new Random();
            while ((uniqueFingerprint.Count() < 10) && (iteration < pixelDensity))
            {
                int x = random.Next(userImage.Width);
                int y = random.Next(userImage.Height);
                Color color = sampleOut.GetPixel(x, y);

                if ((lastcolor != color) && (!uniqueFingerprint.Any(f => f.XLocation == x && f.YLocation == y)))
                {
                    uniqueFingerprint.Add(new ImageRecognitionFingerPrint() { PixelColor = color, XLocation = x, YLocation = y });
                    sampleUpdate.DrawRectangle(Pens.Yellow, x, y, 1, 1);
                }

                iteration++;
            }

            //begin search
            DateTime timeoutDue = DateTime.Now.AddSeconds(v_TimeoutSeconds);

            bool imageFound = false;
            //for each row on the screen
            for (int rowPixel = 0; rowPixel < desktopImage.Height - 1; rowPixel++)
            {
                if (rowPixel + uniqueFingerprint.First().YLocation >= desktopImage.Height)
                    continue;

                //for each column on screen
                for (int columnPixel = 0; columnPixel < desktopImage.Width - 1; columnPixel++)
                {
                    if ((v_TimeoutSeconds > 0) && (DateTime.Now > timeoutDue))
                    {
                        throw new Exception("Image recognition command ran out of time searching for image");
                    }

                    if (columnPixel + uniqueFingerprint.First().XLocation >= desktopImage.Width)
                        continue;

                    try
                    {
                        //get the current pixel from current row and column
                        // userImageFingerPrint.First() for now will always be from top left (0,0)
                        var currentPixel = desktopImage.GetPixel(columnPixel + uniqueFingerprint.First().XLocation, rowPixel + uniqueFingerprint.First().YLocation);

                        //compare to see if desktop pixel matches top left pixel from user image
                        if (currentPixel == uniqueFingerprint.First().PixelColor)
                        {

                            //look through each item in the fingerprint to see if offset pixel colors match
                            int matchCount = 0;
                            for (int item = 0; item < uniqueFingerprint.Count; item++)
                            {
                                //find pixel color from offset X,Y relative to current position of row and column
                                currentPixel = desktopImage.GetPixel(columnPixel + uniqueFingerprint[item].XLocation, rowPixel + uniqueFingerprint[item].YLocation);

                                //if color matches
                                if (uniqueFingerprint[item].PixelColor == currentPixel)
                                {
                                    matchCount++;

                                    //draw on output to demonstrate finding
                                    if (testMode)
                                        screenShotUpdate.DrawRectangle(Pens.Blue, columnPixel + uniqueFingerprint[item].XLocation, rowPixel + uniqueFingerprint[item].YLocation, 5, 5);
                                }
                                else
                                {
                                    //mismatch in the pixel series, not a series of matching coordinate
                                    //?add threshold %?
                                    imageFound = false;

                                    //draw on output to demonstrate finding
                                    if (testMode)
                                        screenShotUpdate.DrawRectangle(Pens.OrangeRed, columnPixel + uniqueFingerprint[item].XLocation, rowPixel + uniqueFingerprint[item].YLocation, 5, 5);
                                }

                            }

                            if (matchCount == uniqueFingerprint.Count())
                            {
                                imageFound = true;

                                var topLeftX = columnPixel;
                                var topLeftY = rowPixel;

                                if (testMode)
                                {
                                    //draw on output to demonstrate finding
                                    var Rectangle = new Rectangle(topLeftX, topLeftY, userImageMaxWidth, userImageMaxHeight);
                                    Brush brush = new SolidBrush(Color.ForestGreen);
                                    screenShotUpdate.FillRectangle(brush, Rectangle);
                                }

                                //move mouse to position
                                var mouseMove = new SendMouseMoveCommand
                                {
                                    v_XMousePosition = (topLeftX + (v_xOffsetAdjustment)).ToString(),
                                    v_YMousePosition = (topLeftY + (v_xOffsetAdjustment)).ToString(),
                                    v_MouseClick = v_MouseClick
                                };

                                mouseMove.RunCommand(sender);
                            }
                        }

                        if (imageFound)
                            break;

                    }
                    catch (Exception)
                    {
                        //continue
                    }
                }

                if (imageFound)
                    break;
            }

            if (testMode)
            {
                //screenShotUpdate.FillRectangle(Brushes.White, 5, 20, 275, 105);
                //screenShotUpdate.DrawString("Blue = Matching Point", new Font("Arial", 12, FontStyle.Bold), Brushes.SteelBlue, 5, 20);
                // screenShotUpdate.DrawString("OrangeRed = Mismatched Point", new Font("Arial", 12, FontStyle.Bold), Brushes.SteelBlue, 5, 60);
                // screenShotUpdate.DrawString("Green Rectangle = Match Area", new Font("Arial", 12, FontStyle.Bold), Brushes.SteelBlue, 5, 100);

                //screenShotUpdate.DrawImage(sampleOut, desktopOutput.Width - sampleOut.Width, 0);

                UI.Forms.Supplement_Forms.frmImageCapture captureOutput = new UI.Forms.Supplement_Forms.frmImageCapture();
                captureOutput.pbTaggedImage.Image = sampleOut;
                captureOutput.pbSearchResult.Image = desktopOutput;
                captureOutput.Show();
                captureOutput.TopMost = true;
                //captureOutput.WindowState = FormWindowState.Maximized;
            }

            graphics.Dispose();
            userImage.Dispose();
            desktopImage.Dispose();
            screenShotUpdate.Dispose();

            if (!imageFound)
            {
                throw new Exception("Specified image was not found in window!");
            }


        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);


            UIPictureBox imageCapture = new UIPictureBox();
            imageCapture.Width = 200;
            imageCapture.Height = 200;
            imageCapture.DataBindings.Add("EncodedImage", this, "v_ImageCapture", false, DataSourceUpdateMode.OnPropertyChanged);

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_ImageCapture", this));
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_ImageCapture", this, new Control[] { imageCapture }, editor));
            RenderedControls.Add(imageCapture);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_xOffsetAdjustment", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_YOffsetAdjustment", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_MouseClick", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TimeoutSeconds", this, editor));


            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Find On Screen]";
        }
    }
}