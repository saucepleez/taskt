﻿using System;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Cell")]
    [Attributes.ClassAttributes.CommandSettings("Go To Cell")]
    [Attributes.ClassAttributes.Description("This command moves to a specific cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to move to a new cell from your currently selected cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGoToCellCommand : AExcelCellCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_CellRangeLocation))]
        //[PropertyParameterOrder(6000)]
        //public string v_CellLocation { get; set; }

        public ExcelGoToCellCommand()
        {
            //this.CommandName = "ExcelGoToCellCommand";
            //this.SelectionName = "Go To Cell";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var excelInstance, var excelSheet) = v_InstanceName.ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(engine);

            //var rg = v_CellLocation.GetExcelRange(engine, excelInstance, excelSheet, this);

            var rg = this.ExpandValueOrVariableAsExcelSingleCellLocation(engine);

            rg.Select();
        }
    }
}