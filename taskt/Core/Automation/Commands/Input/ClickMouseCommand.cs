using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.CommandSettings("Click Mouse")]
    [Attributes.ClassAttributes.Description("Simulates mouse clicks.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to simulate multiple types of mouse clicks.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'SetCursorPos' function from user32.dll to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ClickMouseCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please indicate mouse click type")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyUISelectionOption("Left Click")]
        //[PropertyUISelectionOption("Middle Click")]
        //[PropertyUISelectionOption("Right Click")]
        //[PropertyUISelectionOption("Left Down")]
        //[PropertyUISelectionOption("Middle Down")]
        //[PropertyUISelectionOption("Right Down")]
        //[PropertyUISelectionOption("Left Up")]
        //[PropertyUISelectionOption("Middle Up")]
        //[PropertyUISelectionOption("Right Up")]
        //[PropertyUISelectionOption("Double Left Click")]
        //[InputSpecification("Indicate the type of click required")]
        //[SampleUsage("Select from **Left Click**, **Middle Click**, **Right Click**, **Double Left Click**, **Left Down**, **Middle Down**, **Right Down**, **Left Up**, **Middle Up**, **Right Up** ")]
        //[Remarks("You can simulate custom click by using multiple mouse click commands in succession, adding **Pause Command** in between where required.")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_MouseClickType))]
        public string v_MouseClick { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_WaitTimeAfterMouseClick))]
        public string v_WaitTimeAfterClick { get; set; }

        public ClickMouseCommand()
        {
            //this.CommandName = "SendMouseClickCommand";
            //this.SelectionName = "Send Mouse Click";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var clickType = this.GetUISelectionValue(nameof(v_MouseClick), engine);

            var mousePosition = Cursor.Position;
            User32Functions.SendMouseClick(clickType, mousePosition.X, mousePosition.Y);

            var waitTime = this.ConvertToUserVariableAsInteger(nameof(v_WaitTimeAfterClick), engine);
            System.Threading.Thread.Sleep(waitTime);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create window name helper control
        //    RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateDefaultDropdownGroupFor("v_MouseClick", this, editor));
       
        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Click Type: " + v_MouseClick + "]";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_MouseClick))
        //    {
        //        this.validationResult += "Mouse click type is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}