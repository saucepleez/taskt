using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("Simulates mouse clicks.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to simulate multiple types of mouse clicks.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'SetCursorPos' function from user32.dll to achieve automation.")]
    public class SendMouseClickCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate mouse click type")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Double Left Click")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate the type of click required")]
        [Attributes.PropertyAttributes.SampleUsage("Select from **Left Click**, **Middle Click**, **Right Click**, **Double Left Click**, **Left Down**, **Middle Down**, **Right Down**, **Left Up**, **Middle Up**, **Right Up** ")]
        [Attributes.PropertyAttributes.Remarks("You can simulate custom click by using multiple mouse click commands in succession, adding **Pause Command** in between where required.")]
        public string v_MouseClick { get; set; }

        public SendMouseClickCommand()
        {
            this.CommandName = "SendMouseClickCommand";
            this.SelectionName = "Send Mouse Click";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var mousePosition = System.Windows.Forms.Cursor.Position;
            User32Functions.SendMouseClick(v_MouseClick, mousePosition.X, mousePosition.Y);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create window name helper control
            RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateDefaultDropdownGroupFor("v_MouseClick", this, editor));
       

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Click Type: " + v_MouseClick + "]";
        }
    }
}