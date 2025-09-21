using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// execute some action to exists file commands
    /// </summary>
    public abstract class AFileExistsFilePathCommands : AFileFilePathCommands, IFileExistsFilePathProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        [PropertyParameterOrder(10000)]
        public virtual string v_WaitTimeForFile { get; set; }
    }
}
