﻿using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Excel RC Location commands
    /// </summary>
    public abstract class AExcelRCLocationCommand : AExcelInstanceCommand, IExcelRCLocationProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowLocation))]
        [PropertyParameterOrder(6000)]
        public virtual string v_CellRow { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnLocation))]
        [PropertyParameterOrder(7000)]
        public virtual string v_CellColumn { get; set; }
    }
}