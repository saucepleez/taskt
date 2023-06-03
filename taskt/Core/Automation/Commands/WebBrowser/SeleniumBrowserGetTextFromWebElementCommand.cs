using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("WebElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Get Text From WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Text Value from WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Text Value from WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserGetTextFromWebElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        public SeleniumBrowserGetTextFromWebElementCommand()
        {
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var elem = v_WebElement.ConvertToUserVariableAsWebElement("WebElement", engine);

            var v = elem.Text;

            if (string.IsNullOrEmpty(v))
            {
                v = elem.GetAttribute("textContent");
            }
            if (string.IsNullOrEmpty(v) && (elem.TagName.ToLower() == "input"))
            {
                v = elem.GetAttribute("value");
            }

            v.StoreInUserVariable(engine, v_Result);
        }
    }
}