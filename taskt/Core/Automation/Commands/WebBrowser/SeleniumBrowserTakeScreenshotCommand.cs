using OpenQA.Selenium;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Actions")]
    [Attributes.ClassAttributes.Description("This command allows you to take a screenshot in Selenium web browser session.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to take a screenshot from the current displayed webpage within the web browser.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserTakeScreenshotCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the instance name (ex. myInstance, {{{vInstance}}})")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create Browser** command")]
        //[SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.WebBrowser)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
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
            this.CommandName = "SeleniumBrowserTakeScreenshotCommand";
            this.SelectionName = "Take Screenshot";
            //this.v_InstanceName = "";
            //this.v_SeleniumScreenshotPathParameter = "";
            //this.v_SeleniumScreenshotFileNameParameter = "screenshot_001";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            //var browserObject = engine.GetAppInstance(vInstance);
            //var seleniumInstance = (IWebDriver)browserObject;
            var seleniumInstance = v_InstanceName.GetSeleniumBrowserInstance(engine);

            var screenshotPath = v_SeleniumScreenshotPathParameter.ConvertToUserVariable(sender);
            var screenshotFileName = v_SeleniumScreenshotFileNameParameter.ConvertToUserVariable(sender);

            // take the screenshot
            Screenshot image = ((ITakesScreenshot)seleniumInstance).GetScreenshot();
            // save the screenshot to the entered folder by provided name for the screenshot file name
            image.SaveAsFile(screenshotPath + "/" + screenshotFileName + ".png", ScreenshotImageFormat.Png);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
        //    CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, PropertyInstanceType.InstanceType.WebBrowser);
        //    RenderedControls.AddRange(instanceCtrls);
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SeleniumScreenshotPathParameter", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SeleniumScreenshotFileNameParameter", this, editor));

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
        //    }

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InstanceName))
        //    {
        //        this.validationResult += "Instance name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_SeleniumScreenshotPathParameter))
        //    {
        //        this.validationResult += "Folder is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_SeleniumScreenshotFileNameParameter))
        //    {
        //        this.validationResult += "File name is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}