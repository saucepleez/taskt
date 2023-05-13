using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Row Action")]
    [Attributes.ClassAttributes.CommandSettings("Set DataTable Row Values By DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to set a DataTable Row values to a DataTable by a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a DataTable Row values to a DataTable by a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetDataTableRowValuesByDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        [PropertyDescription("DataTable Variable Name to be Setted")]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        [PropertyDescription(" Row Index to be Setted")]
        public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        [PropertyDescription("DataTable Variable Name to Set")]
        public string v_RowName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        [PropertyDescription(" Row Index to Set")]
        public string v_SrcRowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_WhenColumnNotExists))]
        public string v_NotExistsKey { get; set; }

        public SetDataTableRowValuesByDataTableCommand()
        {
            //this.CommandName = "SetDataTableRowValuesByDataTableCommand";
            //this.SelectionName = "Set DataTable Row Values By DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var myDT, var rowIndex) = this.GetDataTableVariableAndRowIndex(nameof(v_DataTableName), nameof(v_RowIndex), engine);

            (var addDT, var srcRowIndex) = this.GetDataTableVariableAndRowIndex(nameof(v_RowName), nameof(v_SrcRowIndex), engine);
            string ifNotColumnExists = this.GetUISelectionValue(nameof(v_NotExistsKey), "Column not exists", engine);

            // get columns list
            new GetDataTableColumnListCommand
            {
                v_DataTableName = this.v_DataTableName,
                v_OutputList = VariableNameControls.GetInnerVariableName(0, engine)
            }.RunCommand(engine);
            var columns = (List<string>)VariableNameControls.GetInnerVariable(0, engine).VariableValue;

            if (ifNotColumnExists == "error")
            {
                for (int i = 0; i < addDT.Columns.Count; i++)
                {
                    if (!columns.Contains(addDT.Columns[i].ColumnName))
                    {
                        throw new Exception("Column name " + addDT.Columns[i].ColumnName + " does not exists");
                    }
                }
            }
            for (int i = 0; i < addDT.Columns.Count; i++)
            {
                if (columns.Contains(addDT.Columns[i].ColumnName))
                {
                    myDT.Rows[rowIndex][addDT.Columns[i].ColumnName] = addDT.Rows[srcRowIndex][addDT.Columns[i].ColumnName];
                }
            }
        }
    }
}