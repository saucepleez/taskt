using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Get From Web Browser")]
    [Attributes.ClassAttributes.CommandSettings("Get Web Browser Size")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Web Browser Size.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Web Browser Position.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetWebBrowserSizeCommand : ASeleniumWebDriverActionCommands, ISizeProperties, ICanExecuteJavaScriptToWebDriver
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_OptionalResult))]
        [PropertyDescription("Variable Name to Store Width")]
        [PropertyValidationRule("Width", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Width")]
        [PropertyParameterOrder(6000)]
        public string v_Width { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_OptionalResult))]
        [PropertyDescription("Variable Name to Store Height")]
        [PropertyValidationRule("Height", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Height")]
        [PropertyParameterOrder(6100)]
        public string v_Height { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Position Type")]
        [PropertyUISelectionOption("Window")]
        [PropertyUISelectionOption("Viewport")]
        [PropertyIsOptional(true, "Window")]
        [PropertyParameterOrder(7000)]
        public string v_SizeType { get; set; }

        public SeleniumBrowserGetWebBrowserSizeCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WebDriverActionCore(new Action<OpenQA.Selenium.IWebDriver, string>((seleniumInstance, _) =>
            {
                string script = string.Empty;
                switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_SizeType), engine))
                {
                    case "window":
                        script = "return window.outerWidth+','+window.outerHeight;";
                        break;
                    case "viewport":
                        script = "return window.innerWidth+','+window.innerHeight;";
                        break;
                }
                var r = this.ExecuteJavaScript(seleniumInstance, script).ToString();

                var spt = r.Split(',');

                if (!string.IsNullOrEmpty(v_Width))
                {
                    spt[0].StoreInUserVariable(engine, v_Width);
                }
                if (!string.IsNullOrEmpty(v_Height))
                {
                    spt[1].StoreInUserVariable(engine, v_Height);
                }
            }), engine);
        }
    }
}