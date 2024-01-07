using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// abstruct command class for Math commands. only Value, Result parameters
    /// </summary>
    [Serializable]
    public abstract class AMathValueResultCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        [PropertyParameterOrder(5000)]
        public virtual string v_Value { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_OutputNumericalVariableName))]
        [PropertyParameterOrder(6000)]
        public virtual string v_Result { get; set; }
    }
}