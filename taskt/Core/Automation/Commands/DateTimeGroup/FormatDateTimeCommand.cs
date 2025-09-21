using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DateTime")]
    [Attributes.ClassAttributes.SubGruop("Format")]
    [Attributes.ClassAttributes.CommandSettings("Format DateTime")]
    [Attributes.ClassAttributes.Description("This command allows you to Format DateTime.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Format DateTime.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class FormatDateTimeCommand : AFormatDateTimeCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_InputDateTime))]
        //public string v_DateTime { get; set; }

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

        public FormatDateTimeCommand()
        {
            //this.CommandName = "FormatDateTimeCommand";
            //this.SelectionName = "Format DateTime";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var myDT = this.ExpandValueOrVariableAsDateTime(engine);
            
            string format = v_Format.ExpandValueOrUserVariable(engine);

            myDT.ToString(format).StoreInUserVariable(engine, v_Result);
        }
    }
}