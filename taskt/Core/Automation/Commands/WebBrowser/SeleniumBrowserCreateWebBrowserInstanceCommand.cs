using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Instance")]
    [Attributes.ClassAttributes.CommandSettings("Create Web Browser Instance")]
    [Attributes.ClassAttributes.Description("This command allows you to create a new Selenium web browser session which enables automation for websites.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create a browser that will eventually perform web automation such as checking an internal company intranet site to retrieve data.\nIf this command does not work, please check your browser version, and WebDriver version.\nYou can check the WebDriver version with \"foo.exe -v\" in command prompt.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserCreateWebBrowserInstanceCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyTextBoxSetting(1, false)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Instance Tracking (After task ends)")]
        [PropertyUISelectionOption("Forget Instance")]
        [PropertyUISelectionOption("Keep Instance Alive")]
        [InputSpecification("Specify if taskt should remember this instance name after the script has finished executing.")]
        //[SampleUsage("Select **Forget Instance** to  or **Keep Instance Alive** to allow subsequent tasks to call the instance by name.")]
        [PropertyDetailSampleUsage("**Forget Instance**", "Forget the instance After tasks ends")]
        [PropertyDetailSampleUsage("**Keep Instance Alive**", "Allow subsequent tasks to call the instance by name")]
        [Remarks("Calling the **Close Browser** command or ending the browser session will end the instance.  This command only works during the lifetime of the application.  If the application is closed, the references will be forgetten automatically.")]
        [PropertyIsOptional(true, "Forget Instance")]
        [PropertyDisplayText(false, "")]
        public string v_InstanceTracking { get; set; }

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
        [PropertyDisplayText(false, "")]
        public string v_BrowserWindowOption { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("We bBrowser Command Line Options")]
        [InputSpecification("Command Line Options", true)]
        [SampleUsage("user-data-dir=c:\\users\\public\\SeleniumTasktProfile")]
        [Remarks("")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "")]
        public string v_SeleniumOptions { get; set; }

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
        public string v_EngineType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Window Handle")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Window Handle", PropertyValidationRule.ValidationRuleFlags.None)]
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
        public string v_WebDriverPath { get; set; }


        // fields
        private bool supportIECommands;

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
            var seleniumEngine = SelectionItemsControls.ExpandValueOrUserVariableAsSelectionItem(this, nameof(v_EngineType), engine);

            var driverPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "Resources");
            var browserPath = v_BrowserPath.ExpandValueOrUserVariable(engine);
            var webDriverPath = v_WebDriverPath.ExpandValueOrUserVariable(engine);

            OpenQA.Selenium.DriverService driverService;
            OpenQA.Selenium.IWebDriver webDriver;
            if (seleniumEngine == "chrome")
            {
                OpenQA.Selenium.Chrome.ChromeOptions options = new OpenQA.Selenium.Chrome.ChromeOptions();
                if (!string.IsNullOrEmpty(browserPath))
                {
                    options.BinaryLocation = browserPath;
                }

                if (!string.IsNullOrEmpty(v_SeleniumOptions))
                {
                    var convertedOptions = v_SeleniumOptions.ExpandValueOrUserVariable(engine);
                    options.AddArguments(convertedOptions);
                }

                if (!string.IsNullOrEmpty(webDriverPath))
                {
                    driverService = OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService(System.IO.Path.GetDirectoryName(webDriverPath), System.IO.Path.GetFileName(webDriverPath));
                }
                else
                {
                    driverService = OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService(driverPath);
                }
                
                webDriver = new OpenQA.Selenium.Chrome.ChromeDriver((OpenQA.Selenium.Chrome.ChromeDriverService)driverService, options);
            }
            else if (seleniumEngine == "edge")
            {
                OpenQA.Selenium.Edge.EdgeOptions options = new OpenQA.Selenium.Edge.EdgeOptions();

                if (!string.IsNullOrEmpty(webDriverPath))
                {
                    driverService = OpenQA.Selenium.Edge.EdgeDriverService.CreateDefaultService(System.IO.Path.GetDirectoryName(webDriverPath), System.IO.Path.GetFileName(webDriverPath));
                }
                else
                {
                    driverService = OpenQA.Selenium.Edge.EdgeDriverService.CreateDefaultService(driverPath, "msedgedriver.exe");
                }
                
                webDriver = new OpenQA.Selenium.Edge.EdgeDriver((OpenQA.Selenium.Edge.EdgeDriverService)driverService, options);
            }
            else if (seleniumEngine == "firefox")
            {
                OpenQA.Selenium.Firefox.FirefoxOptions options = new OpenQA.Selenium.Firefox.FirefoxOptions();
                if (!string.IsNullOrEmpty(browserPath))
                {
                    options.BrowserExecutableLocation = browserPath;
                }
                else
                {
                    options.BrowserExecutableLocation = @"c:\Program Files\Mozilla Firefox\firefox.exe";
                }

                if (!string.IsNullOrEmpty(webDriverPath))
                {
                    driverService = OpenQA.Selenium.Firefox.FirefoxDriverService.CreateDefaultService(System.IO.Path.GetDirectoryName(webDriverPath), System.IO.Path.GetFileName(webDriverPath));
                }
                else
                {
                    driverService = OpenQA.Selenium.Firefox.FirefoxDriverService.CreateDefaultService(driverPath);
                }
                
                webDriver = new OpenQA.Selenium.Firefox.FirefoxDriver((OpenQA.Selenium.Firefox.FirefoxDriverService)driverService, options);
            }
            else if (seleniumEngine == "ie")
            {
                driverService = OpenQA.Selenium.IE.InternetExplorerDriverService.CreateDefaultService(driverPath);
                webDriver = new OpenQA.Selenium.IE.InternetExplorerDriver((OpenQA.Selenium.IE.InternetExplorerDriverService)driverService, new OpenQA.Selenium.IE.InternetExplorerOptions());
            }
            else
            {
                throw new Exception("strange Web Browser");
            }

            //add app instance
            var instanceName = v_InstanceName.ExpandValueOrUserVariable(engine);
            engine.AddAppInstance(instanceName, webDriver);

            var instanceTracking = SelectionItemsControls.ExpandValueOrUserVariableAsSelectionItem(this, nameof(v_InstanceTracking), engine);
            if (instanceTracking != "forget instance")
            {
                GlobalAppInstances.AddInstance(instanceName, webDriver);
            }

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
                var whnd = WindowNameControls.ConvertProcessIdToWinHandle(procId);
                whnd.ToInt32().StoreInUserVariable(engine, v_Handle);
            }

            //var whnd = WindowNameControls.ConvertPidToWinHandle(driverService.ProcessId);
            //int x = 1;

            //var query = "select processid from win32_process where parentprocessid = " + driverService.ProcessId;
            //using (var searcher = new ManagementObjectSearcher(query))
            //{
            //    foreach (var obj in searcher.Get())
            //    {
            //        var prop = obj.Properties;
            //        var str = "";
            //        foreach(var p in prop)
            //        {
            //            str += p.Name + ", ";
            //        }

            //        Console.WriteLine("ChildProc: " + str + " : ProcID: " + obj.Properties["ProcessId"].Value);
            //    }
            //}

            //var query = "select processid from win32_process where parentprocessid = " + driverService.ProcessId;
            //using (var searcher = new ManagementObjectSearcher(query))
            //{
            //    var mos = searcher.Get();
            //    if (mos.Count >= 2)
            //    {
            //        int idx = 0;
            //        foreach(var m in mos)
            //        {
            //            if (idx == 1)
            //            {
            //                var p = int.Parse(m.Properties["ProcessId"].Value.ToString());
            //                IntPtr whnd = (IntPtr)0;
            //                if (seleniumEngine == "firefox")
            //                {
            //                    var query2 = "select processid from win32_process where parentprocessid = " + p;
            //                    using (var searcher2 = new ManagementObjectSearcher(query2))
            //                    {
            //                        var mos2 = searcher2.Get();
            //                        if (mos2.Count >= 1)
            //                        {
            //                            int idx2 = 0;
            //                            foreach(var o in mos2)
            //                            {
            //                                p = int.Parse(o.Properties["ProcessId"].Value.ToString());
            //                                whnd = WindowNameControls.ConvertPidToWinHandle(p);
            //                                break;
            //                            }
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    whnd = WindowNameControls.ConvertPidToWinHandle(p);
            //                }
                            
            //                Console.WriteLine("atta! Pid: " + p + ", whnd: " + whnd);
            //            }
            //            idx++;
            //        }
            //    }
            //    else
            //    {
            //        throw new Exception("Error");
            //    }
            //}

            //int x = 1;
        }

        public override void AfterShown(UI.Forms.frmCommandEditor editor)
        {
            if (!editor.appSettings.ClientSettings.SupportIECommand)
            {
                var cmd = ControlsList.GetPropertyControl<ComboBox>(nameof(v_EngineType));
                for (int i = cmd.Items.Count - 1; i >= 0; i--)
                {
                    if (cmd.Items[i].ToString() == "IE")
                    {
                        cmd.Items.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}