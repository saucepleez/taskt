using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Key/Mouse Commands")]
    [Attributes.ClassAttributes.SubGruop("Mouse")]
    [Attributes.ClassAttributes.CommandSettings("Click Mouse")]
    [Attributes.ClassAttributes.Description("Simulates mouse clicks.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to simulate multiple types of mouse clicks.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'SetCursorPos' function from user32.dll to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ClickMouseCommand : ScriptCommand
    {
        [XmlAttribute]
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
            //User32Functions.SendMouseClick(clickType, mousePosition.X, mousePosition.Y);
            KeyMouseControls.SendMouseClick(clickType, mousePosition.X, mousePosition.Y);

            var waitTime = this.ConvertToUserVariableAsInteger(nameof(v_WaitTimeAfterClick), engine);
            System.Threading.Thread.Sleep(waitTime);
        }
    }
}