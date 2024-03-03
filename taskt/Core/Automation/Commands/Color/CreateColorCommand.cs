using System;
using System.Xml.Serialization;
using System.Drawing;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Color Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Create Color")]
    [Attributes.ClassAttributes.Description("This command allows you to create Color.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create Color.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateColorCommand : AColorCreateCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ColorControls), nameof(ColorControls.v_InputColorVariableName))]
        //public string v_Color { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ColorControls), nameof(ColorControls.v_ColorValue))]
        [PropertyDescription("Red Value")]
        [PropertyDetailSampleUsage("**{{{vRed}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Red")]
        [PropertyValidationRule("Red", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.NotBetween)]
        [PropertyDisplayText(true, "Red")]
        [PropertyParameterOrder(6000)]
        public string v_Red { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ColorControls), nameof(ColorControls.v_ColorValue))]
        [PropertyDescription("Green Value")]
        [PropertyDetailSampleUsage("**{{{vGreen}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Green")]
        [PropertyValidationRule("Green", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.NotBetween)]
        [PropertyDisplayText(true, "Green")]
        [PropertyParameterOrder(6001)]
        public string v_Green { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ColorControls), nameof(ColorControls.v_ColorValue))]
        [PropertyDescription("Blue Value")]
        [PropertyDetailSampleUsage("**{{{vBlue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Blue")]
        [PropertyValidationRule("Blue", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.NotBetween)]
        [PropertyDisplayText(true, "Blue")]
        [PropertyParameterOrder(6002)]
        public string v_Blue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ColorControls), nameof(ColorControls.v_ColorValue))]
        [PropertyDescription("Alpha Value")]
        [PropertyDetailSampleUsage("**{{{vAlpha}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Alpha")]
        [PropertyIsOptional(true, "255")]
        [PropertyValidationRule("Alpha", PropertyValidationRule.ValidationRuleFlags.NotBetween)]
        [PropertyDisplayText(true, "Alpha")]
        [PropertyParameterOrder(6003)]
        public string v_Alpha { get; set; }

        public CreateColorCommand()
        {
            //this.CommandName = "CreateColorCommand";
            //this.SelectionName = "Create Color";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            int r = this.ExpandValueOrUserVariableAsInteger(nameof(v_Red), engine);
            int g = this.ExpandValueOrUserVariableAsInteger(nameof(v_Green), engine);
            int b = this.ExpandValueOrUserVariableAsInteger(nameof(v_Blue), engine);

            int a = this.ExpandValueOrUserVariableAsInteger(nameof(v_Alpha), engine);

            var co = Color.FromArgb(a, r, g, b);
            //co.StoreInUserVariable(engine, v_Color);
            this.StoreColorInUserVariable(co, nameof(v_Color), engine);
        }
    }
}