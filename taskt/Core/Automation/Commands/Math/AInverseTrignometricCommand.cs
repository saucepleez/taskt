﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// abstract class for Inverse Trignometric Commands
    /// </summary>
    [Serializable]
    public abstract class AInverseTrignometricCommand : AMathValueResultCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(MathControls), nameof(MathControls.v_AngleType))]
        [PropertyParameterOrder(7000)]
        public string v_AngleType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(MathControls), nameof(MathControls.v_WhenValueIsOutOfRange))]
        [PropertyParameterOrder(8000)]
        public string v_WhenValueIsOutOfRange { get; set; }
    }
}