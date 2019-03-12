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
    [Attributes.ClassAttributes.Description("This command allows you to build a date and apply it to a variable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to perform a date calculation.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class DateCalculationCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please supply the date value or variable (ex. [DateTime.Now]")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify either text or a variable that contains the start date.")]
        [Attributes.PropertyAttributes.SampleUsage("[DateTime.Now] or 1/1/2000")]
        [Attributes.PropertyAttributes.Remarks("You can use known text or variables.")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select a Calculation Method")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Add Seconds")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Add Minutes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Add Hours")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Add Days")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Add Years")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Subtract Seconds")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Subtract Minutes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Subtract Hours")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Subtract Days")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Subtract Years")]
        [Attributes.PropertyAttributes.InputSpecification("Select the necessary operation")]
        [Attributes.PropertyAttributes.SampleUsage("Select From Add Seconds, Add Minutes, Add Hours, Add Days, Add Years, Subtract Seconds, Subtract Minutes, Subtract Hours, Subtract Days, Subtract Years ")]
        public string v_CalculationMethod { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please supply the increment value")]
        [Attributes.PropertyAttributes.InputSpecification("Enter how many units to increment by")]
        [Attributes.PropertyAttributes.SampleUsage("15, [vIncrement]")]
        [Attributes.PropertyAttributes.Remarks("You can use negative numbers which will do the opposite, ex. Subtract Days and an increment of -5 will Add Days.")]
        public string v_Increment { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Specify String Format")]
        [Attributes.PropertyAttributes.InputSpecification("Specify if a specific string format is required.")]
        [Attributes.PropertyAttributes.SampleUsage("MM/dd/yy, hh:mm, etc.")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ToStringFormat { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the date calculation")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }

        public DateCalculationCommand()
        {
            this.CommandName = "DateCalculationCommand";
            this.SelectionName = "Date Calculation";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_InputValue = "{DateTime.Now}";
            this.v_ToStringFormat = "MM/dd/yyyy hh:mm:ss";

        }

        public override void RunCommand(object sender)
        {
            //get variablized string
            var variableDateTime = v_InputValue.ConvertToUserVariable(sender);

            //convert to date time
            DateTime requiredDateTime;
            if (!DateTime.TryParse(variableDateTime, out requiredDateTime))
            {
                throw new InvalidDataException("Date was unable to be parsed - " + variableDateTime);
            }

            //get increment value
            double requiredInterval;
            var variableIncrement = v_Increment.ConvertToUserVariable(sender);

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
            var formatting = v_ToStringFormat.ConvertToUserVariable(sender);
            var stringDateFormatted = requiredDateTime.ToString(formatting);


            //store string in variable
            stringDateFormatted.StoreInUserVariable(sender, v_applyToVariableName);

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_CalculationMethod", this));
            RenderedControls.Add(CommandControls.CreateDropdownFor("v_CalculationMethod", this));

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Increment", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ToStringFormat", this, editor));

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);


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
                    operandLanguage = " to ";
                }
                else
                {
                    operandLanguage = " from ";
                }

                //return value
                return base.GetDisplayValue() + " [" + operand + " " + v_Increment + " " + interval + operandLanguage + v_InputValue + ", Apply Result to Variable '" + v_applyToVariableName + "']";
            }
            else
            {
                return base.GetDisplayValue();
            }

        }
    }
}