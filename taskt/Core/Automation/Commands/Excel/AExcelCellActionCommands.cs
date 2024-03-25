﻿using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for commands that using Excel Cell get, set, etc...
    /// </summary>
    public abstract class AExcelCellActionCommands : AExcelCellCommands, IExcelCellActionProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        [PropertyParameterOrder(8000)]
        public virtual string v_ValueType { get; set; }
    }
}
