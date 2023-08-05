using OpenQA.Selenium;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Web Browser Actions")]
    [Attributes.ClassAttributes.CommandSettings("Take Screenshot")]
    [Attributes.ClassAttributes.Description("This command allows you to take a screenshot in Selenium web browser session.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to take a screenshot from the current displayed webpage within the web browser.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserTakeScreenshotCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Folder where the screenshot should be stored")]
        [InputSpecification("Folder Path", true)]
        //[SampleUsage("**C:\\screenshots\\** or **{{{vPath}}}**")]
        [PropertyDetailSampleUsage("**C:\\screenshots**", PropertyDetailSampleUsage.ValueType.Value, "Folder")]
        [PropertyDetailSampleUsage("**{{{vFolder}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Folder")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [PropertyValidationRule("Folder", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Folder")]
        public string v_SeleniumScreenshotPathParameter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Screenshot File Name (no extension needed)")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("File Name for the Screenshot", true)]
        //[SampleUsage("**screenshot_001** or **{{{vName}}}**")]
        [PropertyDetailSampleUsage("**screenshot_001**", PropertyDetailSampleUsage.ValueType.Value, "File Name")]
        [PropertyDetailSampleUsage("**{{{vFileName}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Name")]
        [Remarks("PNG Image saved")]
        [PropertyFirstValue("screenshot_001")]
        [PropertyValidationRule("File Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "File Name")]
        public string v_SeleniumScreenshotFileNameParameter { get; set; }

        public SeleniumBrowserTakeScreenshotCommand()
        {
            //this.CommandName = "SeleniumBrowserTakeScreenshotCommand";
            //this.SelectionName = "Take Screenshot";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_InstanceName = "";
            //this.v_SeleniumScreenshotPathParameter = "";
            //this.v_SeleniumScreenshotFileNameParameter = "screenshot_001";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var seleniumInstance = v_InstanceName.GetSeleniumBrowserInstance(engine);

            var screenshotPath = v_SeleniumScreenshotPathParameter.ConvertToUserVariable(sender);
            var screenshotFileName = v_SeleniumScreenshotFileNameParameter.ConvertToUserVariable(sender);

            // take the screenshot
            Screenshot image = ((ITakesScreenshot)seleniumInstance).GetScreenshot();
            // save the screenshot to the entered folder by provided name for the screenshot file name
            image.SaveAsFile(screenshotPath + "/" + screenshotFileName + ".png", ScreenshotImageFormat.Png);
        }
    }
}