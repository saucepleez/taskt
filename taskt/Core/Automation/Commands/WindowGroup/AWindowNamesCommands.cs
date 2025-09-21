using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for window names commands
    /// </summary>
    public abstract class AWindowNamesCommands : AWindowNameCoreCommands, IWindowNamesProperties, ICanHandleList
    {
        /// <summary>
        /// found window names list
        /// </summary>
        [XmlAttribute]
        [PropertyDescription("Variable Name to Store Window Names List")]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List, false)]
        public override string v_NameResult { get; set; }

        /// <summary>
        /// found window handles list
        /// </summary>
        [XmlAttribute]
        [PropertyDescription("Variable Name to Store Window Handles List")]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List, false)]
        public override string v_HandleResult { get; set; }

        // todo: add sort order?
    }
}
