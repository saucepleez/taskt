using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// execute some action to exists file and get file path commands
    /// </summary>
    public abstract class AFileExistsFilePathPathResultCommands : AFileExistsFilePathCommands, IFileExistsFilePathPathResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        [PropertyParameterOrder(20000)]
        public virtual string v_ResultPath { get; set; }
    }
}
