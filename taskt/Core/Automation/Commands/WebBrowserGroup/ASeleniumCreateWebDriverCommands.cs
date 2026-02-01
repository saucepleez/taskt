using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for create WebDriver (WebBrowser instance) commands
    /// </summary>
    public abstract class ASeleniumCreateWebDriverCommands : ASeleniumDoSomethingToWebDriverCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_OutputInstanceName))]
        //[PropertyParameterOrder(5000)]
        public override string v_InstanceName { get; set; }
    }
}
