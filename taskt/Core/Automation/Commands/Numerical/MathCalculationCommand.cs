using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Numerical Commands")]
    [Attributes.ClassAttributes.CommandSettings("Math Calculation")]
    [Attributes.ClassAttributes.Description("This command allows you to perform a math calculation and apply it to a variable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to perform a math calculation.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MathCalculationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Formula to be Computed")]
        [InputSpecification("Formula to be Computed", true)]
        //[SampleUsage("**2+1** or **{{{vNum}}}+1**")]
        [PropertyDetailSampleUsage("**2+1**", PropertyDetailSampleUsage.ValueType.Value, "Formula")]
        [PropertyDetailSampleUsage("**{{{vNum}}}+1**", "Add **1** to the value of Variable **vNum**")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Compute", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Compute")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Thousand Seperator")]
        [InputSpecification("Thousand Separator", true)]
        [Remarks("Typically a comma or a decimal point (period)")]
        [PropertyDetailSampleUsage("**,**", "Specify **comma** for Thousand Separator")]
        [PropertyDetailSampleUsage("**.**", "Specify **period** for Thousand Separator")]
        [PropertyDetailSampleUsage("**{{{vSeparator}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Thousand Separator")]
        [PropertyIsOptional(true)]
        [PropertyFirstValue("")]
        public string v_ThousandSeperator { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Decimal Seperator")]
        [InputSpecification("Decimal Separator", true)]
        [Remarks("Typically a comma or a decimal point (period)")]
        [SampleUsage("**.** or **,** or **{{{vSeperator}}}**")]
        [PropertyDetailSampleUsage("**,**", "Specify **comma** for Decimal Seperator")]
        [PropertyDetailSampleUsage("**.**", "Specify **period** for Decimal Seperator")]
        [PropertyDetailSampleUsage("**{{{vSeparator}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Decimal Seperator")]
        [PropertyIsOptional(true, ".")]
        [PropertyFirstValue(".")]
        public string v_DecimalSeperator { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public MathCalculationCommand()
        {
            //this.CommandName = "MathCalculationCommand";
            //this.SelectionName = "Math Calculation";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
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