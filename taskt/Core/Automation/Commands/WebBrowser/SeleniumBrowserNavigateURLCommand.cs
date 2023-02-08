using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Navigate")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate a Selenium web browser session to a given URL or resource.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to navigate an existing Selenium instance to a known URL or web resource")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserNavigateURLCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the instance name (ex. myInstacne, {{{vInstance}}})")]
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
        [PropertyDescription("URL to navigate to")]
        [InputSpecification("URL", true)]
        //[SampleUsage("**https://mycompany.com/orders** or **{{{vURL}}}**")]
        [PropertyDetailSampleUsage("**https://mycompany.com/orders**", PropertyDetailSampleUsage.ValueType.Value, "URL")]
        [PropertyDetailSampleUsage("**{{{vURL}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "URL")]
        [Remarks("")]
        [PropertyValidationRule("URL", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "URL")]
        public string v_URL { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("HTTPS usage")]
        [InputSpecification("Please specify **True** or **False**")]
        //[SampleUsage("\"True\" to use HTTPS, \"False\" if you want to try HTTP instead")]
        [PropertyDetailSampleUsage("**True**", "Use **HTTPS** when no protocol is specified in the URL")]
        [PropertyDetailSampleUsage("**False**", "Use **HTTP** when no protocol is specified in the URL")]
        [PropertyUISelectionOption("True")]
        [PropertyUISelectionOption("False")]
        [PropertyIsOptional(true, "True")]
        [Remarks("Choose if you want to use HTTP or HTTPS for navigation. If no protocol is specified in the URL above, taskt will resort to this choice.")]
        [PropertyDisplayText(false, "")]
        public string v_UseHttps { get; set; }

        //private Dictionary<bool, string> v_HttpsChoice = new Dictionary<bool, string>();

        public SeleniumBrowserNavigateURLCommand()
        {
            this.CommandName = "SeleniumBrowserNavigateURLCommand";
            this.SelectionName = "Navigate to URL";
            this.v_InstanceName = "";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            //this.v_UseHttps = "True";
            //this.v_HttpsChoice.Add(true, "https://");
            //this.v_HttpsChoice.Add(false, "http://");
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var parsedURL = v_URL.ConvertToUserVariable(sender);
            if (!parsedURL.StartsWith("http"))
            {
                var useHttps = v_UseHttps.ConvertToUserVariableAsBool("Use HTTPS", engine);
                //parsedURL = this.v_HttpsChoice[useHttps] + parsedURL;
                parsedURL = ((useHttps) ? "https://" : "http://") + parsedURL;
            }

            //var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            //var browserObject = engine.GetAppInstance(vInstance);
            //var seleniumInstance = (OpenQA.Selenium.IWebDriver)browserObject;

            var seleniumInstance = v_InstanceName.GetSeleniumBrowserInstance(engine);

            seleniumInstance.Navigate().GoToUrl(parsedURL);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
        //    UI.CustomControls.CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebBrowser);
        //    RenderedControls.AddRange(instanceCtrls);
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_URL", this, editor));

        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_UseHttps", this, editor));

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
        //        this.v_UseHttps = "True";
        //    }

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    string url;
        //    if (String.IsNullOrEmpty(this.v_URL))
        //    {
        //        url = "";
        //    }
        //    else
        //    {
        //        url = v_URL;
        //    }
        //    if (!url.StartsWith("http"))
        //    {
        //        var useHttps = (v_UseHttps.Trim().ToLower() == "true");
        //        return base.GetDisplayValue() + " [URL: '" + v_HttpsChoice[useHttps] + v_URL + "', Instance Name: '" + v_InstanceName + "']";
        //    }
        //    else
        //    {
        //        return base.GetDisplayValue() + " [URL: '" + v_URL + "', Instance Name: '" + v_InstanceName + "']";
        //    }
            
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InstanceName))
        //    {
        //        this.validationResult += "Instance name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_URL))
        //    {
        //        this.validationResult += "URL is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}

    }
}