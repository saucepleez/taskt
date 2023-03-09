using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Variable Commands")]
    [Attributes.ClassAttributes.CommandSettings("Set Variable Index")]
    [Attributes.ClassAttributes.Description("This command allows you to modify variables.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to modify the value of variables.  You can even use variables to modify other variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetVariableIndexCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VariableNameControls), nameof(VariableNameControls.v_VariableName))]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Both)]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Index")]
        [InputSpecification("Index", true)]
        [PropertyDetailSampleUsage("**0**", "Specify the First Index")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Index")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Index")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("Index", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Index")]
        public string v_Index { get; set; }

        public SetVariableIndexCommand()
        {
            //this.CommandName = "SetVariableIndexCommand";
            //this.SelectionName = "Set Variable Index";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;
            
            var variableName = VariableNameControls.GetVariableName(v_userVariableName, engine);
            var rawVariable = variableName.GetRawVariable(engine);

            var index = this.ConvertToUserVariableAsInteger(nameof(v_Index), engine);
            rawVariable.CurrentPosition = index;
        }
    }
}