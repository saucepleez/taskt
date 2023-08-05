using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Row Action")]
    [Attributes.ClassAttributes.CommandSettings("Add DataTable Rows By DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to add a DataTable Row to a DataTable by a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a DataTable Row to a DataTable by a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class AddDataTableRowsByDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        [PropertyDescription("DataTable Variable Name to be added a row")]
        [PropertyValidationRule("DataTable to be added", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable to be added")]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        [PropertyDescription("DataTable Variable Name to add to the DataTable")]
        [PropertyValidationRule("DataTable to add", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable to add")]
        public string v_RowName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_WhenColumnNotExists))]
        [PropertyDescription("When DataTable (to add) Column does not Exists")]
        public string v_NotExistsKey { get; set; }

        public AddDataTableRowsByDataTableCommand()
        {
            //this.CommandName = "AddDataTableRowByDataTableCommand";
            //this.SelectionName = "Add DataTable Rows By DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            DataTable myDT = v_DataTableName.GetDataTableVariable(engine);

            DataTable addDT = v_RowName.GetDataTableVariable(engine);

            string notExistsKey = this.GetUISelectionValue(nameof(v_NotExistsKey), "Key Does Not Exists", engine);

            // get columns list
            List<string> columns = myDT.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            if (notExistsKey == "error")
            {
                for (int i = 0; i < addDT.Columns.Count; i++)
                {
                    if (!columns.Contains(addDT.Columns[i].ColumnName))
                    {
                        throw new Exception("Column name " + addDT.Columns[i].ColumnName + " does not exists");
                    }
                }
            }
            for (int i = 0; i < addDT.Rows.Count; i++)
            {
                DataRow row = myDT.NewRow();
                for (int j = 0; j < addDT.Columns.Count; j++)
                {
                    if (columns.Contains(addDT.Columns[j].ColumnName))
                    {
                        row[addDT.Columns[j].ColumnName] = addDT.Rows[i][addDT.Columns[j].ColumnName];
                    }
                }
                myDT.Rows.Add(row);
            }
        }
    }
}