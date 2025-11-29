using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Instance")]
    [Attributes.ClassAttributes.CommandSettings("Attach Web Browser Instance")]
    [Attributes.ClassAttributes.Description("This command allows you to create a new Selenium web browser session which enables automation for websites.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create a browser that will eventually perform web automation such as checking an internal company intranet site to retrieve data.\nIf this command does not work, please check your browser version, and WebDriver version.\nYou can check the WebDriver version with \"foo.exe -v\" in command prompt.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumAttachCreateWebBrowserInstanceCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyTextBoxSetting(1, false)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Web Browser Type")]
        [PropertyUISelectionOption("Edge")]
        [PropertyUISelectionOption("Chrome")]
        //[PropertyUISelectionOption("Firefox")]    // Firefox not supported now
        [InputSpecification("", true)]
        [Remarks("")]
        [PropertyIsOptional(true, "Chrome")]
        [PropertyFirstValue("Chrome")]
        [PropertyDisplayText(true, "Web Browser Type")]
        public string v_EngineType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Debugger Address")]
        [PropertyIsOptional(true, "127.0.0.1")]
        [PropertyValidationRule("Debugger Address", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Address")]
        public string v_DebuggerAddress { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Debugger Port")]
        [PropertyIsOptional(true, "9222")]
        [PropertyValidationRule("Debugger Port", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Port")]
        public string v_DebuggerPort { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_OutputWindowHandle))]
        public string v_Handle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Web Driver Binary Path")]
        [InputSpecification("Web Driver Binary Path", true)]
        [PropertyDetailSampleUsage("**C:\\temp\\WebDriverPath.exe**", PropertyDetailSampleUsage.ValueType.Value, "WebDriver Path")]
        [PropertyDetailSampleUsage("**{{{vBrowserPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "WebDriver Path")]
        [Remarks("When path is Empty, taskt uses default WebDriver.\nIE is not supported.\nIf you use a fixed web browser version, use this parameter.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [PropertyIsOptional(true, "Empty")]
        [PropertyDisplayText(false, "")]
        public string v_WebDriverPath { get; set; }

        public SeleniumAttachCreateWebBrowserInstanceCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var driverPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "Resources");
            
            var webDriverPath = v_WebDriverPath.ExpandValueOrUserVariable(engine);

            if (string.IsNullOrEmpty(v_DebuggerAddress))
            {
                v_DebuggerAddress = "127.0.0.1";
            }
            var address = this.ExpandValueOrUserVariable(nameof(v_DebuggerAddress), "Debugger Address", engine);

            if (string.IsNullOrEmpty(v_DebuggerPort))
            {
                v_DebuggerPort = "9222";
            }
            var port = this.ExpandValueOrUserVariableAsInteger(nameof(v_DebuggerPort), engine);
            if (port < 0 || port > 65535)
            {
                throw new Exception($"Strange Debugger Port. Port: '{v_DebuggerPort}', Expand Value: '{port}'");
            }
            var debugger = $"{address}:{port}";

            OpenQA.Selenium.DriverService driverService;
            OpenQA.Selenium.IWebDriver webDriver;

            var seleniumEngine = SelectionItemsControls.ExpandValueOrUserVariableAsSelectionItem(this, nameof(v_EngineType), engine);
            switch(seleniumEngine)
            {
                case "chrome":
                    var chromeOptions = new OpenQA.Selenium.Chrome.ChromeOptions();
                    chromeOptions.DebuggerAddress = debugger;

                    if (!string.IsNullOrEmpty(webDriverPath))
                    {
                        driverService = OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService(System.IO.Path.GetDirectoryName(webDriverPath), System.IO.Path.GetFileName(webDriverPath));
                    }
                    else
                    {
                        driverService = OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService(driverPath);
                    }

                    webDriver = new OpenQA.Selenium.Chrome.ChromeDriver((OpenQA.Selenium.Chrome.ChromeDriverService)driverService, chromeOptions);
                    break;

                case "edge":
                    var edgeOptions = new OpenQA.Selenium.Edge.EdgeOptions();
                    edgeOptions.DebuggerAddress = debugger;

                    if (!string.IsNullOrEmpty(webDriverPath))
                    {
                        driverService = OpenQA.Selenium.Edge.EdgeDriverService.CreateDefaultService(System.IO.Path.GetDirectoryName(webDriverPath), System.IO.Path.GetFileName(webDriverPath));
                    }
                    else
                    {
                        driverService = OpenQA.Selenium.Edge.EdgeDriverService.CreateDefaultService(driverPath, "msedgedriver.exe");
                    }

                    webDriver = new OpenQA.Selenium.Edge.EdgeDriver((OpenQA.Selenium.Edge.EdgeDriverService)driverService, edgeOptions);
                    break;

                //case "firefox":
                //    var ffOptions = new OpenQA.Selenium.Firefox.FirefoxOptions();
                    

                //    if (!string.IsNullOrEmpty(webDriverPath))
                //    {
                //        driverService = OpenQA.Selenium.Firefox.FirefoxDriverService.CreateDefaultService(System.IO.Path.GetDirectoryName(webDriverPath), System.IO.Path.GetFileName(webDriverPath));
                //    }
                //    else
                //    {
                //        driverService = OpenQA.Selenium.Firefox.FirefoxDriverService.CreateDefaultService(driverPath);
                //    }

                //    webDriver = new OpenQA.Selenium.Firefox.FirefoxDriver((OpenQA.Selenium.Firefox.FirefoxDriverService)driverService, ffOptions);
                //    break;

                default:
                    throw new Exception("Strange Web Browser");
            }

            // add app instance
            var instanceName = v_InstanceName.ExpandValueOrUserVariable(engine);
            engine.AddAppInstance(instanceName, webDriver);

            if (!string.IsNullOrEmpty(v_Handle))
            {
                var procId = ProcessControls.GetChildProcessId(driverService.ProcessId, 1);
                if (seleniumEngine == "firefox")
                {
                    procId = ProcessControls.GetChildProcessId(procId, 0);
                }
                var whnd = WindowControls.ConvertProcessIdToWindowHandle(procId);
                whnd.StoreInUserVariable(engine, v_Handle);
            }
        }
    }
}