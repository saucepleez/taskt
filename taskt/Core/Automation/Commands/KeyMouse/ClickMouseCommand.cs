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
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_input))]
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

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Ignore Wait Time When Click Type is 'None'")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        public string v_IgnoreWaitTime { get; set; }

        public ClickMouseCommand()
        {
            //this.CommandName = "SendMouseClickCommand";
            //this.SelectionName = "Send Mouse Click";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var clickType = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_MouseClick), engine);

            var mousePosition = Cursor.Position;
            KeyMouseControls.SendMouseClick(clickType, mousePosition.X, mousePosition.Y);

            var isIgnore = this.ExpandValueOrUserVariableAsYesNo(nameof(v_IgnoreWaitTime), engine);

            if ((clickType != "none") && isIgnore)
            {
                var waitTime = this.ExpandValueOrUserVariableAsInteger(nameof(v_WaitTimeAfterClick), engine);
                System.Threading.Thread.Sleep(waitTime);
            }
        }
    }
}