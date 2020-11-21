using OpenQA.Selenium;
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
    [Attributes.ClassAttributes.Description("This command allows you to take a screenshot in Selenium web browser session.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to take a screenshot from the current displayed webpage within the web browser.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserTakeScreenshotCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Browser** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }


        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define where the screenshot should be stored")]
        [Attributes.PropertyAttributes.InputSpecification("Enter folder path or select folder from the list to define where the screenshot should be stored")]
        [Attributes.PropertyAttributes.SampleUsage("C:/screenshots/")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_SeleniumScreenshotPathParameter { get; set; }


        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define the screenshot file name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter file name for the screenshot")]
        [Attributes.PropertyAttributes.SampleUsage("screenshot_001")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SeleniumScreenshotFileNameParameter { get; set; }

        public SeleniumBrowserTakeScreenshotCommand()
        {
            this.CommandName = "SeleniumBrowserTakeScreenshotCommand";
            this.SelectionName = "Take Screenshot";
            this.v_InstanceName = "default";
            this.v_SeleniumScreenshotPathParameter = "";
            this.v_SeleniumScreenshotFileNameParameter = "screenshot_001";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var browserObject = engine.GetAppInstance(vInstance);
            var seleniumInstance = (OpenQA.Selenium.IWebDriver)browserObject;

            var screenshotPath = v_SeleniumScreenshotPathParameter.ConvertToUserVariable(sender);
            var screenshotFileName = v_SeleniumScreenshotFileNameParameter.ConvertToUserVariable(sender);

            // take the screenshot
            Screenshot image = ((ITakesScreenshot)seleniumInstance).GetScreenshot();
            // save the screenshot to the entered folder by provided name for the screenshot file name
            image.SaveAsFile(screenshotPath + "/" + screenshotFileName + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SeleniumScreenshotPathParameter", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SeleniumScreenshotFileNameParameter", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
}