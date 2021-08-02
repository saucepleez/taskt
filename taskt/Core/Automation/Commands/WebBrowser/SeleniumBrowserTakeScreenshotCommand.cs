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
    [Attributes.ClassAttributes.SubGruop("Actions")]
    [Attributes.ClassAttributes.Description("This command allows you to take a screenshot in Selenium web browser session.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to take a screenshot from the current displayed webpage within the web browser.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserTakeScreenshotCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name (ex. myInstance, {{{vInstance}}})")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Browser** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        public string v_InstanceName { get; set; }


        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define folder where the screenshot should be stored (ex. C:\\screenshots, {{{vPath}}})")]
        [Attributes.PropertyAttributes.InputSpecification("Enter folder path or select folder from the list to define where the screenshot should be stored")]
        [Attributes.PropertyAttributes.SampleUsage("**C:\\screenshots\\** or **{{{vPath}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_SeleniumScreenshotPathParameter { get; set; }


        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define the screenshot file name (no extension needed) (ex. screenshot_001, {{{vName}}})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter file name for the screenshot")]
        [Attributes.PropertyAttributes.SampleUsage("**screenshot_001** or **{{{vName}}}**")]
        [Attributes.PropertyAttributes.Remarks("png image")]
        public string v_SeleniumScreenshotFileNameParameter { get; set; }

        public SeleniumBrowserTakeScreenshotCommand()
        {
            this.CommandName = "SeleniumBrowserTakeScreenshotCommand";
            this.SelectionName = "Take Screenshot";
            this.v_InstanceName = "";
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

            if (editor.creationMode == frmCommandEditor.CreationMode.Add)
            {
                this.v_InstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
            }

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InstanceName))
            {
                this.validationResult += "Instance name is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_SeleniumScreenshotPathParameter))
            {
                this.validationResult += "Folder is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_SeleniumScreenshotFileNameParameter))
            {
                this.validationResult += "File name is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}