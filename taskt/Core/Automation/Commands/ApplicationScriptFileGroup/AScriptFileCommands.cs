using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for specify script file commands
    /// </summary>
    public abstract class AScriptFileCommands : ScriptCommand, IFileExistsFilePathProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_NoSample_FilePath))]
        [PropertyParameterOrder(5000)]
        public virtual string v_TargetFilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        [PropertyParameterOrder(10000)]
        public virtual string v_WaitTimeForFile { get; set; }
    }
}
