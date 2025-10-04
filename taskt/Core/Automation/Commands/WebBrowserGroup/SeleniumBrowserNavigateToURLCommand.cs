using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Navigate")]
    [Attributes.ClassAttributes.CommandSettings("Navigate To URL")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate a Selenium web browser session to a given URL or resource.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to navigate an existing Selenium instance to a known URL or web resource")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserNavigateToURLCommand : ScriptCommand
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

        public SeleniumBrowserNavigateToURLCommand()
        {
            //this.CommandName = "SeleniumBrowserNavigateURLCommand";
            //this.SelectionName = "Navigate to URL";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_UseHttps = "True";
            //this.v_HttpsChoice.Add(true, "https://");
            //this.v_HttpsChoice.Add(false, "http://");
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var parsedURL = v_URL.ExpandValueOrUserVariable(engine);
            if (!parsedURL.StartsWith("http"))
            {
                // check Edge/Chrome/Firefox special
                if (!parsedURL.StartsWith("edge://") &&
                    !parsedURL.StartsWith("chrome://") &&
                    !parsedURL.StartsWith("about:"))
                {
                    if (string.IsNullOrEmpty(v_UseHttps))
                    {
                        v_UseHttps = "True";
                    }
                    var useHttps = v_UseHttps.ExpandValueOrUserVariableAsBool("Use HTTPS", engine);
                    parsedURL = ((useHttps) ? "https://" : "http://") + parsedURL;
                }
            }

            var seleniumInstance = v_InstanceName.ExpandValueOrUserVariableAsSeleniumBrowserInstance(engine);

            seleniumInstance.Navigate().GoToUrl(parsedURL);
        }
    }
}