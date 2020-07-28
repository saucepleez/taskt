using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.User32;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{

    [Serializable]
    [Group("Input Commands")]
    [Description("Simulates mouse movements")]
    [UsesDescription("Use this command to simulate the movement of the mouse, additionally, this command also allows you to perform a click after movement has completed.")]
    [ImplementationDescription("This command implements 'SetCursorPos' function from user32.dll to achieve automation.")]
    public class SendMouseMoveCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please enter the X position to move the mouse to")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowMouseCaptureHelper)]
        [InputSpecification("Input the new horizontal coordinate of the mouse, 0 starts at the left and goes to the right")]
        [SampleUsage("0")]
        [Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1920")]
        public string v_XMousePosition { get; set; }
        [XmlAttribute]
        [PropertyDescription("Please enter the Y position to move the mouse to")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowMouseCaptureHelper)]
        [InputSpecification("Input the new horizontal coordinate of the window, 0 starts at the left and goes down")]
        [SampleUsage("0")]
        [Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1080")]
        public string v_YMousePosition { get; set; }
        [XmlAttribute]
        [PropertyDescription("Please indicate mouse click type if required")]
        [PropertyUISelectionOption("None")]
        [PropertyUISelectionOption("Left Click")]
        [PropertyUISelectionOption("Middle Click")]
        [PropertyUISelectionOption("Right Click")]
        [PropertyUISelectionOption("Double Left Click")]
        [PropertyUISelectionOption("Left Down")]
        [PropertyUISelectionOption("Middle Down")]
        [PropertyUISelectionOption("Right Down")]
        [PropertyUISelectionOption("Left Up")]
        [PropertyUISelectionOption("Middle Up")]
        [PropertyUISelectionOption("Right Up")]
        [InputSpecification("Indicate the type of click required")]
        [SampleUsage("Select from **Left Click**, **Middle Click**, **Right Click**, **Double Left Click**, **Left Down**, **Middle Down**, **Right Down**, **Left Up**, **Middle Up**, **Right Up** ")]
        [Remarks("You can simulate custom click by using multiple mouse click commands in succession, adding **Pause Command** in between where required.")]
        public string v_MouseClick { get; set; }

        public SendMouseMoveCommand()
        {
            CommandName = "SendMouseMoveCommand";
            SelectionName = "Send Mouse Move";
            CommandEnabled = true;
            CustomRendering = true;
           
        }

        public override void RunCommand(object sender)
        {

            var engine = (AutomationEngineInstance)sender;
            var mouseX = v_XMousePosition.ConvertToUserVariable(engine);
            var mouseY = v_YMousePosition.ConvertToUserVariable(engine);



            try
            {
                var xLocation = Convert.ToInt32(Math.Floor(Convert.ToDouble(mouseX)));
                var yLocation = Convert.ToInt32(Math.Floor(Convert.ToDouble(mouseY)));

                User32Functions.SetCursorPosition(xLocation, yLocation);
                User32Functions.SendMouseClick(v_MouseClick, xLocation, yLocation);


            }
            catch (Exception ex)
            {
                throw new Exception("Error parsing input to int type (X: " + v_XMousePosition + ", Y:" + v_YMousePosition + ") " + ex.ToString());
            }

          



        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_XMousePosition", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_YMousePosition", this, editor));

            //create window name helper control
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_MouseClick", this, editor));


            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Coordinates (" + v_XMousePosition + "," + v_YMousePosition + ") Click: " + v_MouseClick + "]";
        }

     
    }
}