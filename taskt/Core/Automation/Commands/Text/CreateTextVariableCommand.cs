using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.Description("This command allows you to create text variables.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create text variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateTextVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a text variable name")]
        [InputSpecification("")]
        [SampleUsage("**vSomeVariable** or **{{{vSomeVariable}}}**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable")]

        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify Text Value to Set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**1** or **Hello** or {{{vNum}}}")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [PropertyDisplayText(true, "Text")]
        public string v_Value { get; set; }

        public CreateTextVariableCommand()
        {
            this.CommandName = "CreateTextVariable";
            this.SelectionName = "Create Text Variable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
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