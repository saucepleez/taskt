using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Data Commands")]
    [Description("This command performs a specific operation on a date and saves the result in a variable.")]
    public class DateCalculationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Date")]
        [InputSpecification("Specify either text or a variable that contains the date.")]
        [SampleUsage("1/1/2000 || {DateTime.Now}")]
        [Remarks("You can use known text or variables.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputDate { get; set; }

        [XmlAttribute]
        [PropertyDescription("Calculation Method")]
        [PropertyUISelectionOption("Add Seconds")]
        [PropertyUISelectionOption("Add Minutes")]
        [PropertyUISelectionOption("Add Hours")]
        [PropertyUISelectionOption("Add Days")]
        [PropertyUISelectionOption("Add Years")]
        [PropertyUISelectionOption("Subtract Seconds")]
        [PropertyUISelectionOption("Subtract Minutes")]
        [PropertyUISelectionOption("Subtract Hours")]
        [PropertyUISelectionOption("Subtract Days")]
        [PropertyUISelectionOption("Subtract Years")]
        [InputSpecification("Select the date operation.")]
        [SampleUsage("")]
        [Remarks("The selected operation will be applied to the input date value and result will be stored in the output variable.")]
        public string v_CalculationMethod { get; set; }

        [XmlAttribute]
        [PropertyDescription("Increment Value")]
        [InputSpecification("Specify how many units to increment by.")]
        [SampleUsage("15 || {vIncrement}")]
        [Remarks("You can use negative numbers which will do the opposite, ex. Subtract Days and an increment of -5 will Add Days.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Increment { get; set; }

        [XmlAttribute]
        [PropertyDescription("Date Format (Optional)")]
        [InputSpecification("Specify the output date format.")]
        [SampleUsage("MM/dd/yy hh:mm:ss || MM/dd/yyyy || {vDateFormat}")]
        [Remarks("You can specify either a valid DateTime, Date or Time Format; an invalid format will result in an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ToStringFormat { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Date Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                  " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public DateCalculationCommand()
        {
            CommandName = "DateCalculationCommand";
            SelectionName = "Date Calculation";
            CommandEnabled = true;
            CustomRendering = true;

            v_InputDate = "{DateTime.Now}";
            v_ToStringFormat = "MM/dd/yyyy hh:mm:ss";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            //get variablized string
            var variableDateTime = v_InputDate.ConvertToUserVariable(engine);

            //convert to date time
            DateTime requiredDateTime;
            if (!DateTime.TryParse(variableDateTime, out requiredDateTime))
            {
                throw new InvalidDataException("Date was unable to be parsed - " + variableDateTime);
            }

            //get increment value
            double requiredInterval;
            var variableIncrement = v_Increment.ConvertToUserVariable(engine);

            //convert to double
            if (!Double.TryParse(variableIncrement, out requiredInterval))
            {
                throw new InvalidDataException("Date was unable to be parsed - " + variableIncrement);
            }

            //perform operation
            switch (v_CalculationMethod)
            {
                case "Add Seconds":
                    requiredDateTime = requiredDateTime.AddSeconds(requiredInterval);
                    break;
                case "Add Minutes":
                    requiredDateTime = requiredDateTime.AddMinutes(requiredInterval);
                    break;
                case "Add Hours":
                    requiredDateTime = requiredDateTime.AddHours(requiredInterval);
                    break;
                case "Add Days":
                    requiredDateTime = requiredDateTime.AddDays(requiredInterval);
                    break;
                case "Add Years":
                    requiredDateTime = requiredDateTime.AddYears((int)requiredInterval);
                    break;
                case "Subtract Seconds":
                    requiredDateTime = requiredDateTime.AddSeconds((requiredInterval * -1));
                    break;
                case "Subtract Minutes":
                    requiredDateTime = requiredDateTime.AddMinutes((requiredInterval * -1));
                    break;
                case "Subtract Hours":
                    requiredDateTime = requiredDateTime.AddHours((requiredInterval * -1));
                    break;
                case "Subtract Days":
                    requiredDateTime = requiredDateTime.AddDays((requiredInterval * -1));
                    break;
                case "Subtract Years":
                    requiredDateTime = requiredDateTime.AddYears(((int)requiredInterval * -1));
                    break;
                default:
                    break;
            }

            //handle if formatter is required
            var formatting = v_ToStringFormat.ConvertToUserVariable(engine);
            var stringDateFormatted = requiredDateTime.ToString(formatting);

            //store string (Result) in variable
            stringDateFormatted.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputDate", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_CalculationMethod", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Increment", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ToStringFormat", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            //if calculation method was selected
            if (v_CalculationMethod != null)
            {
                //determine operand and interval
                var operand = v_CalculationMethod.Split(' ')[0];
                var interval = v_CalculationMethod.Split(' ')[1];

                //additional language handling based on selection made
                string operandLanguage;
                if (operand == "Add")
                {
                    operandLanguage = "to";
                }
                else
                {
                    operandLanguage = "from";
                }

                //return value
                return base.GetDisplayValue() + $" [{operand} '{v_Increment}' {interval} {operandLanguage} '{v_InputDate}' - Store Date in '{v_OutputUserVariableName}']";
            }
            else
            {
                return base.GetDisplayValue();
            }

        }
    }
}