using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.App;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Web Browser Commands")]
    [Description("This command creates a new Selenium web browser session which enables automation for websites.")]

    public class SeleniumCreateBrowserCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Browser Instance Name")]
        [InputSpecification("Enter a unique name that will represent the application instance.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("This unique name allows you to refer to the instance by name in future commands, " +
                 "ensuring that the commands you specify run against the correct application.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Browser Engine Type")]
        [PropertyUISelectionOption("Chrome")]
        [PropertyUISelectionOption("IE")]
        [InputSpecification("Select the browser engine to execute the Selenium automation with.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_EngineType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Instance Tracking")]
        [PropertyUISelectionOption("Forget Instance")]
        [PropertyUISelectionOption("Keep Instance Alive")]
        [InputSpecification("Select **Forget Instance** to forget the instance after execution finishes, " +
                            "or **Keep Instance Alive** to allow subsequent tasks to call the instance by name.")]
        [SampleUsage("")]
        [Remarks("Calling the **Close Browser** command or ending the browser session will end the instance. " +
                 "This command only works during the lifetime of the application. " +
                 "If the application is closed, the references will be forgotten automatically.")]
        public string v_InstanceTracking { get; set; }

        [XmlAttribute]
        [PropertyDescription("Window State")]
        [PropertyUISelectionOption("Normal")]
        [PropertyUISelectionOption("Maximize")]
        [InputSpecification("Select the window state that the browser should start up with.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_BrowserWindowOption { get; set; }

        [XmlAttribute]
        [PropertyDescription("Selenium Command Line Options")]
        [InputSpecification("Select options to be passed to the Selenium command.")]
        [SampleUsage("user-data-dir=c:\\users\\public\\SeleniumTasktProfile || {vOptions}")]
        [Remarks("This input is optional.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SeleniumOptions { get; set; }

        public SeleniumCreateBrowserCommand()
        {
            CommandName = "SeleniumCreateBrowserCommand";
            SelectionName = "Create Browser";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultBrowser";
            v_InstanceTracking = "Forget Instance";
            v_BrowserWindowOption = "Maximize";
            v_EngineType = "Chrome";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var driverPath =Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Resources");

            DriverService driverService;
            IWebDriver webDriver;

            if (v_EngineType == "Chrome")
            {
                ChromeOptions options = new ChromeOptions();

                if (!string.IsNullOrEmpty(v_SeleniumOptions))
                {
                    var convertedOptions = v_SeleniumOptions.ConvertToUserVariable(sender);
                    options.AddArguments(convertedOptions);
                }

                driverService = ChromeDriverService.CreateDefaultService(driverPath);
                webDriver = new ChromeDriver((ChromeDriverService)driverService, options);
            }
            else
            {
                driverService = InternetExplorerDriverService.CreateDefaultService(driverPath);
                webDriver = new InternetExplorerDriver((InternetExplorerDriverService)driverService, new InternetExplorerOptions());
            }

            //add app instance
            engine.AddAppInstance(v_InstanceName, webDriver);

            //handle app instance tracking
            if (v_InstanceTracking == "Keep Instance Alive")
                GlobalAppInstances.AddInstance(v_InstanceName, webDriver);

            //handle window type on startup - https://github.com/saucepleez/taskt/issues/22
            switch (v_BrowserWindowOption)
            {
                case "Maximize":
                    webDriver.Manage().Window.Maximize();
                    break;
                case "Normal":
                case "":
                default:
                    break;
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_EngineType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_InstanceTracking", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_BrowserWindowOption", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SeleniumOptions", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return $"Create {v_EngineType} Browser [Instance Name '{v_InstanceName}']";
        }
    }
}
