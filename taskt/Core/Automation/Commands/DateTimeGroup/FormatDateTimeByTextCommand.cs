using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DateTime")]
    [Attributes.ClassAttributes.SubGruop("Format")]
    [Attributes.ClassAttributes.CommandSettings("Format DateTime By Text")]
    [Attributes.ClassAttributes.Description("This command allows you to Format DateTime By Text.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Format DateTime By Text.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class FormatDateTimeByTextCommand : AFormatDateTimeCommands
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("DateTime Text")]
        //[InputSpecification("")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyDetailSampleUsage("**2020-15-1**", PropertyDetailSampleUsage.ValueType.Value, "DateTime")]
        //[PropertyDetailSampleUsage("**{{{vDateTime}}}**", PropertyDetailSampleUsage.ValueType.VariableName, "DateTime")]
        //[PropertyParameterOrder(5000)]
        //[PropertyValidationRule("DateTime", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "DateTime")]
        [PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_DateTimeText))]
        public override string v_DateTime { get; set; }

        //[XmlAttribute]
        //[PropertyDescription("DateTime Format")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyCustomUIHelper("Format Checker", nameof(lnkFormatChecker_Click))]
        //[InputSpecification("")]
        //[PropertyDetailSampleUsage("**MM/dd/yyyy**", "Specify Format Month/Day/Year")]
        //[PropertyDetailSampleUsage("**HH:mm:ss**", "Specify Format Hour/Minute/Second")]
        //[PropertyDetailSampleUsage("{{{vFormat}}}", PropertyDetailSampleUsage.ValueType.VariableValue, "Format")]
        //[Remarks("Please refer to the Microsoft DateTime.ToString() page for format details")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Format", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Format")]
        //[PropertyParameterOrder(6000)]
        //public string v_Format { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //public string v_Result { get; set; }

        public FormatDateTimeByTextCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //using (var v = new InnerScriptVariable(engine)) 
            //{
            //    var cdt = new CreateDateTimeFromTextCommand()
            //    {
            //        v_DateTime = v.VariableName,
            //        v_Text = this.v_DateTime,
            //    };
            //    cdt.RunCommand(engine);

            //    var fdt = new FormatDateTimeCommand()
            //    {
            //        v_DateTime = v.VariableName,
            //        v_Format = this.v_Format,
            //        v_Result = this.v_Result,
            //    };
            //    fdt.RunCommand(engine);
            //}

            var cdt = new CreateDateTimeFromTextCommand()
            {
                v_Text = this.v_DateTime,
            };
            this.CommandProcess(
                cdt,
                engine
            );
        }
    }
}