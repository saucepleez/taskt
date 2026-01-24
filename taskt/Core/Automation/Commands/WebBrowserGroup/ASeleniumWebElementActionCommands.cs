using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for WebElement action commands
    /// </summary>
    public abstract class ASeleniumWebElementActionCommands : ASeleniumDoSomethingToWebElementCommands, ISeleniumWebElementActionProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_WhenFailAction))]
        [PropertyParameterOrder(10000)]
        public string v_WhenFailAction { get; set; }
    }
}
