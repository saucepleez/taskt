using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DateTime Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Calculate DateTime")]
    [Attributes.ClassAttributes.Description("This command allows you to Calculate DateTime. Add Day, Minute, etc.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Calculate DateTime. Add Day, Minute, etc.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CalculateDateTimeCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_InputDateTime))]
        public string v_DateTime { get; set; }

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
        [PropertyUISelectionOption("Substract Years")]
        [PropertyUISelectionOption("Substract Months")]
        [PropertyUISelectionOption("Substract Days")]
        [PropertyUISelectionOption("Substract Hours")]
        [PropertyUISelectionOption("Substract Minutes")]
        [PropertyUISelectionOption("Substract Seconds")]
        [PropertyValidationRule("Calculation Method", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Method")]
        public string v_CalculationMethod { get; set; }

        [XmlAttribute]
        [PropertyDescription("Value to Add or Substruct")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**5**", "Add or Substruct **5**")]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", "Add or Substruct Value of Variable **vValue**")]
        [Remarks("Adding **-5** is same as Substructing **5**")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Value", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Value")]
        public string v_Value { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DateTime, true)]
        public string v_Result { get; set; }

        public CalculateDateTimeCommand()
        {
            //this.CommandName = "CalculateDateTimeCommand";
            //this.SelectionName = "Calculate DateTime";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            var myDT = v_DateTime.GetDateTimeVariable(engine);

            string meth = this.GetUISelectionValue(nameof(v_CalculationMethod), engine);

            int value = this.ConvertToUserVariableAsInteger(nameof(v_Value), engine);

            string[] method = meth.Split(' ');
            if (method[0] == "substract")
            {
                value = -value;
            }

            DateTime calcDT = new DateTime();
            switch (method[1])
            {
                case "years":
                    calcDT = myDT.AddYears(value);
                    break;
                case "months":
                    calcDT = myDT.AddMonths(value);
                    break;
                case "days":
                    calcDT = myDT.AddDays(value);
                    break;
                case "hours":
                    calcDT = myDT.AddHours(value);
                    break;
                case "minutes":
                    calcDT = myDT.AddMinutes(value);
                    break;
                case "seconds":
                    calcDT = myDT.AddSeconds(value);
                    break;
            }
            calcDT.StoreInUserVariable(engine, v_Result);
        }
    }
}