using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Format DateTime commands
    /// </summary>
    public abstract class AFormatDateTimeCommands : ADateTimeConvertCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_InputDateTime))]
        //public string v_DateTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_Format))]
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
        [PropertyParameterOrder(6000)]
        public virtual string v_Format { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //public string v_Result { get; set; }

        //protected void lnkFormatChecker_Click(object sender, EventArgs e)
        //{
        //    TextBox txt = (TextBox)((CommandItemControl)sender).Tag;
        //    UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmFormatChecker.ShowFormatCheckerFormLinkClicked(txt, "DateTime");
        //}

        protected void CommandProcess(ADateTimeCreateCommands create, Engine.AutomationEngineInstance engine)
        {
            using (var v = new InnerScriptVariable(engine))
            {
                create.v_DateTime = v.VariableName;
                create.RunCommand(engine);

                var fdt = new FormatDateTimeCommand()
                {
                    v_DateTime = v.VariableName,
                    v_Format = this.v_Format,
                    v_Result = this.v_Result,
                };
                fdt.RunCommand(engine);
            }
        }
    }
}