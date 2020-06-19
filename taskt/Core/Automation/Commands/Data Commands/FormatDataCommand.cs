using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Data Commands")]
    [Description("This command converts text (in either date or number format) to a specified format and saves the result in a variable.")]
    public class FormatDataCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Input Data")]
        [InputSpecification("Specify either text or a variable that contains a date or number requiring formatting.")]
        [SampleUsage("1/1/2000 || 2500 || {DateTime.Now} || {vNumber}")]
        [Remarks("You can use known text or variables.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputData { get; set; }

        [XmlAttribute]
        [PropertyDescription("Input Data Type")]
        [PropertyUISelectionOption("Date")]
        [PropertyUISelectionOption("Number")]
        [InputSpecification("Select the type of input data.")]
        [SampleUsage("")]
        [Remarks("Select 'Date' if the input data is a Date or 'Number' if it is a Number. Input data of other types will result in an error.")]
        public string v_FormatType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Data Format")]
        [InputSpecification("Specify the output data format.")]
        [SampleUsage("MM/dd/yy, hh:mm:ss || C2 || D2 || {vDataFormat}")]
        [Remarks("You should specify a valid input data format; invalid formats will result in an error. 'C2' and 'D2' are Number Formats.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ToStringFormat { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Text Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                  " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public FormatDataCommand()
        {
            CommandName = "FormatDataCommand";
            SelectionName = "Format Data";
            CommandEnabled = true;
            CustomRendering = true;

            v_InputData = "{DateTime.Now}";
            v_FormatType = "Date";
            v_ToStringFormat = "MM/dd/yyyy";
        }

        public override void RunCommand(object sender)
        {
            //get variablized string
            var variableString = v_InputData.ConvertToUserVariable(sender);

            //get formatting
            var formatting = v_ToStringFormat.ConvertToUserVariable(sender);

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
                default:
                    throw new Exception("Formatter Type Not Supported: " + v_FormatType);
            }

            if (formattedString == "")
            {
                throw new InvalidDataException("Unable to convert '" + variableString + "' to type '" + v_FormatType + "'");
            }
            else
            {
                formattedString.StoreInUserVariable(sender, v_OutputUserVariableName);
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputData", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_FormatType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ToStringFormat", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Format {v_FormatType} '{v_InputData}' as '{v_ToStringFormat}' - Store Text in '{v_OutputUserVariableName}']";
        }
    }
}