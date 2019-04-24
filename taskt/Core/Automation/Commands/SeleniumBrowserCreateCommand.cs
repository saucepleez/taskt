using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to create a new Selenium web browser session which enables automation for websites.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create a browser that will eventually perform web automation such as checking an internal company intranet site to retrieve data")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserCreateCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Signifies a unique name that will represemt the application instance.  This unique name allows you to refer to the instance by name in future commands, ensuring that the commands you specify run against the correct application.")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Instance Tracking (after task ends)")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Forget Instance")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Keep Instance Alive")]
        [Attributes.PropertyAttributes.InputSpecification("Specify if taskt should remember this instance name after the script has finished executing.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Forget Instance** to forget the instance or **Keep Instance Alive** to allow subsequent tasks to call the instance by name.")]
        [Attributes.PropertyAttributes.Remarks("Calling the **Close Browser** command or ending the browser session will end the instance.  This command only works during the lifetime of the application.  If the application is closed, the references will be forgetten automatically.")]
        public string v_InstanceTracking { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select a Window State")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Normal")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Maximize")]
        [Attributes.PropertyAttributes.InputSpecification("Select the window state that the browser should start up with.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Normal** to start the browser in normal mode or **Maximize** to start the browser in maximized mode.")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_BrowserWindowOption { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select a Browser Engine Type")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Chrome")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("IE")]
        [Attributes.PropertyAttributes.InputSpecification("Select the window state that the browser should start up with.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Normal** to start the browser in normal mode or **Maximize** to start the browser in maximized mode.")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_EngineType { get; set; }

        public SeleniumBrowserCreateCommand()
        {
            this.CommandName = "SeleniumBrowserCreateCommand";
            this.SelectionName = "Create Browser";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_EngineType = "Chrome";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var driverPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "Resources");
            var seleniumEngine = v_EngineType.ConvertToUserVariable(sender);
            var instanceName = v_InstanceName.ConvertToUserVariable(sender);

            OpenQA.Selenium.DriverService driverService;
            OpenQA.Selenium.IWebDriver webDriver;

            if (seleniumEngine == "Chrome")
            {
                driverService = OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService(driverPath);
                webDriver = new OpenQA.Selenium.Chrome.ChromeDriver((OpenQA.Selenium.Chrome.ChromeDriverService)driverService, new OpenQA.Selenium.Chrome.ChromeOptions());          
            }
            else
            {
                driverService = OpenQA.Selenium.IE.InternetExplorerDriverService.CreateDefaultService(driverPath);
                webDriver = new OpenQA.Selenium.IE.InternetExplorerDriver((OpenQA.Selenium.IE.InternetExplorerDriverService)driverService, new OpenQA.Selenium.IE.InternetExplorerOptions());            
            }


            //add app instance
            engine.AddAppInstance(instanceName, webDriver);


            //handle app instance tracking
            if (v_InstanceTracking == "Keep Instance Alive")
            {
                GlobalAppInstances.AddInstance(instanceName, webDriver);
            }

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

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return "Create " + v_EngineType + " Browser - [Instance Name: '" + v_InstanceName + "', Instance Tracking: " + v_InstanceTracking + "]";
        }
    }
}