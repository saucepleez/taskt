using System;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.Description("This command allows you to apply formatting to a string")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to apply specific formatting to text or a variable")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class FormatDataCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please supply the value or variable.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify either text or a variable that contains a date or number requiring formatting")]
        [Attributes.PropertyAttributes.SampleUsage("**{{{DateTime.Now}}}** or **1/1/2000** or **2500** or **{{{vNum}}}** or **C:\\temp\\myfile.txt**")]
        [Attributes.PropertyAttributes.Remarks("You can use known text or variables.")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the type of data")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Date")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Number")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Path")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate the source type")]
        [Attributes.PropertyAttributes.SampleUsage("Choose **Date** or **Number** or **Path**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_FormatType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Specify required output format")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify if a specific string format is required.")]
        [Attributes.PropertyAttributes.SampleUsage("**MM/dd/yy** or **hh:mm** or **#.0** or **file** etc.")]
        [Attributes.PropertyAttributes.Remarks("Path supports **file**, **folder**, **filewithoutextension**, **extension**, **drive**")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_ToStringFormat { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive output")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        public string v_applyToVariableName { get; set; }


        public FormatDataCommand()
        {
            this.CommandName = "FormatDataCommand";
            this.SelectionName = "Format Data";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_InputValue = "";
            this.v_FormatType = "Date";
            this.v_ToStringFormat = "MM/dd/yyyy";
            
        }

        public override void RunCommand(object sender)
        {
            //get variablized string
            var variableString = v_InputValue.ConvertToUserVariable(sender);

            //get formatting
            var formatting = v_ToStringFormat.ConvertToUserVariable(sender);

            //var variableName = v_applyToVariableName.ConvertToUserVariable(sender);


            string formattedString = "";
            switch (v_FormatType)
            {
                case "Date":
                    if (DateTime.TryParse(variableString, out var parsedDate))
                    {
                        formattedString = parsedDate.ToString(formatting);
                    }
                    break;
                case "Number":
                    if (Decimal.TryParse(variableString, out var parsedDecimal))
                    {
                        formattedString = parsedDecimal.ToString(formatting);
                    }
                    break;
                case "Path":
                    switch(formatting.ToLower())
                    {
                        case "file":
                        case "filename":
                        case "fn":
                            formattedString = Path.GetFileName(variableString);
                            break;

                        case "folder":
                        case "directory":
                        case "dir":
                            formattedString = Path.GetDirectoryName(variableString);
                            break;

                        case "filewithoutextension":
                        case "filenamewithoutextension":
                        case "fnwoext":
                            formattedString = Path.GetFileNameWithoutExtension(variableString);
                            break;

                        case "extension":
                        case "ext":
                            formattedString = Path.GetExtension(variableString);
                            break;

                        case "drive":
                        case "drivename":
                        case "root":
                            formattedString = Path.GetPathRoot(variableString);
                            break;

                        default:
                            formattedString = "";
                            break;
                    }
                    break;
                    
                default:
                    throw new Exception("Formatter Type Not Supported: " + v_FormatType);
            }

            if (formattedString == "")
            {
                throw new InvalidDataException("Unable to convert '" + variableString + "' to type '" + v_FormatType + "'");
            }
            else
            {
                formattedString.StoreInUserVariable(sender, v_applyToVariableName);
            }



        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));

            ////RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_FormatType", this));
            ////RenderedControls.Add(CommandControls.CreateDropdownFor("v_FormatType", this));
            //var typeCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_FormatType", this, editor);
            //RenderedControls.AddRange(typeCtrls);

            //var formatCtls = CommandControls.CreateDefaultInputGroupFor("v_ToStringFormat", this, editor);
            //RenderedControls.AddRange(formatCtls);

            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            //RenderedControls.Add(VariableNameControl);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            if (editor.creationMode == frmCommandEditor.CreationMode.Add)
            {
                this.v_InputValue = editor.ReplaceVariableMaker("{{{DateTime.Now}}}");
            }

            return RenderedControls;

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Format '" + v_InputValue + "' and Apply Result to Variable '" + v_applyToVariableName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            this.IsValid = true;
            this.validationResult = "";

            if (String.IsNullOrEmpty(this.v_InputValue))
            {
                this.validationResult += "Value is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_FormatType))
            {
                this.validationResult += "Type of data is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_FormatType))
            {
                this.validationResult += "Output format is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_applyToVariableName))
            {
                this.validationResult += "Variable is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}