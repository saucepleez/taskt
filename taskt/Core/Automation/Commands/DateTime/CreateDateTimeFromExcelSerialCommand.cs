using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DateTime Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.Description("This command allows you to create DateTime from Excel Serial Value.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create DateTime from Excel Serial Value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateDateTimeFromExcelSerialCommand : ScriptCommand
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
        [PropertyDescription("Please specify Excel Serial Value to Create DateTime")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**43210** or **{{{vSerial}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Serial", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Serial")]
        public string v_Serial { get; set; }

        public CreateDateTimeFromExcelSerialCommand()
        {
            this.CommandName = "CreateDateTimeFromExcelSerialCommand";
            this.SelectionName = "Create DateTime From Excel Serial";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            //decimal value = new PropertyConvertTag(v_Serial, "Serial").ConvertToUserVariableAsDecimal(engine);
            decimal value = this.ConvertToUserVariableAsDecimal(nameof(v_Serial), "Serial", engine);

            try
            {
                DateTime myDT = DateTime.FromOADate((double)value);
                myDT.StoreInUserVariable(engine, v_DateTime);
            }
            catch (Exception)
            {
                throw new Exception("Serial '" + v_Serial + "' is not Excel Serial");
            }
        }
    }
}