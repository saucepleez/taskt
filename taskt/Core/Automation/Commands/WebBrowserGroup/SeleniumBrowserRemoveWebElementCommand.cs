using System;
using taskt.Core.Automation.Commands.WebBrowserGroup;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("WebElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Remove WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Remove WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Remove WebElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserRemoveWebElementCommand : ASeleniumWebElementActionAndScrollCommands, ICanExecuteJavaScriptToWebDriver
    {
        public SeleniumBrowserRemoveWebElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WebElementActionAndScroll(
                new Action<OpenQA.Selenium.IWebElement, OpenQA.Selenium.IWebDriver>((el, dr) =>
                {
                    using (var path = new InnerScriptVariable(engine))
                    {
                        var getPath = new SeleniumBrowserGetCSSSelectorFromWebElementCommand()
                        {
                            v_WebElement = this.v_WebElement,
                            v_Result = path.VariableName,
                        };
                        getPath.RunCommand(engine);

                        this.ExecuteJavaScript(dr, @"
const elem = document.querySelector(arguments[0]);
elem.remove();
", path.VariableValue.ToString());
                    }
                }), engine,
                new Action<Exception>(ex =>
                {
                    throw new Exception($"{EM_SeleniumWebElementActionPropertiesExtensionMehtods.GetFailActionMessage("Remove")}");
                })
            );
        }
    }
}