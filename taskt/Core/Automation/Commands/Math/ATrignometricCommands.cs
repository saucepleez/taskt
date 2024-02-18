using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Abstract Class for Trignometric Commands
    /// </summary>
    [Serializable]
    public abstract class ATrignometricCommands : AMathValueResultCommands, ITrignometricProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(MathControls), nameof(MathControls.v_AngleType))]
        [PropertyParameterOrder(5500)]
        public virtual string v_AngleType { get; set; }
    }
}