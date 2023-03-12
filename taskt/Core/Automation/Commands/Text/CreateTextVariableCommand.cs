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
        [PropertyDescription("Text Variable Name")]
        [InputSpecification("Text Variable Name", true)]
        [PropertyDetailSampleUsage("**vText**", "Specify Variable Name **vText**")]
        [PropertyDetailSampleUsage("**{{{vText}}}**", "Specify Variable Name **vText**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable")]

        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Text Value")]
        [InputSpecification("Text Value", true)]
        [Remarks("")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Text Value")]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Text Value")]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text Value")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [PropertyDisplayText(true, "Text")]
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