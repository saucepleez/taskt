using System;
using System.Xml.Serialization;
using System.Drawing;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Color")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Create Color From ARGB Number")]
    [Attributes.ClassAttributes.Description("This command allows you to create Color from ARGB Number.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create Color from ARGB Number.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CreateColorFromARGBNumberCommand : AColorCreateCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ColorControls), nameof(ColorControls.v_InputColorVariableName))]
        //public string v_Color { get; set; }

        [XmlAttribute]
        [PropertyDescription("Color ARGB Number Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**1234**", "Specify ARGB Number Value **1234**")]
        [PropertyDetailSampleUsage("**{{{vARGB}}}**", "Specify Value of Variable **vARGB** for ARGB Number Value")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("ARGB Number", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "ARGB Number")]
        [PropertyParameterOrder(6000)]
        public string v_ARGB { get; set; }

        public CreateColorFromARGBNumberCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var v = NumberControls.ExpandValueOrUserVariableAsInteger(v_ARGB, "ARGB Number", engine);

            var co = Color.FromArgb(v);
            this.StoreColorInUserVariable(co, nameof(v_Color), engine);
        }
    }
}