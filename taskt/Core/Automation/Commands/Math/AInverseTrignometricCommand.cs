using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// abstract class for Inverse Trignometric Commands
    /// </summary>
    [Serializable]
    public abstract class AInverseTrignometricCommand : ATrignometricCommand, IInverseTrignometricProperties
    {
        [XmlAttribute]
        [PropertyParameterOrder(7000)]
        public override string v_AngleType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(MathControls), nameof(MathControls.v_WhenValueIsOutOfRange))]
        [PropertyParameterOrder(8000)]
        public virtual string v_WhenValueIsOutOfRange { get; set; }
    }
}