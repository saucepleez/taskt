using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// folder path commands
    /// </summary>
    public abstract class AFolderFolderPathCommands : ScriptCommand, IFolderPathProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        [PropertyParameterOrder(5000)]
        public virtual string v_TargetFolderPath { get; set; }
    }
}
