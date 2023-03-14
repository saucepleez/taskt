using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Create Text Variable")]
    [Attributes.ClassAttributes.Description("This command allows you to create text variables.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create text variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateTextVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextControls), nameof(TextControls.v_OutputTextVariableName))]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextControls), nameof(TextControls.v_Text_MultiLine))]
        public string v_Value { get; set; }

        public CreateTextVariableCommand()
        {
            //this.CommandName = "CreateTextVariable";
            //this.SelectionName = "Create Text Variable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            var valueToSet = v_Value.ConvertToUserVariable(engine);
            valueToSet.StoreInUserVariable(engine, v_userVariableName);
        }
    }
}