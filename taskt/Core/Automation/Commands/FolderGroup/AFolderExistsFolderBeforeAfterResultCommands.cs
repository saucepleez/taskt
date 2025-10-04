using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for exists folder and get before/after process path commands
    /// </summary>
    public abstract class AFolderExistsFolderBeforeAfterResultCommands : AFolderExistsFolderPathCommands, IFolderExistsFolderPathBeforeAfterPathResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPathResult))]
        [PropertyDescription("Variable Name to Store Folder Path Before Process")]
        [PropertyParameterOrder(20000)]
        public virtual string v_BeforeFolderPathResult { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPathResult))]
        [PropertyDescription("Variable Name to Store Folder Path After Process")]
        [PropertyParameterOrder(21000)]
        public virtual string v_AfterFolderPathResult { get; set; }
    }
}
