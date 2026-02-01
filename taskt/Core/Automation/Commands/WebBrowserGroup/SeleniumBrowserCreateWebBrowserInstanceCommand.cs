using OpenQA.Selenium;
using OpenQA.Selenium.Chromium;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Instance")]
    [Attributes.ClassAttributes.CommandSettings("Create Web Browser Instance")]
    [Attributes.ClassAttributes.Description("This command allows you to create a new Selenium web browser session which enables automation for websites.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create a browser that will eventually perform web automation such as checking an internal company intranet site to retrieve data.\nIf this command does not work, please check your browser version, and WebDriver version.\nYou can check the WebDriver version with \"foo.exe -v\" in command prompt.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserCreateWebBrowserInstanceCommand : ASeleniumCreateWebDriverCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        //[PropertyTextBoxSetting(1, false)]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Web Browser Type")]
        [PropertyUISelectionOption("Edge")]
        [PropertyUISelectionOption("Chrome")]
        [PropertyUISelectionOption("Firefox")]
        [PropertyUISelectionOption("IE")]
        [InputSpecification("", true)]
        [Remarks("")]
        [PropertyIsOptional(true, "Chrome")]
        [PropertyFirstValue("Chrome")]
        [PropertyDisplayText(true, "Web Browser Type")]
        [PropertyParameterOrder(6000)]
        public string v_BrowserType { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        //[PropertyDescription("Instance Tracking (After task ends)")]
        //[PropertyUISelectionOption("Forget Instance")]
        //[PropertyUISelectionOption("Keep Instance Alive")]
        //[InputSpecification("Specify if taskt should remember this instance name after the script has finished executing.")]
        ////[SampleUsage("Select **Forget Instance** to  or **Keep Instance Alive** to allow subsequent tasks to call the instance by name.")]
        //[PropertyDetailSampleUsage("**Forget Instance**", "Forget the instance After tasks ends")]
        //[PropertyDetailSampleUsage("**Keep Instance Alive**", "Allow subsequent tasks to call the instance by name")]
        //[Remarks("Calling the **Close Browser** command or ending the browser session will end the instance.  This command only works during the lifetime of the application.  If the application is closed, the references will be forgetten automatically.")]
        //[PropertyIsOptional(true, "Forget Instance")]
        //[PropertyDisplayText(false, "")]
        //public string v_InstanceTracking { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Window State")]
        [PropertyUISelectionOption("Normal")]
        [PropertyUISelectionOption("Maximize")]
        [InputSpecification("Select the window state that the browser should start up with.")]
        //[SampleUsage("Select **Normal** to start the browser in normal mode or **Maximize** to start the browser in maximized mode.")]
        [PropertyDetailSampleUsage("**Normal**", "Start the WebBrowser in Normal mode")]
        [PropertyDetailSampleUsage("**Maximize**", "Start the WebBrowser in maximized mode")]
        [Remarks("")]
        [PropertyIsOptional(true, "Normal")]
        [PropertyDisplayText(false, "Window State")]
        [PropertyParameterOrder(7000)]
        public string v_BrowserWindowOption { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Profile Folder Path")]
        [PropertyIsOptional(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [PropertyValidationRule("Profile", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Profile")]
        [PropertyParameterOrder(8000)]
        public string v_ProfileFolder { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Use Headless")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [PropertyDisplayText(false, "")]
        [Remarks("Headless mode does not show WebBrowser window")]
        [PropertyParameterOrder(9000)]
        public string v_HeadlessMode { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_MultiLinesTextBox))]
        [PropertyDescription("Web Browser Command Line Options (one option per line)")]
        [InputSpecification("Command Line Options", true)]
        [SampleUsage("user-data-dir=c:\\users\\public\\SeleniumTasktProfile")]
        [Remarks("")]
        [PropertyIsOptional(true)]
        [PropertyTextBoxSetting(3, true)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(10000)]
        public string v_SeleniumOptions { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_OutputWindowHandle))]
        [PropertyParameterOrder(11000)]
        public string v_Handle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Web Browser Binary Path")]
        [InputSpecification("Web Browser Binary Path", true)]
        //[SampleUsage("**C:\\temp\\BrowserPath.exe** or **{{{vPath}}}**")]
        [PropertyDetailSampleUsage("**C:\\temp\\BrowserPath.exe**", PropertyDetailSampleUsage.ValueType.Value, "WebBrowser Path")]
        [PropertyDetailSampleUsage("**{{{vBrowserPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "WebBrowser Path")]
        [Remarks("When path is Empty, taskt try open default path.\nEdge and IE is not supported.\nIf you use a fixed web browser version, use this parameter.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [PropertyIsOptional(true, "Empty")]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(12000)]
        public string v_BrowserPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Web Driver Binary Path")]
        [InputSpecification("Web Driver Binary Path", true)]
        //[SampleUsage("**C:\\temp\\WebDriverPath.exe** or **{{{vPath}}}**")]
        [PropertyDetailSampleUsage("**C:\\temp\\WebDriverPath.exe**", PropertyDetailSampleUsage.ValueType.Value, "WebDriver Path")]
        [PropertyDetailSampleUsage("**{{{vBrowserPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "WebDriver Path")]
        [Remarks("When path is Empty, taskt uses default WebDriver.\nIE is not supported.\nIf you use a fixed web browser version, use this parameter.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [PropertyIsOptional(true, "Empty")]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(13000)]
        public string v_WebDriverPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Hide Terminal Window")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [PropertyDisplayText(false, "Hide Terminal")]
        [PropertyParameterOrder(14000)]
        public string v_HideTerminalWindow { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Temporary Folder when does not specified")]
        [PropertyUISelectionOption("User Temp")]
        [PropertyUISelectionOption("taskt Temporary")]
        [PropertyIsOptional(true, "User Temp")]
        [PropertyValidationRule("Temporary Folder", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Temporary Folder")]
        [PropertyParameterOrder(15000)]
        public string v_TemporaryProfileFolder { get; set; }

        public SeleniumBrowserCreateWebBrowserInstanceCommand()
        {
            //this.CommandName = "SeleniumBrowserCreateCommand";
            //this.SelectionName = "Create Browser";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_EngineType = "Chrome";
            //this.v_InstanceName = "";
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var seleniumEngine = SelectionItemsControls.ExpandValueOrUserVariableAsSelectionItem(this, nameof(v_BrowserType), engine);

            //var driverPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "Resources");
            //
            //var webDriverPath = v_WebDriverPath.ExpandValueOrUserVariable(engine);

            var browserPath = v_BrowserPath.ExpandValueOrUserVariable(engine);

            string profilePath = string.Empty;

            string GetTemporaryProfilePath()
            {
                var folderName = $"prof-{Guid.NewGuid().ToString()}";
                switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_TemporaryProfileFolder), engine))
                {
                    case "user temp":
                        return Path.Combine(IO.Folders.GetUserTemporaryFolderPath(), folderName);
                        
                    case "taskt temporary":
                        return Path.Combine(IO.Folders.GetTasktTemporaryFolderPath(), folderName);
                    default:
                        return string.Empty;
                }
            }

            void SetChromiumOptions(ChromiumOptions options)
            {   
                if (!string.IsNullOrEmpty(browserPath))
                {
                    options.BinaryLocation = browserPath;
                }

                if (!string.IsNullOrEmpty(v_ProfileFolder))
                {
                    var profileFolder = v_ProfileFolder.ExpandValueOrUserVariable(engine);
                    options.AddArgument($"--user-data-dir={profileFolder}");
                    profilePath = profileFolder;
                }

                if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_HeadlessMode), engine))
                {
                    options.AddArgument("--headless");
                }

                if (!string.IsNullOrEmpty(v_SeleniumOptions))
                {
                    var convertedOptions = v_SeleniumOptions.ExpandValueOrUserVariable(engine);

                    var spt = convertedOptions.Replace("\r\n", "\r").Split(new char[] { '\r', '\n' });

                    foreach (var opt in spt)
                    {
                        var opt2 = opt;
                        options.AddArgument(opt2);
                        if (opt2.StartsWith("user-data-dir=") || opt2.StartsWith("--user-data-dir="))
                        {
                            if (opt2.StartsWith("user-data-dir="))
                            {
                                profilePath = opt.Substring(14);
                            }
                            else
                            {
                                profilePath = opt.Substring(16);
                            }
                        }
                    }
                }

                // profile folder does not specified
                if (string.IsNullOrEmpty(profilePath))
                {
                    profilePath = GetTemporaryProfilePath();
                    options.AddArgument($"user-data-dir={profilePath}");
                }
            }

            DriverService CreateDriverService(Func<string, string, DriverService> driverFunc, string webDriverName)
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

            DriverService driverService;
            IWebDriver webDriver;
            if (seleniumEngine == "chrome")
            {
                OpenQA.Selenium.Chrome.ChromeOptions options = new OpenQA.Selenium.Chrome.ChromeOptions();
                
                SetChromiumOptions(options);

                //if (!string.IsNullOrEmpty(webDriverPath))
                //{
                //    driverService = OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService(System.IO.Path.GetDirectoryName(webDriverPath), System.IO.Path.GetFileName(webDriverPath));
                //}
                //else
                //{
                //    driverService = OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService(driverPath);
                //}
                //driverService.HideCommandPromptWindow = hideTerminal;
                driverService = CreateDriverService(OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService, "chromedriver.exe");
                
                webDriver = new OpenQA.Selenium.Chrome.ChromeDriver((OpenQA.Selenium.Chrome.ChromeDriverService)driverService, options);
            }
            else if (seleniumEngine == "edge")
            {
                OpenQA.Selenium.Edge.EdgeOptions options = new OpenQA.Selenium.Edge.EdgeOptions();

                SetChromiumOptions(options);

                //if (!string.IsNullOrEmpty(webDriverPath))
                //{
                //    driverService = OpenQA.Selenium.Edge.EdgeDriverService.CreateDefaultService(System.IO.Path.GetDirectoryName(webDriverPath), System.IO.Path.GetFileName(webDriverPath));
                //}
                //else
                //{
                //    driverService = OpenQA.Selenium.Edge.EdgeDriverService.CreateDefaultService(driverPath, "msedgedriver.exe");
                //}
                //driverService.HideCommandPromptWindow = hideTerminal;
                driverService = CreateDriverService(OpenQA.Selenium.Edge.EdgeDriverService.CreateDefaultService, "msedgedriver.exe");

                webDriver = new OpenQA.Selenium.Edge.EdgeDriver((OpenQA.Selenium.Edge.EdgeDriverService)driverService, options);
            }
            else if (seleniumEngine == "firefox")
            {
                OpenQA.Selenium.Firefox.FirefoxOptions options = new OpenQA.Selenium.Firefox.FirefoxOptions();
                if (!string.IsNullOrEmpty(browserPath))
                {
                    options.BinaryLocation = browserPath;
                }
                else
                {
                    options.BinaryLocation = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                }

                if (!string.IsNullOrEmpty(v_ProfileFolder))
                {
                    var profileFolder = v_ProfileFolder.ExpandValueOrUserVariable(engine);

                    //options.Profile = new OpenQA.Selenium.Firefox.FirefoxProfile(profileFolder);
                    options.AddArgument($"-profile={profileFolder}");
                    profilePath = profileFolder;
                }

                if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_HeadlessMode), engine))
                {
                    options.AddArgument("-headless");
                }

                if (!string.IsNullOrEmpty(v_SeleniumOptions))
                {
                    var convertedOptions = v_SeleniumOptions.ExpandValueOrUserVariable(engine);
                    
                    var spt = convertedOptions.Replace("\r\n", "\r").Split(new char[] { '\r', '\n' });

                    foreach (var opt in spt)
                    {
                        var opt2 = opt;
                        options.AddArgument(opt2);
                        if (opt2.StartsWith("-profile=") || opt2.StartsWith("--profile="))
                        {
                            if (opt2.StartsWith("-profile="))
                            {
                                profilePath = opt2.Substring(9);
                            }
                            else
                            {
                                profilePath = opt2.Substring(10);
                            }
                        }
                    }
                }

                // profile folder does not specified
                if (string.IsNullOrEmpty(profilePath))
                {
                    profilePath = GetTemporaryProfilePath();
                    //options.Profile = new OpenQA.Selenium.Firefox.FirefoxProfile(profilePath);
                    options.AddArgument($"-profile={profilePath}");
                    if (!Directory.Exists(profilePath))
                    {
                        Directory.CreateDirectory(profilePath);
                    }
                }

                //if (!string.IsNullOrEmpty(webDriverPath))
                //{
                //    driverService = OpenQA.Selenium.Firefox.FirefoxDriverService.CreateDefaultService(System.IO.Path.GetDirectoryName(webDriverPath), System.IO.Path.GetFileName(webDriverPath));
                //}
                //else
                //{
                //    driverService = OpenQA.Selenium.Firefox.FirefoxDriverService.CreateDefaultService(driverPath);
                //}
                //driverService.HideCommandPromptWindow = hideTerminal;
                driverService = CreateDriverService(OpenQA.Selenium.Firefox.FirefoxDriverService.CreateDefaultService, "geckodriver.exe");

                webDriver = new OpenQA.Selenium.Firefox.FirefoxDriver((OpenQA.Selenium.Firefox.FirefoxDriverService)driverService, options);
            }
            else if (seleniumEngine == "ie")
            {
                //driverService = OpenQA.Selenium.IE.InternetExplorerDriverService.CreateDefaultService(driverPath);
                //driverService.HideCommandPromptWindow = hideTerminal;
                driverService = CreateDriverService(OpenQA.Selenium.IE.InternetExplorerDriverService.CreateDefaultService, "IEDriverServer.exe");

                webDriver = new OpenQA.Selenium.IE.InternetExplorerDriver((OpenQA.Selenium.IE.InternetExplorerDriverService)driverService, new OpenQA.Selenium.IE.InternetExplorerOptions());
            }
            else
            {
                throw new Exception("Strange Web Browser");
            }

            // add app instance
            //var instanceName = v_InstanceName.ExpandValueOrUserVariable(engine);
            //engine.AddAppInstance(instanceName, webDriver);
            this.CreateWebBrowserInstance(webDriver, profilePath, engine);

            //var instanceTracking = SelectionItemsControls.ExpandValueOrUserVariableAsSelectionItem(this, nameof(v_InstanceTracking), engine);
            //if (instanceTracking != "forget instance")
            //{
            //    GlobalAppInstances.AddInstance(instanceName, webDriver);
            //}

            var browserWindowOption = SelectionItemsControls.ExpandValueOrUserVariableAsSelectionItem(this, nameof(v_BrowserWindowOption), engine);
            if (browserWindowOption == "maximize")
            {
                webDriver.Manage().Window.Maximize();
            }

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

        public override void AfterShown(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            if (!editor.appSettings.ClientSettings.SupportIECommand)
            {
                var cmb = ControlsList.GetPropertyControl<ComboBox>(nameof(v_BrowserType));
                for (int i = cmb.Items.Count - 1; i >= 0; i--)
                {
                    if (cmb.Items[i].ToString() == "IE")
                    {
                        cmb.Items.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}