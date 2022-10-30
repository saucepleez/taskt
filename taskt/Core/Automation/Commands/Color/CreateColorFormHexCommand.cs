using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Color Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.Description("This command allows you to create Color from HEX.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create Color from HEX.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateColorFromHexCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a Color Variable Name")]
        [InputSpecification("")]
        [SampleUsage("**vColor** or **{{{vColor}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Color, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable")]
        public string v_Color { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Color HEX Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**#ff22bb** or *ff22bb** or **{{{vHex}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Hex", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Hex")]
        public string v_Hex { get; set; }

        public CreateColorFromHexCommand()
        {
            this.CommandName = "CreateColorFromHEXCommand";
            this.SelectionName = "Create Color From HEX";
            this.CommandEnabled = true;
            this.CustomRendering = true;
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