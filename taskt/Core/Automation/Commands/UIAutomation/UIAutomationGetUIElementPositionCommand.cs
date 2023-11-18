using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Get From UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get UIElement Position")]
    [Attributes.ClassAttributes.Description("This command allows you to get UIElement Position.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get UIElement Position.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationGetUIElementPositionCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [PropertyDescription("Variable Name to Store X Position")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("X Position", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "X")]
        public string v_XPosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [PropertyDescription("Variable Name to Store Y Position")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Y Position", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Y Position")]
        public string v_YPosition { get; set; }

        [XmlAttribute]
        [PropertyDescription("Base position")]
        [InputSpecification("", true)]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUISelectionOption("Top Left")]
        [PropertyUISelectionOption("Bottom Right")]
        [PropertyUISelectionOption("Top Right")]
        [PropertyUISelectionOption("Bottom Left")]
        [PropertyUISelectionOption("Center")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Top Left")]
        public string v_PositionBase { get; set; }

        public UIAutomationGetUIElementPositionCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var targetElement = v_TargetElement.ExpandUserVariableAsUIElement(engine);

            var rct = targetElement.Current.BoundingRectangle;

            double x = 0.0, y = 0.0;
            switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_PositionBase), engine))
            {
                case "top left":
                    x = rct.Left;
                    y = rct.Top;
                    break;
                case "bottom right":
                    x = rct.Right;
                    y = rct.Bottom;
                    break;
                case "top right":
                    x = rct.Right;
                    y = rct.Top;
                    break;
                case "bottom left":
                    x = rct.Left;
                    y = rct.Bottom;
                    break;
                case "center":
                    x = (rct.Right - rct.Left) / 2.0;
                    y = (rct.Bottom - rct.Top) / 2.0;
                    break;
            }

            if (!string.IsNullOrEmpty(v_XPosition))
            {
                x.StoreInUserVariable(engine, v_XPosition);
            }
            if (!string.IsNullOrEmpty(v_YPosition))
            {
                y.StoreInUserVariable(engine, v_YPosition);
            }
        }
    }
}