﻿using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    public abstract class AExcelRowRangeSetCommand : AExcelRowRangeCommand, IExcelRowRangeSetProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_WhenItemNotEnough))]
        [PropertyDescription("When DataTable Items Not Enough")]
        [PropertyParameterOrder(12000)]
        public string v_WhenItemNotEnough { get; set; }
    }
}
