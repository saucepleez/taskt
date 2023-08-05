using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Instance")]
    [Attributes.ClassAttributes.CommandSettings("Get Web Browser Info")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate a Selenium web browser session to a given URL or resource.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to navigate an existing Selenium instance to a known URL or web resource")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserGetWebBrowserInfoCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Information Type")]
        [PropertyUISelectionOption("Window Title")]
        [PropertyUISelectionOption("Window URL")]
        [PropertyUISelectionOption("Current Handle ID")]
        [PropertyUISelectionOption("HTML Page Source")]
        [PropertyUISelectionOption("Handle ID List")]
        [InputSpecification("", true)]
        [SampleUsage("")]
        [PropertyValidationRule("Information Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Information Type")]
        public string v_InfoType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public SeleniumBrowserGetWebBrowserInfoCommand()
        {
            //this.CommandName = "SeleniumBrowserInfoCommand";
            //this.SelectionName = "Get Browser Info";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var seleniumInstance = SeleniumBrowserControls.GetSeleniumBrowserInstance(v_InstanceName, engine);

            var requestedInfo = this.GetUISelectionValue(nameof(v_InfoType), engine);
            string info = "";
            switch (requestedInfo)
            {
                case "window title":
                    info = seleniumInstance.Title;
                    break;
                case "window url":
                    info = seleniumInstance.Url;
                    break;
                case "current handle id":
                    info = seleniumInstance.CurrentWindowHandle;
                    break;
                case "html page source":
                    info = seleniumInstance.PageSource;
                    break;
                case "handle id list":
                    info = Newtonsoft.Json.JsonConvert.SerializeObject(seleniumInstance.WindowHandles);
                    break;
            }

            //store data
            info.StoreInUserVariable(sender, v_applyToVariableName);
        }
    }
}