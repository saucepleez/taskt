using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;
using taskt.Core.Script;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("Simulates mouse movements")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to simulate the movement of the mouse, additionally, this command also allows you to perform a click after movement has completed.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'SetCursorPos' function from user32.dll to achieve automation.")]
    public class SendMouseMoveCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the X position to move the mouse to")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowMouseCaptureHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Input the new horizontal coordinate of the mouse, 0 starts at the left and goes to the right")]
        [Attributes.PropertyAttributes.SampleUsage("0")]
        [Attributes.PropertyAttributes.Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1920")]
        public string v_XMousePosition { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the Y position to move the mouse to")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowMouseCaptureHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Input the new horizontal coordinate of the window, 0 starts at the left and goes down")]
        [Attributes.PropertyAttributes.SampleUsage("0")]
        [Attributes.PropertyAttributes.Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1080")]
        public string v_YMousePosition { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate mouse click type if required")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("None")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Double Left Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Up")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate the type of click required")]
        [Attributes.PropertyAttributes.SampleUsage("Select from **Left Click**, **Middle Click**, **Right Click**, **Double Left Click**, **Left Down**, **Middle Down**, **Right Down**, **Left Up**, **Middle Up**, **Right Up** ")]
        [Attributes.PropertyAttributes.Remarks("You can simulate custom click by using multiple mouse click commands in succession, adding **Pause Command** in between where required.")]
        public string v_MouseClick { get; set; }

        public SendMouseMoveCommand()
        {
            this.CommandName = "SendMouseMoveCommand";
            this.SelectionName = "Send Mouse Move";
            this.CommandEnabled = true;
            //modify this value and enable Render() method to enable custom rendering for this or any command.
            //this.CustomRendering = true;
            this.CustomRendering = false;
           
        }

        public override void RunCommand(object sender)
        {
  

            var mouseX = v_XMousePosition.ConvertToUserVariable(sender);
            var mouseY = v_YMousePosition.ConvertToUserVariable(sender);



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

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Coordinates (" + v_XMousePosition + "," + v_YMousePosition + ") Click: " + v_MouseClick + "]";
        }

        //public override List<Control> Render(UI.Forms.frmCommandEditor editor)
        //{
        //    //sample showing custom rendering using helpers
        //    base.Render(editor);

        //    var vXPositionLabel = CommandControls.CreateDefaultLabelFor("v_XMousePosition", this);
        //    var vXPositionBox = CommandControls.CreateDefaultInputFor("v_XMousePosition", this);

        //    var vYPositionLabel = CommandControls.CreateDefaultLabelFor("v_YMousePosition", this);
        //    var vYPositionBox = CommandControls.CreateDefaultInputFor("v_YMousePosition", this);


        //    var vXPositionHelpers = CommandControls.CreateUIHelpersFor("v_XMousePosition", this, new Control[] { vXPositionBox, vYPositionBox }, editor);
        //    var vYPositionHelpers = CommandControls.CreateUIHelpersFor("v_YMousePosition", this, new Control[] { vXPositionBox, vYPositionBox }, editor);

        //    var vMouseClickLabel = CommandControls.CreateDefaultLabelFor("v_MouseClick", this);
        //    var vMouseClickDropdown = CommandControls.CreateDropdownFor("v_MouseClick", this);


        //    var vCommentLabel = CommandControls.CreateDefaultLabelFor("v_Comment", this);
        //    var vCommentBox = CommandControls.CreateDefaultInputFor("v_Comment", this);

        //    RenderedControls.Add(vXPositionLabel);
        //    RenderedControls.AddRange(vXPositionHelpers);
        //    RenderedControls.Add(vXPositionBox);

        //    RenderedControls.Add(vYPositionLabel);
        //    RenderedControls.AddRange(vYPositionHelpers);
        //    RenderedControls.Add(vYPositionBox);      

        //    RenderedControls.Add(vMouseClickLabel);
        //    RenderedControls.Add(vMouseClickDropdown);

        //    RenderedControls.Add(vCommentLabel);
        //    RenderedControls.Add(vCommentBox);

        //    return RenderedControls;


        //}
    }
}