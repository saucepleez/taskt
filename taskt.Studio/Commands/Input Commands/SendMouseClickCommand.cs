using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Infrastructure;
using taskt.Core.User32;

namespace taskt.Commands
{

    [Serializable]
    [Group("Input Commands")]
    [Description("Simulates mouse clicks.")]
    [UsesDescription("Use this command to simulate multiple types of mouse clicks.")]
    [ImplementationDescription("This command implements 'SetCursorPos' function from user32.dll to achieve automation.")]
    public class SendMouseClickCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate mouse click type")]
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

        public SendMouseClickCommand()
        {
            CommandName = "SendMouseClickCommand";
            SelectionName = "Send Mouse Click";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var mousePosition = System.Windows.Forms.Cursor.Position;
            User32Functions.SendMouseClick(v_MouseClick, mousePosition.X, mousePosition.Y);
        }
        public override List<Control> Render(IfrmCommandEditor editor)
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