using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// execute some action to exists file and get file paths before/after action commands
    /// </summary>
    public abstract class AFileExistsFilePathBeforeAfterResultCommands : AFileExistsFilePathCommands, IFileExistsFilePathBeforeAfterPathResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        [PropertyDescription("Variable Name to Store File Path Before Command")]
        [PropertyParameterOrder(20000)]
        public virtual string v_BeforeFilePathResult { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        [PropertyDescription("Variable Name to Store File Path After Command")]
        [PropertyParameterOrder(21000)]
        public virtual string v_AfterFilePathResult { get; set; }
    }
}
