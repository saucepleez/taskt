using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Key/Mouse Commands")]
    [Attributes.ClassAttributes.SubGruop("Mouse")]
    [Attributes.ClassAttributes.CommandSettings("Move Mouse")]
    [Attributes.ClassAttributes.Description("Simulates mouse movements")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to simulate the movement of the mouse, additionally, this command also allows you to perform a click after movement has completed.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'SetCursorPos' function from user32.dll to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MoveMouseCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("X Position to Move the Mouse to")]
        [InputSpecification("X Position", true)]
        [PropertyDetailSampleUsage("**250**", PropertyDetailSampleUsage.ValueType.Value, "X Position")]
        [PropertyDetailSampleUsage("**{{{vXPos}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "X Position")]
        [PropertyCustomUIHelper("Capture Mouse Position", nameof(lnkMouseCapture_Clicked))]
        [Remarks("Input the new horizontal coordinate of the mouse, 0 starts at the left and goes to the right. This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1920")]
        [PropertyValidationRule("X Position", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "X Position")]
        public string v_XMousePosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Y Position to Move the Mouse to")]        
        [InputSpecification("Y Position", true)]
        [PropertyDetailSampleUsage("**250**", PropertyDetailSampleUsage.ValueType.Value, "Y Position")]
        [PropertyDetailSampleUsage("**{{{vYPos}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Y Position")]
        [Remarks("Input the new horizontal coordinate of the window, 0 starts at the left and goes down. This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1080")]
        [PropertyCustomUIHelper("Capture Mouse Position", nameof(lnkMouseCapture_Clicked))]
        [PropertyValidationRule("Y Position", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Y Position")]
        public string v_YMousePosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_MouseClickType))]
        [PropertyIsOptional(true, "None")]
        [PropertyValidationRule("Click Type", PropertyValidationRule.ValidationRuleFlags.None)]
        public string v_MouseClick { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_WaitTimeAfterMouseClick))]
        public string v_WaitTimeAfterClick { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Ignore Wait Time When Click Type is 'None'")]
        [PropertyIsOptional(true, "Yes")]
        [PropertyFirstValue("Yes")]
        public string v_IgnoreWaitTime { get; set; }

        public MoveMouseCommand()
        {
            //this.CommandName = "SendMouseMoveCommand";
            //this.SelectionName = "Send Mouse Move";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;  
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var mouseX = this.ExpandValueOrUserVariableAsInteger(nameof(v_XMousePosition), engine);
            var mouseY = this.ExpandValueOrUserVariableAsInteger(nameof(v_YMousePosition), engine);

            try
            {
                KeyMouseControls.SetCursorPosition(mouseX, mouseY);

                if (!String.IsNullOrEmpty(v_MouseClick))
                {
                    // because default value is different from Click Mouse
                    var ignoreWaitTime = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_IgnoreWaitTime), engine);

                    var clickCommand = new ClickMouseCommand
                    {
                        v_MouseClick = v_MouseClick,
                        v_WaitTimeAfterClick = v_WaitTimeAfterClick,
                        v_IgnoreWaitTime = ignoreWaitTime,
                    };
                    clickCommand.RunCommand(engine);
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
                    //((TextBox)ControlsList[nameof(v_XMousePosition)]).Text = v_XMousePosition;
                    //((TextBox)ControlsList[nameof(v_YMousePosition)]).Text = v_YMousePosition;
                    ControlsList.GetPropertyControl<TextBox>(nameof(v_XMousePosition)).Text = v_XMousePosition;
                    ControlsList.GetPropertyControl<TextBox>(nameof(v_YMousePosition)).Text = v_YMousePosition;
                }
            }
        }
    }
}