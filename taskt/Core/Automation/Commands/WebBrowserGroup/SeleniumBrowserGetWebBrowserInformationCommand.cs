using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Instance")]
    [Attributes.ClassAttributes.CommandSettings("Get Web Browser Information")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Web Browser Information.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Web Browser Information.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetWebBrowserInformationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Information Type")]
        [PropertyUISelectionOption("Window Title")]
        [PropertyUISelectionOption("Window URL")]
        [PropertyUISelectionOption("Current Handle")]
        [PropertyUISelectionOption("HTML Page Source")]
        [PropertyUISelectionOption("Handles JSON Array")]
        [InputSpecification("", true)]
        [SampleUsage("")]
        [PropertyValidationRule("Information Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Information Type")]
        public string v_InfoType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public SeleniumBrowserGetWebBrowserInformationCommand()
        {
            //this.CommandName = "SeleniumBrowserInfoCommand";
            //this.SelectionName = "Get Browser Info";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var seleniumInstance = SeleniumBrowserControls.ExpandValueOrUserVariableAsSeleniumBrowserInstance(v_InstanceName, engine);

            var requestedInfo = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_InfoType), engine);
            string info = "";
            switch (requestedInfo)
            {
                case "window title":
                    info = seleniumInstance.Title;
                    break;
                case "window url":
                    info = seleniumInstance.Url;
                    break;
                case "current handle":
                    info = seleniumInstance.CurrentWindowHandle;
                    break;
                case "html page source":
                    info = seleniumInstance.PageSource;
                    break;
                case "handles json array":
                    info = Newtonsoft.Json.JsonConvert.SerializeObject(seleniumInstance.WindowHandles);
                    break;
            }

            //store data
            info.StoreInUserVariable(engine, v_applyToVariableName);
        }
    }
}