using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.SubGruop("Clipboard")]
    [Attributes.ClassAttributes.CommandSettings("Set Clipboard Text")]
    [Attributes.ClassAttributes.Description("This command allows you to set text to the clipboard.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to copy the data from the clipboard and apply it to a variable.  You can then use the variable to extract the value.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against the VariableList from the scripting engine using System.Windows.Forms.Clipboard.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetClipboardTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_MultiLinesTextBox))]
        [PropertyDescription("Value to Set Clipboard")]
        [InputSpecification("Value", true)]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Value")]
        [PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Value")]
        public string v_InputValue { get; set; }

        public SetClipboardTextCommand()
        {
            //this.CommandName = "ClipboardSetTextCommand";
            //this.SelectionName = "Set Clipboard Text";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            var input = v_InputValue.ConvertToUserVariable(engine);
            //User32Functions.SetClipboardText(input);
            ClipboardControls.SetClipboardText(input);
        }
    }
}
