using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Get From UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get UIElement Size")]
    [Attributes.ClassAttributes.Description("This command allows you to get UIElement Size.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get UIElement Size.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationGetUIElementSizeCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [PropertyDescription("Variable Name to Store Width")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Width", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Width")]
        public string v_Width { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [PropertyDescription("Variable Name to Store Height")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Height", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Height")]
        public string v_Height { get; set; }

        public UIAutomationGetUIElementSizeCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var targetElement = v_TargetElement.ExpandUserVariableAsUIElement(engine);

            var rct = targetElement.Current.BoundingRectangle;
            if (!string.IsNullOrEmpty(v_Width))
            {
                rct.Width.StoreInUserVariable(engine, v_Width);
            }
            if (!string.IsNullOrEmpty(v_Height))
            {
                rct.Height.StoreInUserVariable(engine, v_Height);
            }
        }
    }
}