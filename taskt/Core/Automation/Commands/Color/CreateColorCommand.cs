using System;
using System.Xml.Serialization;
using System.Drawing;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Color Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.Description("This command allows you to create Color.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create Color.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateColorCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ColorControls), nameof(ColorControls.v_InputColorVariableName))]
        public string v_Color { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ColorControls), nameof(ColorControls.v_ColorValue))]
        [PropertyDescription("Red Value")]
        [PropertyDetailSampleUsage("**{{{vRed}}}**", "Specify Variable **vRed**")]
        [PropertyValidationRule("Red", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.NotBetween)]
        [PropertyDisplayText(true, "Red")]
        public string v_Red { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ColorControls), nameof(ColorControls.v_ColorValue))]
        [PropertyDescription("Green Value")]
        [PropertyDetailSampleUsage("**{{{vGreen}}}**", "Specify Variable **vGreen**")]
        [PropertyValidationRule("Green", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.NotBetween)]
        [PropertyDisplayText(true, "Green")]
        public string v_Green { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ColorControls), nameof(ColorControls.v_ColorValue))]
        [PropertyDescription("Blue Value")]
        [PropertyDetailSampleUsage("**{{{vBlue}}}**", "Specify Variable **vBlue**")]
        [PropertyValidationRule("Blue", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.NotBetween)]
        [PropertyDisplayText(true, "Blue")]
        public string v_Blue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ColorControls), nameof(ColorControls.v_ColorValue))]
        [PropertyDescription("Alpha Value")]
        [PropertyDetailSampleUsage("**{{{vAlpha}}}**", "Specify Variable **vAlpha**")]
        [PropertyIsOptional(true, "255")]
        [PropertyValidationRule("Alpha", PropertyValidationRule.ValidationRuleFlags.NotBetween)]
        [PropertyDisplayText(true, "Alpha")]
        public string v_Alpha { get; set; }

        public CreateColorCommand()
        {
            this.CommandName = "CreateColorCommand";
            this.SelectionName = "Create Color";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            int r = this.ConvertToUserVariableAsInteger(nameof(v_Red), "Red", engine);
            int g = this.ConvertToUserVariableAsInteger(nameof(v_Green), "Green", engine);
            int b = this.ConvertToUserVariableAsInteger(nameof(v_Blue), "Blue", engine);

            int a = this.ConvertToUserVariableAsInteger(nameof(v_Alpha), "Alpha", engine);

            Color co = Color.FromArgb(a, r, g, b);
            co.StoreInUserVariable(engine, v_Color);
        }
    }
}