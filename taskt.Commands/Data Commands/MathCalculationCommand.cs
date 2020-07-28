using System;
using System.Collections.Generic;
using System.Data;
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
    [Description("This command performs a math calculation and saves the result in a variable.")]
    public class MathCalculationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Math Expression")]
        [InputSpecification("Specify either text or a variable that contains a valid math expression.")]
        [SampleUsage("(2 + 5) * 3 || ({vNumber1} + {vNumber2}) * {vNumber3}")]
        [Remarks("You can use known numbers or variables.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_MathExpression { get; set; }

        [XmlAttribute]
        [PropertyDescription("Thousand Separator (Optional)")]
        [InputSpecification("Specify the seperator used to identify decimal places.")]
        [SampleUsage(", || . || {vThousandSeparator}")]
        [Remarks("Typically a comma or a decimal point (period), like in 100,000, ',' is a thousand separator.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ThousandSeparator { get; set; }

        [XmlAttribute]
        [PropertyDescription("Decimal Separator (Optional)")]
        [InputSpecification("Specify the seperator used to identify decimal places.")]
        [SampleUsage(". || , || {vDecimalSeparator}")]
        [Remarks("Typically a comma or a decimal point (period), like in 60.99, '.' is a decimal separator.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DecimalSeparator { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Result Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                  " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public MathCalculationCommand()
        {
            CommandName = "MathCalculationCommand";
            SelectionName = "Math Calculation";
            CommandEnabled = true;
            CustomRendering = true;

            v_MathExpression = "(2 + 5) * 3";
            v_DecimalSeparator = ".";
            v_ThousandSeparator = ",";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            //get variablized string
            var variableMath = v_MathExpression.ConvertToUserVariable(engine);

            try
            {
                var decimalSeparator = v_DecimalSeparator.ConvertToUserVariable(engine);
                var thousandSeparator = v_ThousandSeparator.ConvertToUserVariable(engine);

                //remove thousandths markers
                variableMath = variableMath.Replace(thousandSeparator, "");

                //check decimal seperator
                if (decimalSeparator != ".")
                {
                    variableMath = variableMath.Replace(decimalSeparator, ".");
                }

                //perform compute
                DataTable dt = new DataTable();
                var result = dt.Compute(variableMath, "");

                //restore decimal seperator
                if (decimalSeparator != ".")
                {
                    result = result.ToString().Replace(".", decimalSeparator);
                }
               
                //store string in variable
                result.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
            }
            catch (Exception ex)
            {
                //throw exception is math calc failed
                throw ex;
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_MathExpression", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ThousandSeparator", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DecimalSeparator", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Compute '{v_MathExpression}' - Store Result in '{v_OutputUserVariableName}']";
        }
    }
}