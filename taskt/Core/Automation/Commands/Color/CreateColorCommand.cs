using System;
using System.Xml.Serialization;
using System.Drawing;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Color Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.Description("This command allows you to create Color.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create Color.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateColorCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a Color Variable Name")]
        [InputSpecification("")]
        [SampleUsage("**vColor** or **{{{vColor}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Color, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable")]
        public string v_Color { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Red Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**0** or *255** or **{{{vRed}}}**")]
        [Remarks("Values range from 0 to 255")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Red", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.NotBetween)]
        [PropertyValueRange(0, 255)]
        [PropertyDisplayText(true, "Red")]
        public string v_Red { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Green Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**0** or *255** or **{{{vGreen}}}**")]
        [Remarks("Values range from 0 to 255")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Green", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.NotBetween)]
        [PropertyValueRange(0, 255)]
        [PropertyDisplayText(true, "Green")]
        public string v_Green { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Blue Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**0** or *255** or **{{{vBlue}}}**")]
        [Remarks("Values range from 0 to 255")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Blue", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.NotBetween)]
        [PropertyValueRange(0, 255)]
        [PropertyDisplayText(true, "Blue")]
        public string v_Blue { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Alpha Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**0** or *255** or **{{{Alpha}}}**")]
        [Remarks("Values range from 0 to 255")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyIsOptional(true, "255")]
        [PropertyDisplayText(true, "Alpha")]
        public string v_Alpha { get; set; }

        public CreateColorCommand()
        {
            this.CommandName = "CreateColorCommand";
            this.SelectionName = "Create Color";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            //int r = v_Red.ConvertToUserVariableAsInteger("Red", engine);
            //int g = v_Green.ConvertToUserVariableAsInteger("Green", engine);
            //int b = v_Blue.ConvertToUserVariableAsInteger("Blue", engine);
            int r = this.ConvertToUserVariableAsInteger(nameof(v_Red), "Red", engine);
            int g = this.ConvertToUserVariableAsInteger(nameof(v_Green), "Green", engine);
            int b = this.ConvertToUserVariableAsInteger(nameof(v_Blue), "Blue", engine);

            //if (string.IsNullOrEmpty(v_Alpha))
            //{
            //    v_Alpha = "255";
            //}
            //int a = v_Alpha.ConvertToUserVariableAsInteger("Alpha", engine);
            int a = this.ConvertToUserVariableAsInteger(nameof(v_Alpha), "Alpha", engine);

            Color co = Color.FromArgb(a, r, g, b);
            co.StoreInUserVariable(engine, v_Color);
        }
    }
}