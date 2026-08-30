using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for exists folder and get folder path commands
    /// </summary>
    public abstract class AFolderExistsFolderPathPathResultCommands : AFolderExistsFolderPathCommands, IFolderExistsFolderPathPathResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPathResult))]
        [PropertyParameterOrder(20000)]
        public virtual string v_ResultPath { get; set; }
    }
}
