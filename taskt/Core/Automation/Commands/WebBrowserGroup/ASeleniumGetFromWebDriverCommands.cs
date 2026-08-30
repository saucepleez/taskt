using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// get from WebDriver commands
    /// </summary>
    public abstract class ASeleniumGetFromWebDriverCommands : ASeleniumDoSomethingToWebDriverCommands, IResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_InputInstanceName))]
        public override string v_InstanceName { get; set; }

        [XmlAttribute]
        public abstract string v_Result { get; set; }
    }
}
