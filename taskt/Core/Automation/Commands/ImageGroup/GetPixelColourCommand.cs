using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Image")]
    [Attributes.ClassAttributes.CommandSettings("Get Pixel Colour")]
    [Attributes.ClassAttributes.Description("This command gets a pixel at x,y coordinates from current screen.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you need to check a single pixel colour on the screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_camera))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetPixelColourCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("X Coordinate")]
        [InputSpecification("X", true)]
        [PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "X")]
        [PropertyDetailSampleUsage("**100**", PropertyDetailSampleUsage.ValueType.Value, "X")]
        [PropertyDetailSampleUsage("**{{{vXOffset}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "X")]
        [PropertyCustomUIHelper("Capture Mouse Position", nameof(lnkMouseCapture_Clicked))]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        [PropertyDisplayText(true, "X coord")]
        public string v_xCoord { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Y Coordinate")]
        [InputSpecification("Y")]
        [PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "Y")]
        [PropertyDetailSampleUsage("**100**", PropertyDetailSampleUsage.ValueType.Value, "Y")]
        [PropertyDetailSampleUsage("**{{{vYOffset}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Y")]
        [PropertyCustomUIHelper("Capture Mouse Position", nameof(lnkMouseCapture_Clicked))]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        [PropertyDisplayText(true, "Y coord")]
        public string v_yCoord { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_userVariableName { get; set; }

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

        public GetPixelColourCommand()
        {
        }

        public override void AfterShown(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            IntPtr desktopPtr = GetDC(IntPtr.Zero);
            Graphics g = Graphics.FromHdc(desktopPtr);

            if (v_xCoord != null && v_yCoord != null)
            {
                int xCoord = Int32.Parse(v_xCoord);
                int yCoord = Int32.Parse(v_yCoord);
                Pen p = new Pen(Color.Red);
                g.DrawLine(p, xCoord, yCoord - 100, xCoord, yCoord + 100);
                g.DrawLine(p, xCoord - 100, yCoord, xCoord + 100, yCoord);
                g.Dispose();
                ReleaseDC(IntPtr.Zero, desktopPtr);
            }
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var xCoord = this.ExpandValueOrUserVariableAsInteger(nameof(v_xCoord), engine);
            var yCoord = this.ExpandValueOrUserVariableAsInteger(nameof(v_yCoord), engine);
            Point p = new Point(xCoord, yCoord);
            using (var bitmap = new Bitmap(1, 1))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(p, new Point(0, 0), new Size(1, 1));
                }
                Color c = bitmap.GetPixel(0, 0);
                int valueToSet = c.ToArgb();
                valueToSet.StoreInUserVariable(engine, v_userVariableName);
            }
        }

        private void lnkMouseCapture_Clicked(object sender, EventArgs e)
        {
            using (var frmShowCursorPos = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmShowCursorPosition())
            {
                if (frmShowCursorPos.ShowDialog() == DialogResult.OK)
                {
                    v_xCoord = frmShowCursorPos.xPos.ToString();
                    v_yCoord = frmShowCursorPos.yPos.ToString();
                    ControlsList.GetPropertyControl<TextBox>(nameof(v_xCoord)).Text = v_xCoord;
                    ControlsList.GetPropertyControl<TextBox>(nameof(v_yCoord)).Text = v_yCoord;
                }
            }
        }
    }
}