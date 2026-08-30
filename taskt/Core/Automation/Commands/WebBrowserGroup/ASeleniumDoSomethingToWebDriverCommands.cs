using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for do something to WebDriver(WebBrowser instance) commands
    /// </summary>
    public abstract class ASeleniumDoSomethingToWebDriverCommands : ScriptCommand, ISeleniumDoSomethingToWebDriverProperties
    {
        [XmlAttribute]
        [PropertyParameterOrder(5000)]
        public abstract string v_InstanceName { get; set; }
    }
}
