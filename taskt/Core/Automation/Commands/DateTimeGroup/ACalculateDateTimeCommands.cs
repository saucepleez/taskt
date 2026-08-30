using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Calculate DateTime commands
    /// </summary>
    public abstract class ACalculateDateTimeCommands : ADateTimeConvertCommands, IDateTimeResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_InputDateTime))]
        //public string v_DateTime { get; set; }

        [XmlAttribute]
        [PropertyDescription("Calculation Method")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Add Years")]
        [PropertyUISelectionOption("Add Months")]
        [PropertyUISelectionOption("Add Days")]
        [PropertyUISelectionOption("Add Hours")]
        [PropertyUISelectionOption("Add Minutes")]
        [PropertyUISelectionOption("Add Seconds")]
        [PropertyUISelectionOption("Subtract Years")]
        [PropertyUISelectionOption("Subtract Months")]
        [PropertyUISelectionOption("Subtract Days")]
        [PropertyUISelectionOption("Subtract Hours")]
        [PropertyUISelectionOption("Subtract Minutes")]
        [PropertyUISelectionOption("Subtract Seconds")]
        [PropertyValidationRule("Calculation Method", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Method")]
        [PropertyParameterOrder(6000)]
        public virtual string v_CalculationMethod { get; set; }

        [XmlAttribute]
        [PropertyDescription("Value to Add or Subtruct")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**5**", "Add or Subtract **5**")]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", "Add or Subtract Value of Variable **vValue**")]
        [Remarks("Adding **-5** is same as Subtracting **5**")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Value", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Value")]
        [PropertyParameterOrder(6001)]
        public virtual string v_Value { get; set; }

        [XmlAttribute]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DateTime, true)]
        public override string v_Result { get; set; }

        /// <summary>
        /// general caclulate datetime process
        /// </summary>
        /// <param name="create"></param>
        /// <param name="engine"></param>
        protected void CommandProcess(ADateTimeCreateCommands create, Engine.AutomationEngineInstance engine)
        {
            using (var dt = new InnerScriptVariable(engine))
            {
                create.v_DateTime = dt.VariableName;
                create.RunCommand(engine);

                var calc = new CalculateDateTimeCommand()
                {
                    v_DateTime = dt.VariableName,
                    v_CalculationMethod = this.v_CalculationMethod,
                    v_Value = this.v_Value,
                    v_Result = this.v_Result,
                };
                calc.RunCommand(engine);
            }
        }
    }
}