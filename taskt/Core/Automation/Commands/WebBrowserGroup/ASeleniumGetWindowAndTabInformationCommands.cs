using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for get Window And Tab information commands
    /// </summary>
    public abstract class ASeleniumGetWindowAndTabInformationCommands : ASeleniumGetFromWebDriverCommands
    {
        [XmlAttribute]
        [PropertyParameterOrder(6000)]
        public override abstract string v_Result { get; set; }
    }
}
