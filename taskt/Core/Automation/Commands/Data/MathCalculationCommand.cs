using System;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using System.Data;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.Description("This command allows you to perform a math calculation and apply it to a variable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to perform a math calculation.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class MathCalculationCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please supply the input to be computed")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify either text or a variable that contains valid math.")]
        [Attributes.PropertyAttributes.SampleUsage("**2+1** or **{{{vNum}}}+1**")]
        [Attributes.PropertyAttributes.Remarks("You can use known numbers or variables.")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate Thousand Seperator")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the seperator used to identify decimal places")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("Typically a comma or a decimal point (period)")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        public string v_ThousandSeperator { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Indicate Decimal Seperator")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the seperator used to identify decimal places")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("Typically a comma or a decimal point (period)")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        public string v_DecimalSeperator { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the math calculation")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        public string v_applyToVariableName { get; set; }

        public MathCalculationCommand()
        {
            this.CommandName = "MathCalculationCommand";
            this.SelectionName = "Math Calculation";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_InputValue = "(2 + 5) * 3";
            this.v_DecimalSeperator = ".";
            this.v_ThousandSeperator = ",";
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
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ThousandSeperator", this, editor));
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DecimalSeperator", this, editor));

            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            //RenderedControls.Add(VariableNameControl);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Apply Calculation Result to '{v_applyToVariableName}']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InputValue))
            {
                this.validationResult += "Compute is empty.\n";
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