using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Numerical Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to perform a math calculation and apply it to a variable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to perform a math calculation.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MathCalculationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please supply the input to be computed")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Specify either text or a variable that contains valid math.")]
        [SampleUsage("**2+1** or **{{{vNum}}}+1**")]
        [Remarks("You can use known numbers or variables.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Compute", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Compute")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyDescription("Indicate Thousand Seperator")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the seperator used to identify decimal places")]
        [SampleUsage("**,** or **.** or **{{{vSeperator}}}**")]
        [Remarks("Typically a comma or a decimal point (period)")]
        [PropertyIsOptional(true)]
        [PropertyFirstValue("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyDisplayText(false, "")]
        public string v_ThousandSeperator { get; set; }

        [XmlAttribute]
        [PropertyDescription("Indicate Decimal Seperator")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the seperator used to identify decimal places")]
        [SampleUsage("**.** or **,** or **{{{vSeperator}}}**")]
        [Remarks("Typically a comma or a decimal point (period)")]
        [PropertyIsOptional(true, ".")]
        [PropertyFirstValue(".")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyDisplayText(false, "")]
        public string v_DecimalSeperator { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the variable to receive the math calculation")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Result")]
        public string v_applyToVariableName { get; set; }

        public MathCalculationCommand()
        {
            this.CommandName = "MathCalculationCommand";
            this.SelectionName = "Math Calculation";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get variablized string
            var variableMath = v_InputValue.ConvertToUserVariable(sender);

            try
            {
                var decimalSeperator = v_DecimalSeperator.ConvertToUserVariable(sender);
                var thousandSeperator = v_ThousandSeperator.ConvertToUserVariable(sender);

                //remove thousandths markers
                if (thousandSeperator != "")
                {
                    variableMath = variableMath.Replace(thousandSeperator, "");
                }

                if (decimalSeperator == "")
                {
                    decimalSeperator = ".";
                }

                //check decimal seperator
                if (decimalSeperator != ".")
                {
                    variableMath = variableMath.Replace(decimalSeperator, ".");
                }

                //perform compute
                DataTable dt = new DataTable();
                var result = dt.Compute(variableMath, "");

                //restore decimal seperator
                if (decimalSeperator != ".")
                {
                    result = result.ToString().Replace(".", decimalSeperator);
                }

               
                //store string in variable
                result.ToString().StoreInUserVariable(sender, v_applyToVariableName);
            }
            catch (Exception ex)
            {
                //throw exception is math calc failed
                throw ex;
            }
        }
    }
}