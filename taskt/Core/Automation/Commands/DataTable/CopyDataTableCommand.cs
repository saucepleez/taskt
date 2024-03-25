﻿using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("DataTable Action")]
    [Attributes.ClassAttributes.CommandSettings("Copy DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to copy a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to copy a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CopyDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        [PropertyDescription("DataTable Variable Name to Copy")]
        [PropertyValidationRule("DataTable to Copy", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable to Copy")]
        public string v_TargetDataTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_NewOutputDataTableName))]
        public string v_NewDataTable { get; set; }

        public CopyDataTableCommand()
        {
            //this.CommandName = "CopyDataTableCommand";
            //this.SelectionName = "Copy DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            DataTable myDT = v_TargetDataTable.ExpandUserVariableAsDataTable(engine);

            DataTable newDT = new DataTable();
            foreach(DataColumn col in myDT.Columns)
            {
                newDT.Columns.Add(col.ColumnName);
            }

            foreach(DataRow row in myDT.Rows)
            {
                DataRow newRow = newDT.NewRow();
                for (int i = 0; i < myDT.Columns.Count; i++)
                {
                    newRow[i] = row[i];
                }
                newDT.Rows.Add(newRow);
            }

            newDT.StoreInUserVariable(engine, v_NewDataTable);
        }
    }
}