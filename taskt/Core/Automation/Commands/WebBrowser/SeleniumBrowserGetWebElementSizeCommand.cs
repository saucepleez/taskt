using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Get From WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Get WebElement Size")]
    [Attributes.ClassAttributes.Description("This command allows you to Get WebElement Size.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get WebElement Size.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumWebElementSizeCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Width")]
        [InputSpecification("Width")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Width", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Width")]
        public string v_Width { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Height")]
        [InputSpecification("Height")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Height", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Height")]
        public string v_Height { get; set; }

        public SeleniumWebElementSizeCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var elem = v_WebElement.ExpandUserVariableAsWebElement("WebElement", engine);

            if (!string.IsNullOrEmpty(v_Width))
            {
                elem.Size.Width.StoreInUserVariable(engine, v_Width);
            }
            if (!string.IsNullOrEmpty(v_Height))
            {
                elem.Size.Height.StoreInUserVariable(engine, v_Height);
            }
        }
    }
}