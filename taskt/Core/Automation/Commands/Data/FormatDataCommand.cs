using System;
using System.Xml.Serialization;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to apply formatting to a string")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to apply specific formatting to text or a variable")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class FormatDataCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please supply the value or variable.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Specify either text or a variable that contains a date or number requiring formatting")]
        [SampleUsage("**{{{DateTime.Now}}}** or **1/1/2000** or **2500** or **{{{vNum}}}** or **C:\\temp\\myfile.txt**")]
        [Remarks("You can use known text or variables.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Value", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Value")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the type of data")]
        [PropertyUISelectionOption("Date")]
        [PropertyUISelectionOption("Number")]
        [PropertyUISelectionOption("Path")]
        [InputSpecification("Indicate the source type")]
        [SampleUsage("Choose **Date** or **Number** or **Path**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyDisplayText(true, "Type")]
        public string v_FormatType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Specify required output format")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyCustomUIHelper("Format Checker", "lnkFormatChecker_Click")]
        [InputSpecification("Specify if a specific string format is required.")]
        [SampleUsage("**MM/dd/yy** or **hh:mm** or **#.0** or **file** etc.")]
        [Remarks("Path supports **file**, **folder**, **filewithoutextension**, **extension**, **drive**")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Format", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Format")]
        public string v_ToStringFormat { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the variable to receive output")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable** or **{{{vSomeVariable}}}**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Result")]
        public string v_applyToVariableName { get; set; }


        public FormatDataCommand()
        {
            this.CommandName = "FormatDataCommand";
            this.SelectionName = "Format Data";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            //this.v_InputValue = "";
            //this.v_FormatType = "Date";
            //this.v_ToStringFormat = "MM/dd/yyyy";   
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            switch (v_FormatType)
            {
                case "Date":
                    var inner0 = VariableNameControls.GetInnerVariableName(0, engine);

                    var dateTimeFromText = new CreateDateTimeFromTextCommand()
                    {
                        v_Text = v_InputValue,
                        v_DateTime = inner0
                    };
                    dateTimeFromText.RunCommand(engine);
                    var formatDateTime = new FormatDateTimeCommand()
                    {
                        v_DateTime = inner0,
                        v_Format = v_ToStringFormat,
                        v_Result = v_applyToVariableName
                    };
                    formatDateTime.RunCommand(engine);
                    break;

                case "Number":
                    var formatNumber = new FormatNumberCommand()
                    {
                        v_Number = v_InputValue,
                        v_Format = v_ToStringFormat,
                        v_Result = v_applyToVariableName
                    };
                    formatNumber.RunCommand(engine);
                    break;

                case "Path":
                    var variableString = v_InputValue.ExpandValueOrUserVariable(engine);
                    var formatting = v_ToStringFormat.ExpandValueOrUserVariable(engine);
                    var formattedString = FilePathControls.FormatFileFolderPath(variableString, formatting);
                    formattedString.StoreInUserVariable(engine, v_applyToVariableName);
                    break;
                    
                default:
                    throw new Exception("Formatter Type Not Supported: " + v_FormatType);
            }
        }

        private void lnkFormatChecker_Click(object sender, EventArgs e)
        {
            //using (var fm = new UI.Forms.Supplement_Forms.frmFormatChecker())
            //{
            //    if (fm.ShowDialog() == DialogResult.OK)
            //    {
            //        ((TextBox)((CommandItemControl)sender).Tag).Text = fm.Format;
            //    }
            //}
            TextBox txt = (TextBox)((CommandItemControl)sender).Tag;
            UI.Forms.Supplement_Forms.frmFormatChecker.ShowFormatCheckerFormLinkClicked(txt);
        }
    }
}