using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Numerical Commands")]
    [Attributes.ClassAttributes.CommandSettings("Round Number")]
    [Attributes.ClassAttributes.Description("This command allows you to Round up, down, or round off numbers.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Round up, down, or round off numbers.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RoundNumberCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        public string v_Numeric { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Round Type")]
        [PropertyUISelectionOption("Round")]
        [PropertyUISelectionOption("Round Up")]
        [PropertyUISelectionOption("Round Down")]
        [PropertyValidationRule("Round Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Type")]
        public string v_RoundType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        public RoundNumberCommand()
        {
            //this.CommandName = "RoundNumberCommand";
            //this.SelectionName = "Round Number";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //decimal num = new PropertyConvertTag(v_Numeric, "Number").ConvertToUserVariableAsDecimal(engine);
            //var num = this.ConvertToUserVariableAsDecimal(nameof(v_Numeric), "Number", engine);
            var num = this.ConvertToUserVariableAsDecimal(nameof(v_Numeric), engine);

            //var round = v_RoundType.GetUISelectionValue("v_RoundType", this, engine);
            //var round = this.GetUISelectionValue(nameof(v_RoundType), "Round Type", engine);
            var round = this.GetUISelectionValue(nameof(v_RoundType), engine);

            decimal res = 0;
            switch (round)
            {
                case "round":
                    res = Math.Round(num, MidpointRounding.AwayFromZero);
                    break;
                case "round up":
                    res = Math.Ceiling(num);
                    break;
                case "round down":
                    res = Math.Truncate(num);
                    break;
            }

            res.ToString().StoreInUserVariable(engine, v_Result);
        }
    }
}