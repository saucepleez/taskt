using OpenQA.Selenium;
using System;
using System.IO;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for create WebDriver (WebBrowser instance) commands
    /// </summary>
    public abstract class ASeleniumCreateWebDriverCommands : ASeleniumDoSomethingToWebDriverCommands, ISeleniumCreateWebDriverProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_OutputInstanceName))]
        //[PropertyParameterOrder(5000)]
        public override string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Web Browser Type")]
        [PropertyUISelectionOption("Edge")]
        [PropertyUISelectionOption("Chrome")]
        [PropertyUISelectionOption("Firefox")]
        [PropertyUISelectionOptionBehavior(MultiAttributesBehavior.Merge)]
        [PropertyIsOptional(true, "Chrome")]
        [PropertyFirstValue("Chrome")]
        [PropertyDisplayText(true, "Web Browser Type")]
        [PropertyParameterOrder(6000)]
        public virtual string v_BrowserType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Web Driver Binary Path")]
        [InputSpecification("Web Driver Binary Path", true)]
        [PropertyDetailSampleUsage("**C:\\temp\\WebDriverPath.exe**", PropertyDetailSampleUsage.ValueType.Value, "WebDriver Path")]
        [PropertyDetailSampleUsage("**{{{vBrowserPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "WebDriver Path")]
        [Remarks("When path is Empty, taskt uses default WebDriver.\nIE is not supported.\nIf you use a fixed web browser version, use this parameter.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [PropertyIsOptional(true, "Empty")]
        [PropertyDisplayText(false, "Web Driver Binary")]
        [PropertyParameterOrder(13000)]
        public virtual string v_WebDriverPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Hide Terminal Window")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [PropertyDisplayText(false, "Hide Terminal")]
        [PropertyParameterOrder(14000)]
        public virtual string v_HideTerminalWindow { get; set; }

        /// <summary>
        /// create WebDriver Searvice
        /// </summary>
        /// <param name="driverFunc"></param>
        /// <param name="webDriverName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        protected DriverService CreateWebDriverService(Func<string, string, DriverService> driverFunc, string webDriverName, Engine.AutomationEngineInstance engine)
        {
            var driverPath = this.ExpandValueOrUserVariable(nameof(v_WebDriverPath), "Web Driver Binary", engine);
            DriverService ret;
            if (string.IsNullOrEmpty(driverPath))
            {
                ret = driverFunc(IO.Folders.GetResourcesFolderPath(), webDriverName);
            }
            else
            {
                ret = driverFunc(Path.GetDirectoryName(driverPath), Path.GetFileName(driverPath));
            }
            var hideTerminal = this.ExpandValueOrUserVariableAsYesNo(nameof(v_HideTerminalWindow), engine);
            ret.HideCommandPromptWindow = hideTerminal;
            return ret;
        }

        /// <summary>
        /// create web driver service
        /// </summary>
        /// <param name="browserType"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected DriverService CreateWebDriverService(string browserType, Engine.AutomationEngineInstance engine)
        {
            switch (browserType.ToLower())
            {
                case "chrome":
                    return CreateWebDriverService(OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService, "chromedriver.exe", engine);

                case "edge":
                    return CreateWebDriverService(OpenQA.Selenium.Edge.EdgeDriverService.CreateDefaultService, "msedgedriver.exe", engine);

                case "firefox":
                    return CreateWebDriverService(OpenQA.Selenium.Firefox.FirefoxDriverService.CreateDefaultService, "geckodriver.exe", engine);

                case "ie":
                    return CreateWebDriverService(OpenQA.Selenium.IE.InternetExplorerDriverService.CreateDefaultService, "IEDriverServer.exe", engine);

                default:
                    throw new Exception($"Strange Web Browser type. Type: '{browserType}'");
            }
        }
    }
}
