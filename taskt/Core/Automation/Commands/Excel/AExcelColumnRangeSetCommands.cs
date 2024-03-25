﻿using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    public abstract class AExcelColumnRangeSetCommands : AExcelColumnRangeCommands, IExcelColumnRangeSetProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_WhenItemNotEnough))]
        [PropertyParameterOrder(12000)]
        public abstract string v_WhenItemNotEnough { get; set; }
    }
}
