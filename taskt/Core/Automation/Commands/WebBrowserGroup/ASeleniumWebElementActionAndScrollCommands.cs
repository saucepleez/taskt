using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for WebElement Action supports scroll to WebElement commands
    /// </summary>
    public abstract class ASeleniumWebElementActionAndScrollCommands : ASeleniumWebElementActionCommands, ISeleniumWebElementActionAndScrollProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_ScrollToWebElement))]
        [PropertyParameterOrder(11000)]
        public string v_ScrollToWebElement { get; set; }
    }
}
