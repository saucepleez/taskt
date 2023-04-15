using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Drawing;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Image Commands")]
    [Attributes.ClassAttributes.CommandSettings("Image Recognition")]
    [Attributes.ClassAttributes.Description("This command attempts to find an existing image on screen.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to attempt to locate an image on screen.  You can subsequently take actions such as move the mouse to the location or perform a click.  This command generates a fingerprint from the comparison image and searches for it in on the desktop.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ImageRecognitionCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Specify the Search Image")]
        [InputSpecification("Specify the Search Image")]
        [Remarks("The image will be used as the image to be found on screen.")]
        [PropertyCustomUIHelper("Capture Reference Image", nameof(ShowImageCapture))]
        [PropertyCustomUIHelper("Run Image Recognition Test", nameof(RunImageCapture))]
        [PropertyValidationRule("Search Image", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_ImageCapture { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Mouse Click Type")]
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
        [InputSpecification("", true)]
        [Remarks("You can simulate custom click by using multiple mouse click commands in succession, adding **Pause Command** in between where required.")]
        [PropertyIsOptional(true, "None")]
        [PropertyDisplayText(true, "Click")]
        public string v_MouseClick { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Offset X Coordinate")]
        [InputSpecification("Offset X", true)]
        [PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "Offset X")]
        [PropertyDetailSampleUsage("**100**", PropertyDetailSampleUsage.ValueType.Value, "Offset X")]
        [PropertyDetailSampleUsage("**{{{vXOffset}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Offset X")]
        [Remarks("This will move the mouse X pixels to the right of the location of the image")]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        [PropertyDisplayText(true, "Offset X")]
        public string v_xOffsetAdjustment { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Offset Y Coordinate")]
        [InputSpecification("Offset Y")]
        [PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "Offset Y")]
        [PropertyDetailSampleUsage("**100**", PropertyDetailSampleUsage.ValueType.Value, "Offset Y")]
        [PropertyDetailSampleUsage("**{{{vYOffset}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Offset Y")]
        [Remarks("This will move the mouse Y pixels down from the top of the location of the image")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        [PropertyDisplayText(true, "Offset Y")]
        public string v_YOffsetAdjustment { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Timeout Length (sec)")]
        [InputSpecification("Timeout Length", true)]
        //[SampleUsage("**30** or **0** or **{{{vTimeout}}}**")]
        [PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "Timeout")]
        [PropertyDetailSampleUsage("**30**", PropertyDetailSampleUsage.ValueType.Value, "Timeout")]
        [PropertyDetailSampleUsage("**{{{vTimeout}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Timeout")]
        [Remarks("Search times become excessive for colors such as white. For best results, capture a large color variance on screen, not just a white block.")]
        [PropertyIsOptional(true, "30")]
        [PropertyFirstValue("30")]
        [PropertyDisplayText(true, "Timeout", "sec")]
        public string v_TimeoutSeconds { get; set; }

        /// <summary>
        /// for test mode
        /// </summary>
        public bool TestMode = false;

        public ImageRecognitionCommand()
        {
            //this.CommandName = "ImageRecognitionCommand";
            //this.SelectionName = "Image Recognition";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;

            //v_xOffsetAdjustment = "0";
            //v_YOffsetAdjustment = "0";
            //v_TimeoutSeconds = "30";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

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

                if ((lastcolor != color) && (!uniqueFingerprint.Any(f => f.xLocation == x && f.yLocation == y)))
                {
                    uniqueFingerprint.Add(new ImageRecognitionFingerPrint() { PixelColor = color, xLocation = x, yLocation = y });
                    sampleUpdate.DrawRectangle(Pens.Yellow, x, y, 1, 1);
                }

                iteration++;
            }

            //begin search
            double timeoutSeconds = this.ConvertToUserVariableAsInteger(nameof(v_TimeoutSeconds), engine);
            DateTime timeoutDue = DateTime.Now.AddSeconds(timeoutSeconds);

            bool imageFound = false;
            //for each row on the screen
            for (int rowPixel = 0; rowPixel < desktopImage.Height - 1; rowPixel++)
            {
                if (rowPixel + uniqueFingerprint.First().yLocation >= desktopImage.Height)
                {
                    continue;
                }

                //for each column on screen
                for (int columnPixel = 0; columnPixel < desktopImage.Width - 1; columnPixel++)
                {

                    if ((timeoutSeconds > 0) && (DateTime.Now > timeoutDue))
                    {
                        throw new Exception("Image Recognition command ran out of time searching for image.");
                    }

                    if (columnPixel + uniqueFingerprint.First().xLocation >= desktopImage.Width)
                    {
                        continue;
                    }
                        
                    try
                    {
                        //get the current pixel from current row and column
                        // userImageFingerPrint.First() for now will always be from top left (0,0)
                        var currentPixel = desktopImage.GetPixel(columnPixel + uniqueFingerprint.First().xLocation, rowPixel + uniqueFingerprint.First().yLocation);

                        //compare to see if desktop pixel matches top left pixel from user image
                        if (currentPixel == uniqueFingerprint.First().PixelColor)
                        {
                            //look through each item in the fingerprint to see if offset pixel colors match
                            int matchCount = 0;
                            for (int item = 0; item < uniqueFingerprint.Count; item++)
                            {
                                //find pixel color from offset X,Y relative to current position of row and column
                                currentPixel = desktopImage.GetPixel(columnPixel + uniqueFingerprint[item].xLocation, rowPixel + uniqueFingerprint[item].yLocation);

                                //if color matches
                                if (uniqueFingerprint[item].PixelColor == currentPixel)
                                {
                                    matchCount++;

                                    //draw on output to demonstrate finding
                                    if (testMode)
                                        screenShotUpdate.DrawRectangle(Pens.Blue, columnPixel + uniqueFingerprint[item].xLocation, rowPixel + uniqueFingerprint[item].yLocation, 5, 5);
                                }
                                else
                                {
                                    //mismatch in the pixel series, not a series of matching coordinate
                                    //?add threshold %?
                                    imageFound = false;

                                    //draw on output to demonstrate finding
                                    if (testMode)
                                    {
                                        screenShotUpdate.DrawRectangle(Pens.OrangeRed, columnPixel + uniqueFingerprint[item].xLocation, rowPixel + uniqueFingerprint[item].yLocation, 5, 5);
                                    }
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

                                var xOffset = this.ConvertToUserVariableAsInteger(nameof(v_xOffsetAdjustment), engine);
                                var yOffset = this.ConvertToUserVariableAsInteger(nameof(v_YOffsetAdjustment), engine);
                                var mouseClick = this.GetUISelectionValue(nameof(v_MouseClick), engine);

                                //move mouse to position
                                var mouseMove = new MoveMouseCommand
                                {
                                    v_XMousePosition = (topLeftX + xOffset).ToString(),
                                    v_YMousePosition = (topLeftY + yOffset).ToString(),
                                    v_MouseClick = mouseClick
                                };

                                mouseMove.RunCommand(engine);
                            }
                        }

                        if (imageFound)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        //continue, no need error message
                    }
                }

                if (imageFound)
                {
                    break;
                }
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
                //captureOutput.WindowState = System.Windows.Forms.FormWindowState.Maximized;
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

        private void ShowImageCapture(object sender, EventArgs e)
        {
            //if (minimizePreference)
            //{
            //    settings.ClientSettings.MinimizeToTray = false;
            //    settings.Save(settings);
            //}

            HideAllForms();

            //var userAcceptance = MessageBox.Show("The image capture process will now begin and display a screenshot of the current desktop in a custom full-screen window.  You may stop the capture process at any time by pressing the 'ESC' key, or selecting 'Close' at the top left. Simply create the image by clicking once to start the rectangle and clicking again to finish. The image will be cropped to the boundary within the red rectangle. Shall we proceed?", "Image Capture", MessageBoxButtons.YesNo);
            using (var fm = new UI.Forms.Supplemental.frmDialog("The image capture process will now begin and display a screenshot of the current desktop in a custom full-screen window.\nYou may stop the capture process at any time by pressing the 'ESC' key, or selecting 'Close' at the top left. Simply create the image by clicking once to start the rectangle and clicking again to finish.\nThe image will be cropped to the boundary within the red rectangle. Shall we proceed?", "Image Capture", UI.Forms.Supplemental.frmDialog.DialogType.YesNo, 0))
            {
                if (fm.ShowDialog() == DialogResult.OK)
                {
                    using (UI.Forms.Supplement_Forms.frmImageCapture imageCaptureForm = new UI.Forms.Supplement_Forms.frmImageCapture())
                    {
                        if (imageCaptureForm.ShowDialog() == DialogResult.OK)
                        {
                            var targetPictureBox = (UIPictureBox)((CommandItemControl)sender).Tag;

                            targetPictureBox.Image = imageCaptureForm.userSelectedBitmap;
                            var convertedImage = Common.ImageToBase64(imageCaptureForm.userSelectedBitmap);
                            var convertedLength = convertedImage.Length;
                            targetPictureBox.EncodedImage = convertedImage;

                            // force set property value
                            //if (editor.selectedCommand.CommandName == "ImageRecognitionCommand")
                            //{
                            //    ((Core.Automation.Commands.ImageRecognitionCommand)editor.selectedCommand).v_ImageCapture = convertedImage;
                            //}

                            v_ImageCapture = convertedImage;

                            //imageCaptureForm.Show();
                        }
                    }
                }
            }

            ShowAllForms();

            //if (minimizePreference)
            //{
            //    settings.ClientSettings.MinimizeToTray = true;
            //    settings.Save(settings);
            //}
        }

        private void RunImageCapture(object sender, EventArgs e)
        {
            //get input control
            var imageSource = ((UIPictureBox)((CommandItemControl)sender).Tag).EncodedImage;

            // image is empty
            if (string.IsNullOrEmpty(imageSource))
            {
                using (var fm = new UI.Forms.Supplemental.frmDialog("Please capture an image before attempting to test!", "Image Recognition", UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, 0))
                {
                    fm.ShowDialog();
                    return;
                }
            }

            //hide all
            HideAllForms();

            try
            {
                //run image recognition
                ImageRecognitionCommand imageRecognitionCommand = new ImageRecognitionCommand
                {
                    v_ImageCapture = imageSource,
                    TestMode = true
                };
                imageRecognitionCommand.RunCommand(new Engine.AutomationEngineInstance());
            }
            catch (Exception ex)
            {
                using (var fm = new UI.Forms.Supplemental.frmDialog("Error: " + ex.Message, "Image Recognition", UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, 0))
                {
                    fm.ShowDialog();
                }
            }
            //show all forms
            ShowAllForms();
        }

        private static void ShowAllForms()
        {
            foreach (Form frm in Application.OpenForms)
            {
                frm.WindowState = FormWindowState.Normal;
            }
        }
        private static void HideAllForms()
        {
            foreach (Form frm in Application.OpenForms)
            {
                frm.WindowState = FormWindowState.Minimized;
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            UIPictureBox imageCapture = new UIPictureBox
            {
                Width = 200,
                Height = 200
            };
            imageCapture.DataBindings.Add(nameof(imageCapture.EncodedImage), this, v_ImageCapture, false, DataSourceUpdateMode.OnPropertyChanged);

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor(nameof(v_ImageCapture), this));
            RenderedControls.AddRange(CommandControls.CreateCustomUIHelpersFor(nameof(v_ImageCapture), this, imageCapture, editor));
            RenderedControls.Add(imageCapture);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(new List<string>() {
                 nameof(v_MouseClick), nameof(v_xOffsetAdjustment), nameof(v_YOffsetAdjustment),nameof(v_TimeoutSeconds) 
            }, this, editor);
            RenderedControls.AddRange(ctrls);

            return RenderedControls;
        }
    }
}