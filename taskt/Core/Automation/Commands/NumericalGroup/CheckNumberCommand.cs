using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Numerical")]
    [Attributes.ClassAttributes.CommandSettings("Check Number")]
    [Attributes.ClassAttributes.Description("This command allows you to Check Number Value.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Check Number Value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CheckNumberCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        public string v_TargetValue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_CheckMethod))]
        [PropertySelectionChangeEvent(nameof(cmbCompareMethod_SelectionChangeCommitted))]
        public string v_CheckMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        [PropertyDescription("Numerical Value to be Compared")]
        [PropertyDisplayText(true, "Compared")]

        public string v_ComparedValue1 { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        [PropertyDescription("Numerical Value to be Compared 2")]
        [PropertyIsOptional(true, "")]
        [PropertyValidationRule("Compared Value 2", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Compared2")]
        public string v_ComparedValue2 { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_OutputNumericalVariableName))]
        public string v_Result { get; set; }

        public CheckNumberCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var method = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_CheckMethod), engine);

            var vStr = this.ExpandValueOrUserVariable(nameof(v_TargetValue), "Target Value", engine);
            bool res = false;
            switch (method)
            {
                case "is number":
                    res = decimal.TryParse(vStr, out _);
                    break;
                case "is not number":
                    res = !decimal.TryParse(vStr, out _);
                    break;
                default:
                    // check number is ...
                    var v = this.ExpandValueOrUserVariableAsDecimal(nameof(v_TargetValue), engine);
                    var decimalPoint = v % 1;
                    switch (method)
                    {
                        case "is odd number":
                            if (decimalPoint == 0)
                            {
                                res = ((v % 2) == 1);
                            }
                            break;
                        case "is even number":
                            if (decimalPoint == 0)
                            {
                                res = ((v % 2) == 0);
                            }
                            break;
                        case "is zero":
                            res = (v == 0);
                            break;
                        case "is not zero":
                            res = (v != 0);
                            break;
                        case "is positive value":
                            res = (v > 0);
                            break;
                        case "is negative value":
                            res = (v < 0);
                            break;
                        case "is integer":
                            res = (decimalPoint == 0);
                            break;
                        case "is not integer":
                            res = (decimalPoint != 0);
                            break;

                        default:
                            // compare other numbers
                            var a = v;
                            decimal b, c;
                            switch (method)
                            {
                                case "is between":
                                case "is not between":
                                    b = this.ExpandValueOrUserVariableAsDecimal(nameof(v_ComparedValue1), engine);
                                    if (!string.IsNullOrEmpty(v_ComparedValue2))
                                    {
                                        c = this.ExpandValueOrUserVariableAsDecimal(nameof(v_ComparedValue2), engine);
                                    }
                                    else
                                    {
                                        throw new Exception("Compared Value 2 is not Specified.");
                                    }

                                    if (b > c)
                                    {
                                        (b, c) = (c, b);    // swap
                                    }
                                    break;

                                default:
                                    b = this.ExpandValueOrUserVariableAsDecimal(nameof(v_ComparedValue1), engine);
                                    c = 0;
                                    break;
                            }
                            switch (method)
                            {
                                case "is equal to":
                                    res = (a == b);
                                    break;
                                case "is not equal to":
                                    res = (a != b);
                                    break;
                                case "is greater than":
                                    res = (a > b);
                                    break;
                                case "is greater than or equal to":
                                    res = (a >= b);
                                    break;
                                case "is less than":
                                    res = (a < b);
                                    break;
                                case "is less than or equal to":
                                    res = (a <= b);
                                    break;
                                case "is between":
                                    res = (a >= b) && (a <= c);
                                    break;
                                case "is not between":
                                    res = (a < b) || (a > c);
                                    break;
                            }
                            break;
                    }
                    break;
            }

            res.StoreInUserVariable(engine, v_Result);
        }

        private void cmbCompareMethod_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var cmb = (ComboBox)sender;
            bool visible1 = false;
            bool visible2 = false;
            switch (cmb.SelectedItem.ToString().ToLower())
            {
                case "is equal to":
                case "is not equal to":
                case "is greater than":
                case "is greater than or equal to":
                case "is less than":
                case "is less than or equal to":
                    visible1 = true;
                    visible2 = false;
                    break;
                case "is between":
                case "is not between":
                    visible1 = true;
                    visible2 = true;
                    break;
            }
            FormUIControls.SetVisibleParameterControlGroup(this.ControlsList, nameof(v_ComparedValue1), visible1);
            FormUIControls.SetVisibleParameterControlGroup(this.ControlsList, nameof(v_ComparedValue2), visible2);
        }
    }
}
