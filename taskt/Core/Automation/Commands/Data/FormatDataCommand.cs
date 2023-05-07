using System;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.Forms;
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

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            //get variablized string
            //var variableString = v_InputValue.ConvertToUserVariable(sender);

            //get formatting
            //var formatting = v_ToStringFormat.ConvertToUserVariable(sender);

            //var variableName = v_applyToVariableName.ConvertToUserVariable(sender);

            //string formattedString = "";
            switch (v_FormatType)
            {
                case "Date":
                    //if (DateTime.TryParse(variableString, out var parsedDate))
                    //{
                    //    formattedString = parsedDate.ToString(formatting);
                    //}

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
                    //if (Decimal.TryParse(variableString, out var parsedDecimal))
                    //{
                    //    formattedString = parsedDecimal.ToString(formatting);
                    //}

                    var formatNumber = new FormatNumberCommand()
                    {
                        v_Number = v_InputValue,
                        v_Format = v_ToStringFormat,
                        v_Result = v_applyToVariableName
                    };
                    formatNumber.RunCommand(engine);
                    break;
                case "Path":
                    //switch(formatting.ToLower())
                    //{
                    //    case "file":
                    //    case "filename":
                    //    case "fn":
                    //        formattedString = Path.GetFileName(variableString);
                    //        break;

                    //    case "folder":
                    //    case "directory":
                    //    case "dir":
                    //        formattedString = Path.GetDirectoryName(variableString);
                    //        break;

                    //    case "filewithoutextension":
                    //    case "filenamewithoutextension":
                    //    case "fnwoext":
                    //        formattedString = Path.GetFileNameWithoutExtension(variableString);
                    //        break;

                    //    case "extension":
                    //    case "ext":
                    //        formattedString = Path.GetExtension(variableString);
                    //        break;

                    //    case "drive":
                    //    case "drivename":
                    //    case "root":
                    //        formattedString = Path.GetPathRoot(variableString);
                    //        break;

                    //    default:
                    //        formattedString = "";
                    //        break;
                    //}
                    var variableString = v_InputValue.ConvertToUserVariable(engine);
                    var formatting = v_ToStringFormat.ConvertToUserVariable(engine);
                    var formattedString = FilePathControls.FormatFileFolderPath(variableString, formatting);
                    formattedString.StoreInUserVariable(engine, v_applyToVariableName);
                    break;
                    
                default:
                    throw new Exception("Formatter Type Not Supported: " + v_FormatType);
            }

            //if (formattedString == "")
            //{
            //    throw new InvalidDataException("Unable to convert '" + variableString + "' to type '" + v_FormatType + "'");
            //}
            //else
            //{
            //    formattedString.StoreInUserVariable(sender, v_applyToVariableName);
            //}
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

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create standard group controls
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));

        //    ////RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_FormatType", this));
        //    ////RenderedControls.Add(CommandControls.CreateDropdownFor("v_FormatType", this));
        //    //var typeCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_FormatType", this, editor);
        //    //RenderedControls.AddRange(typeCtrls);

        //    //var formatCtls = CommandControls.CreateDefaultInputGroupFor("v_ToStringFormat", this, editor);
        //    //RenderedControls.AddRange(formatCtls);

        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
        //    //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
        //    //RenderedControls.Add(VariableNameControl);

        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

        //    //if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    //{
        //    //    this.v_InputValue = CommandControls.replaceApplicationKeyword("{{{DateTime.Now}}}");
        //    //}

        //    return RenderedControls;

        //}
        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Format '" + v_InputValue + "' and Apply Result to Variable '" + v_applyToVariableName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    this.IsValid = true;
        //    this.validationResult = "";

        //    if (String.IsNullOrEmpty(this.v_InputValue))
        //    {
        //        this.validationResult += "Value is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_FormatType))
        //    {
        //        this.validationResult += "Type of data is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_FormatType))
        //    {
        //        this.validationResult += "Output format is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_applyToVariableName))
        //    {
        //        this.validationResult += "Variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}