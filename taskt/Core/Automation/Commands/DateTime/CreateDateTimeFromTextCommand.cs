using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DateTime Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.Description("This command allows you to create DateTime from Text.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create DateTime from Text.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateDateTimeFromTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a DateTime Variable Name")]
        [InputSpecification("")]
        [SampleUsage("**vDateTime** or **{{{vDateTime}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DateTime, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable")]
        public string v_DateTime { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Text Value to Create DateTime")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**2000-01-01** or **{{{vText}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Text", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Text")]
        public string v_Text { get; set; }

        public CreateDateTimeFromTextCommand()
        {
            this.CommandName = "CreateDateTimeFromTextCommand";
            this.SelectionName = "Create DateTime From Text";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            string value = v_Text.ConvertToUserVariable(engine);

            if (DateTime.TryParse(value, out DateTime tryDT))
            {
                tryDT.StoreInUserVariable(engine, v_DateTime);
            }
            else
            {
                throw new Exception("Text '" + v_Text + "' is not DateTime");
            }
        }
    }
}