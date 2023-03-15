using System;
using System.Xml.Serialization;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Numerical Commands")]
    [Attributes.ClassAttributes.CommandSettings("Format Number")]
    [Attributes.ClassAttributes.Description("This command allows you to Format Number.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Format Number.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class FormatNumberCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please specify Number to Format")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**123** or **{{{vNumber}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Number", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Number")]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        public string v_Number { get; set; }

        [XmlAttribute]
        [PropertyDescription("Number Format")]
        [InputSpecification("Number Format", true)]
        [Remarks("For more detailed information, please refer to this URL.\nhttps://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings\nhttps://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings")]
        [PropertyCustomUIHelper("Format Checker", nameof(lnkFormatChecker_Click))]
        //[SampleUsage("**0.#** or **C** or **{{{vFormat}}}**")]
        [PropertyDetailSampleUsage("**0.#**", PropertyDetailSampleUsage.ValueType.Value, "Format")]
        [PropertyDetailSampleUsage("**C**", PropertyDetailSampleUsage.ValueType.Value, "Format")]
        [PropertyDetailSampleUsage("**{{{vFormat}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Format")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Format", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Format")]
        public string v_Format { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify Variable Name to store Result")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**vResult** or **{{{vResult}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyValidationRule("Result Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Store")]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        public FormatNumberCommand()
        {
            //this.CommandName = "FormatNumberCommand";
            //this.SelectionName = "Format Number";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //decimal value = new PropertyConvertTag(v_Number, "Number").ConvertToUserVariableAsDecimal(engine);
            //var value = this.ConvertToUserVariableAsDecimal(nameof(v_Number), "Number", engine);
            var value = this.ConvertToUserVariableAsDecimal(nameof(v_Number), engine);

            string format = v_Format.ConvertToUserVariable(engine);

            value.ToString(format).StoreInUserVariable(engine, v_Result);
        }

        private void lnkFormatChecker_Click(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)((CommandItemControl)sender).Tag;
            UI.Forms.Supplement_Forms.frmFormatChecker.ShowFormatCheckerFormLinkClicked(txt, "Number");
        }
    }
}