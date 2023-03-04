using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DateTime Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Create DateTime From Excel Serial")]
    [Attributes.ClassAttributes.Description("This command allows you to create DateTime from Excel Serial Value.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create DateTime from Excel Serial Value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateDateTimeFromExcelSerialCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_OutputDateTime))]
        public string v_DateTime { get; set; }

        [XmlAttribute]
        [PropertyDescription("Excel Serial Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**43210**", "Specify **43210** for Excel Serial. It's means 2018-04-20.")]
        [PropertyDetailSampleUsage("**{{{vSerial}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Excel Serial")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Serial", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Serial")]
        public string v_Serial { get; set; }

        public CreateDateTimeFromExcelSerialCommand()
        {
            //this.CommandName = "CreateDateTimeFromExcelSerialCommand";
            //this.SelectionName = "Create DateTime From Excel Serial";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            decimal value = this.ConvertToUserVariableAsDecimal(nameof(v_Serial), engine);

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