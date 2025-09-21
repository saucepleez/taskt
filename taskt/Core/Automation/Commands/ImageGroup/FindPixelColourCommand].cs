using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Image")]
    [Attributes.ClassAttributes.CommandSettings("Find Pixel Colour")]
    [Attributes.ClassAttributes.Description("This command searches current screen for a pixel of a certain colour inside a rectangle.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you need to find a coloured single pixel in a region on the screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_camera))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class FindPixelColourCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("X1 Coordinate")]
        [InputSpecification("X1", true)]
        [PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "X")]
        [PropertyDetailSampleUsage("**100**", PropertyDetailSampleUsage.ValueType.Value, "X")]
        [PropertyDetailSampleUsage("**{{{vXOffset}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "X")]
        [PropertyCustomUIHelper("Capture Mouse Position", nameof(lnkMouseCapture_Clicked))]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        [PropertyDisplayText(true, "X1 coord")]
        public string v_x1Coord { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Y1 Coordinate")]
        [InputSpecification("Y1")]
        [PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "Y")]
        [PropertyDetailSampleUsage("**100**", PropertyDetailSampleUsage.ValueType.Value, "Y")]
        [PropertyDetailSampleUsage("**{{{vYOffset}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Y")]
        //[PropertyCustomUIHelper("Capture Mouse Position", nameof(lnkMouseCapture_Clicked))]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        [PropertyDisplayText(true, "Y1 coord")]
        public string v_y1Coord { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("X2 Coordinate")]
        [InputSpecification("X2", true)]
        [PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "X")]
        [PropertyDetailSampleUsage("**100**", PropertyDetailSampleUsage.ValueType.Value, "X")]
        [PropertyDetailSampleUsage("**{{{vXOffset}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "X")]
        [PropertyCustomUIHelper("Capture Mouse Position", nameof(lnkMouseCapture_Clicked2))]
        [PropertyIsOptional(true, "1")]
        [PropertyFirstValue("1")]
        [PropertyDisplayText(true, "X2 coord")]
        public string v_x2Coord { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Y2 Coordinate")]
        [InputSpecification("Y2")]
        [PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "Y")]
        [PropertyDetailSampleUsage("**100**", PropertyDetailSampleUsage.ValueType.Value, "Y")]
        [PropertyDetailSampleUsage("**{{{vYOffset}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Y")]
        //[PropertyCustomUIHelper("Capture Mouse Position", nameof(lnkMouseCapture_Clicked))]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyIsOptional(true, "1")]
        [PropertyFirstValue("1")]
        [PropertyDisplayText(true, "Y2 coord")]
        public string v_y2Coord { get; set; }

        [XmlAttribute]
        [PropertyDescription("Color code to search for")]
        public string v_ColorCodeToSearch { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_userVariableName { get; set; }

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

        public FindPixelColourCommand()
        {
        }

        public override void AfterShown(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            IntPtr desktopPtr = GetDC(IntPtr.Zero);
            Graphics g = Graphics.FromHdc(desktopPtr);

            if (v_x1Coord != null && v_y1Coord != null && v_x2Coord != null && v_y2Coord != null)
            {
                int x1Coord = Int32.Parse(v_x1Coord);
                int y1Coord = Int32.Parse(v_y1Coord);
                int x2Coord = Int32.Parse(v_x2Coord);
                int y2Coord = Int32.Parse(v_y2Coord);
                Pen p = new Pen(Color.Red);
                g.DrawRectangle(p, x1Coord, y1Coord, x2Coord - x1Coord, y2Coord - y1Coord);
                g.Dispose();
                ReleaseDC(IntPtr.Zero, desktopPtr);
            }
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var x1Coord = this.ExpandValueOrUserVariableAsInteger(nameof(v_x1Coord), engine);
            var y1Coord = this.ExpandValueOrUserVariableAsInteger(nameof(v_y1Coord), engine);
            var x2Coord = this.ExpandValueOrUserVariableAsInteger(nameof(v_x2Coord), engine);
            var y2Coord = this.ExpandValueOrUserVariableAsInteger(nameof(v_y2Coord), engine);
            var ColorCodeToSearch = this.ExpandValueOrUserVariableAsInteger(nameof(v_ColorCodeToSearch), engine);
            int width = x2Coord - x1Coord;
            int height = y2Coord - y1Coord;

            false.StoreInUserVariable(engine, v_userVariableName);
            using (var bitmap = new Bitmap(width, height))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(new Point(x1Coord, y1Coord), Point.Empty, new Size(width, height));
                    for (int X = 0; X < width; X++)
                    {
                        for (int Y = 0; Y < height; Y++)
                        {
                            if (bitmap.GetPixel(X, Y).ToArgb() == ColorCodeToSearch)
                            {
                                true.StoreInUserVariable(engine, v_userVariableName);
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void lnkMouseCapture_Clicked(object sender, EventArgs e)
        {
            using (var frmShowCursorPos = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmShowCursorPosition())
            {
                if (frmShowCursorPos.ShowDialog() == DialogResult.OK)
                {
                    v_x1Coord = frmShowCursorPos.xPos.ToString();
                    v_y1Coord = frmShowCursorPos.yPos.ToString();
                    ControlsList.GetPropertyControl<TextBox>(nameof(v_x1Coord)).Text = v_x1Coord;
                    ControlsList.GetPropertyControl<TextBox>(nameof(v_y1Coord)).Text = v_y1Coord;
                }
            }
        }
        private void lnkMouseCapture_Clicked2(object sender, EventArgs e)
        {
            using (var frmShowCursorPos = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmShowCursorPosition())
            {
                if (frmShowCursorPos.ShowDialog() == DialogResult.OK)
                {
                    v_x2Coord = frmShowCursorPos.xPos.ToString();
                    v_y2Coord = frmShowCursorPos.yPos.ToString();
                    ControlsList.GetPropertyControl<TextBox>(nameof(v_x2Coord)).Text = v_x2Coord;
                    ControlsList.GetPropertyControl<TextBox>(nameof(v_y2Coord)).Text = v_y2Coord;
                }
            }
        }
    }
}