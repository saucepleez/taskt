﻿using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert Row")]
    [Attributes.ClassAttributes.CommandSettings("Convert DataTable Row To DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Row to DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Row to DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertDataTableRowToDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        [PropertyDescription("DataTable Variable Name to Converted")]
        public string v_DataTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        public string v_DataRowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_NewOutputDataTableName))]
        public string v_Result { get; set; }

        public ConvertDataTableRowToDataTableCommand()
        {
            //this.CommandName = "ConvertDataTableRowToDataTableCommand";
            //this.SelectionName = "Convert DataTable Row To DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;         
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (var srcDT, var index) = this.ExpandUserVariablesAsDataTableAndRowIndex(nameof(v_DataTable), nameof(v_DataRowIndex), engine);

            DataTable myDT = new DataTable();

            int cols = srcDT.Columns.Count;
            myDT.Rows.Add();
            for (int i = 0; i < cols; i++)
            {
                myDT.Columns.Add(srcDT.Columns[i].ColumnName);
                myDT.Rows[0][i] = srcDT.Rows[index][i];
            }

            myDT.StoreInUserVariable(engine, v_Result);
        }
    }
}