using System;
using System.Xml.Serialization;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DateTime Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.Description("This command allows you to Format DateTime Text.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Format DateTime Text.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class FormatDateTimeCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a DateTime Variable Name")]
        [InputSpecification("")]
        [SampleUsage("**{{{vDateTime}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DateTime, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("DateTime Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable")]
        public string v_DateTime { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify DateTime Format")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyCustomUIHelper("Format Checker", "lnkFormatChecker_Click")]
        [InputSpecification("")]
        [SampleUsage("**MM/dd/yyyy** or **HH:mm:ss** or **{{{vFormat}}}**")]
        [Remarks("Please refer to the Microsoft DateTime.ToString() page for format details")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Format", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Format")]
        public string v_Format { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Variable Name to store Result")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vResult** or **{{{vResult}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Result Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_Result { get; set; }

        public FormatDateTimeCommand()
        {
            this.CommandName = "FormatDateTimeCommand";
            this.SelectionName = "Format DateTime";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            var myDT = v_DateTime.GetDateTimeVariable(engine);
            string format = v_Format.ConvertToUserVariable(engine);

            myDT.ToString(format).StoreInUserVariable(engine, v_Result);
        }

        private void lnkFormatChecker_Click(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)((CommandItemControl)sender).Tag;
            UI.Forms.Supplement_Forms.frmFormatChecker.ShowFormatCheckerFormLinkClicked(txt, "DateTime");
        }
    }
}