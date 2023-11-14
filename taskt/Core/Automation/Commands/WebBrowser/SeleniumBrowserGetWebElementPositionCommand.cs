using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Get From WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Get WebElement Position")]
    [Attributes.ClassAttributes.Description("This command allows you to Get WebElement Position.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get WebElement Position.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumWebElementPositionCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store X Position")]
        [InputSpecification("X Position")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("X Position", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "X Position")]
        public string v_XPosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Y Position")]
        [InputSpecification("Y Position")]
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

        public SeleniumWebElementPositionCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var elem = v_WebElement.ExpandUserVariableAsWebElement("WebElement", engine);

            var loc = elem.Location;
            var size = elem.Size;

            int x = 0, y = 0;
            switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_PositionBase), engine))
            {
                case "top left":
                    x = loc.X;
                    y = loc.Y;
                    break;
                case "bottom right":
                    x = loc.X + size.Width;
                    y = loc.Y + size.Height;
                    break;
                case "top right":
                    x = loc.X + size.Width;
                    y = loc.Y;
                    break;
                case "bottom left":
                    x = loc.X;
                    y = loc.Y + size.Height;
                    break;
                case "center":
                    x = (loc.X + size.Width) / 2;
                    y = (loc.Y + size.Height) / 2;
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