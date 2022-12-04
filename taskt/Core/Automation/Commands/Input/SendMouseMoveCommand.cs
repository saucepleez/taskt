using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;
using taskt.Core.Script;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("Simulates mouse movements")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to simulate the movement of the mouse, additionally, this command also allows you to perform a click after movement has completed.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'SetCursorPos' function from user32.dll to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class SendMouseMoveCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please enter the X position to move the mouse to")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[Attributes.PropertyAttributes.PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowMouseCaptureHelper)]
        [PropertyCustomUIHelper("Capture Mouse Position", nameof(lnkMouseCapture_Clicked))]
        [InputSpecification("Input the new horizontal coordinate of the mouse, 0 starts at the left and goes to the right")]
        [SampleUsage("**250** or **{{{vXPos}}}**")]
        [Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1920")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        public string v_XMousePosition { get; set; }
        [XmlAttribute]
        [PropertyDescription("Please enter the Y position to move the mouse to")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[Attributes.PropertyAttributes.PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowMouseCaptureHelper)]
        [PropertyCustomUIHelper("Capture Mouse Position", nameof(lnkMouseCapture_Clicked))]
        [InputSpecification("Input the new horizontal coordinate of the window, 0 starts at the left and goes down")]
        [SampleUsage("**250** or **{{{vYPos}}}**")]
        [Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1080")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        public string v_YMousePosition { get; set; }
        [XmlAttribute]
        [PropertyDescription("Please indicate mouse click type if required (defualt is None)")]
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
        [PropertyIsOptional(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_MouseClick { get; set; }

        public SendMouseMoveCommand()
        {
            this.CommandName = "SendMouseMoveCommand";
            this.SelectionName = "Send Mouse Move";
            this.CommandEnabled = true;
            this.CustomRendering = true;
           
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

                var click = v_MouseClick.ConvertToUserVariable(sender);
                if (String.IsNullOrEmpty(click))
                {
                    click = "None";
                }

                if (click != "") 
                {
                    User32Functions.SendMouseClick(v_MouseClick, xLocation, yLocation);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error parsing input to int type (X: " + v_XMousePosition + ", Y:" + v_YMousePosition + ") " + ex.ToString());
            }
        }

        private void lnkMouseCapture_Clicked(object sender, EventArgs e)
        {
            using (UI.Forms.Supplemental.frmShowCursorPosition frmShowCursorPos = new UI.Forms.Supplemental.frmShowCursorPosition())
            {
                if (frmShowCursorPos.ShowDialog() == DialogResult.OK)
                {
                    v_XMousePosition = frmShowCursorPos.xPos.ToString();
                    v_YMousePosition = frmShowCursorPos.yPos.ToString();
                    ((TextBox)ControlsList[nameof(v_XMousePosition)]).Text = v_XMousePosition;
                    ((TextBox)ControlsList[nameof(v_YMousePosition)]).Text = v_YMousePosition;
                }
            }
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_XMousePosition", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_YMousePosition", this, editor));

        //    ////create window name helper control
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_MouseClick", this, editor));

        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

        //    return RenderedControls;

        //}

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Coordinates (" + v_XMousePosition + "," + v_YMousePosition + ") Click: " + v_MouseClick + "]";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_XMousePosition))
            {
                this.validationResult += "X position is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_YMousePosition))
            {
                this.validationResult += "Y position is empty.\n";
                this.IsValid = false;
            }
            return this.IsValid;
        }
    }
}