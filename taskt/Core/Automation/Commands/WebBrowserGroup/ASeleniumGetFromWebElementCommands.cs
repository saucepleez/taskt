using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for get value(s) from WebElement commands
    /// </summary>
    public abstract class ASeleniumGetFromWebElementCommands : ASeleniumWebElementActionAndScrollCommands, ISeleniumGetFromWebElementProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_WhenValueCanNotRetrieved))]
        [PropertyParameterOrder(12000)]
        public string v_WhenValueCanNotRetrieved { get; set; }
    }
}
