﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Column Action")]
    [Attributes.ClassAttributes.CommandSettings("Delete DataTable Column")]
    [Attributes.ClassAttributes.Description("This command allows you to delete a column to a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to delete a column to a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class DeleteDataTableColumnCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        public string v_DataTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnType))]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnNameIndex))]
        public string v_DeleteColumnName { get; set; }

        public DeleteDataTableColumnCommand()
        {
            //this.CommandName = "DeleteDataTableColumnCommand";
            //this.SelectionName = "Delete DataTable Column";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (var myDT, var colIndex) = this.ExpandUserVariablesAsDataTableAndColumnIndex(nameof(v_DataTable), nameof(v_ColumnType), nameof(v_DeleteColumnName), engine);

            myDT.Columns.RemoveAt(colIndex);
        }
    }
}