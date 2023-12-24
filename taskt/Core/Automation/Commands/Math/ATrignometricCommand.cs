using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Abstract Class for Trignometric Commands
    /// </summary>
    [Serializable]
    public abstract class ATrignometricCommand : AMathValueResultCommand
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        //public string v_Value { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(MathControls), nameof(MathControls.v_AngleType))]
        [PropertyParameterOrder(5500)]
        public string v_AngleType { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_OutputNumericalVariableName))]
        //public string v_Result { get; set; }
    }
}