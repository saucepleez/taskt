using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("Row Action")]
    [Attributes.ClassAttributes.CommandSettings("Set DataTable Row Values By DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to set a DataTable Row values to a DataTable by a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a DataTable Row values to a DataTable by a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SetDataTableRowValuesByDataTableCommand : ADataTableBothRowCommands
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        [PropertyDescription("DataTable Variable Name to be Setted")]
        public override string v_DataTable { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        //[PropertyDescription(" Row Index to be Setted")]
        //public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        [PropertyDescription("DataTable Variable Name to Set")]
        public string v_RowName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        [PropertyDescription("Row Index to Set")]
        public string v_SrcRowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_WhenColumnNotExists))]
        public string v_WhenColumnNotExists { get; set; }

        public SetDataTableRowValuesByDataTableCommand()
        {
            //this.CommandName = "SetDataTableRowValuesByDataTableCommand";
            //this.SelectionName = "Set DataTable Row Values By DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var myDT, var rowIndex) = this.ExpandUserVariablesAsDataTableAndRowIndex(nameof(v_DataTable), nameof(v_RowIndex), engine);
            (var myDT, var rowIndex) = this.ExpandValueOrUserVariableAsDataTableAndRow(engine);

            var addDT = this.ExpandUserVariableAsDataTable(nameof(v_RowName), engine);
            var srcRowIndex = this.ExpandValueOrUserVariableAsInteger(nameof(v_SrcRowIndex), engine);
            if (srcRowIndex < 0)
            {
                srcRowIndex += addDT.Rows.Count;
            }
            if (srcRowIndex < 0 || srcRowIndex >= addDT.Rows.Count)
            {
                throw new Exception($"Strange DataTable Row Index. Index: '{v_SrcRowIndex}', Expand: '{srcRowIndex}'");
            }

            //(var addDT, var srcRowIndex) = this.ExpandUserVariablesAsDataTableAndRowIndex(nameof(v_RowName), nameof(v_SrcRowIndex), engine);
            string ifNotColumnExists = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenColumnNotExists), "Column not exists", engine);

            //// get columns list
            //new GetDataTableColumnListCommand
            //{
            //    v_DataTable = this.v_DataTable,
            //    v_Result = VariableNameControls.GetInnerVariableName(0, engine)
            //}.RunCommand(engine);
            //var columns = (List<string>)VariableNameControls.GetInnerVariable(0, engine).VariableValue;

            //if (ifNotColumnExists == "error")
            //{
            //    for (int i = 0; i < addDT.Columns.Count; i++)
            //    {
            //        if (!columns.Contains(addDT.Columns[i].ColumnName))
            //        {
            //            throw new Exception("Column name " + addDT.Columns[i].ColumnName + " does not exists");
            //        }
            //    }
            //}
            //for (int i = 0; i < addDT.Columns.Count; i++)
            //{
            //    if (columns.Contains(addDT.Columns[i].ColumnName))
            //    {
            //        myDT.Rows[rowIndex][addDT.Columns[i].ColumnName] = addDT.Rows[srcRowIndex][addDT.Columns[i].ColumnName];
            //    }
            //}

            using (var myColumn = new InnerScriptVariable(engine))
            {
                // get columns list
                new GetDataTableColumnListCommand
                {
                    v_DataTable = this.v_DataTable,
                    v_Result = myColumn.VariableName,
                }.RunCommand(engine);
                //var columns = (List<string>)VariableNameControls.GetInnerVariable(0, engine).VariableValue;
                //var columns = (List<string>)myColumn.VariableValue;
                var columns = EM_CanHandleListExtensionMethods.ExpandUserVariableAsList(myColumn);

                if (ifNotColumnExists == "error")
                {
                    for (int i = 0; i < addDT.Columns.Count; i++)
                    {
                        if (!columns.Contains(addDT.Columns[i].ColumnName))
                        {
                            throw new Exception($"Column name {addDT.Columns[i].ColumnName} does not exists");
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
}
