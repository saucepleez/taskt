using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for do something to webelement commands
    /// </summary>
    public abstract class ASeleniumDoSomethingToWebElementCommands : ScriptCommand, ISeleniumDoSomethingToWebElementProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_InputWebElementName))]
        public string v_WebElement { get; set; }
    }
}
