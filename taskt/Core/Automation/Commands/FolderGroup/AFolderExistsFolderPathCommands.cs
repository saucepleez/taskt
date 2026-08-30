using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for exists folder commands
    /// </summary>
    public abstract class AFolderExistsFolderPathCommands : AFolderFolderPathCommands, IFolderExistsFolderPathProperties
    {

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_WaitTime))]
        [PropertyParameterOrder(10000)]
        public virtual string v_WaitTimeForFolder { get; set; }
    }
}
