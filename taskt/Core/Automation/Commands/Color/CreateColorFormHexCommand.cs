using System;
using System.Xml.Serialization;
using System.Drawing;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Color Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Create Color From HEX")]
    [Attributes.ClassAttributes.Description("This command allows you to create Color from HEX.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create Color from HEX.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateColorFromHexCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ColorControls), nameof(ColorControls.v_InputColorVariableName))]
        public string v_Color { get; set; }

        [XmlAttribute]
        [PropertyDescription("Color HEX Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**#ff22bb**", "Specify Hex Value **ff22bb**")]
        [PropertyDetailSampleUsage("**ff22bb**", "Specify Hex Value **ff22bb**")]
        [PropertyDetailSampleUsage("**{{{vHex}}}**", "Specify Value of Variable **vHex** for HEX Value")]
        [Remarks("Please specify a 6-digit Hexadecimal number")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Hex", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Hex")]
        public string v_Hex { get; set; }

        public CreateColorFromHexCommand()
        {
            //this.CommandName = "CreateColorFromHEXCommand";
            //this.SelectionName = "Create Color From HEX";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            string hex = v_Hex.ConvertToUserVariable(engine);
            if (hex.Length > 6)
            {
                hex = hex.Substring(hex.Length - 6);
            }
            int b = Int32.Parse(hex.Substring(hex.Length - 2), System.Globalization.NumberStyles.HexNumber);
            hex = hex.Substring(0, hex.Length - 2);
            int g = Int32.Parse(hex.Substring(hex.Length - 2), System.Globalization.NumberStyles.HexNumber);
            hex = hex.Substring(0, hex.Length - 2);
            int r = Int32.Parse(hex.Substring(hex.Length - 2), System.Globalization.NumberStyles.HexNumber);

            Color co = Color.FromArgb(255, r, g, b);
            co.StoreInUserVariable(engine, v_Color);
        }
    }
}