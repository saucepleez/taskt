using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Navigate")]
    [Attributes.ClassAttributes.CommandSettings("Navigate to URL")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate a Selenium web browser session to a given URL or resource.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to navigate an existing Selenium instance to a known URL or web resource")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserNavigateURLCommand : ScriptCommand
    {
        [XmlAttribute]
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

        public SeleniumBrowserNavigateURLCommand()
        {
            //this.CommandName = "SeleniumBrowserNavigateURLCommand";
            //this.SelectionName = "Navigate to URL";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
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

            var seleniumInstance = v_InstanceName.GetSeleniumBrowserInstance(engine);

            seleniumInstance.Navigate().GoToUrl(parsedURL);
        }
    }
}