using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("Column Action")]
    [Attributes.ClassAttributes.CommandSettings("Set DataTable Column Values By DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to set a column to a DataTable by a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a column to a DataTable by a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SetDataTableColumnValuesByDataTableCommand : ADataTableBothColumnCommands
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        [PropertyDescription("DataTable Variable Name to be Setted")]
        [PropertyValidationRule("DataTable to be Setted", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable to be Setted")]
        public override string v_DataTable { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnType))]
        //public string v_ColumnType { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnNameIndex))]
        //public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        [PropertyDescription("DataTable Variable Name to Set")]
        [PropertyValidationRule("DataTable to Set", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable to Set")]
        [PropertyParameterOrder(8000)]
        public string v_SetDataTableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_WhenLessRows))]
        [PropertyDescription("When there are Less Rows than DataTable to set")]
        [PropertyParameterOrder(8001)]
        public string v_IfRowNotEnough { set; get; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_WhenGreaterRows))]
        [PropertyParameterOrder(8002)]
        public string v_IfSetDataTableNotEnough { set; get; }

        public SetDataTableColumnValuesByDataTableCommand()
        {
            //this.CommandName = "SetDataTableColumnByDataTableCommand";
            //this.SelectionName = "Set DataTable Column Values By DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var myDT, var colIndex) = this.ExpandUserVariablesAsDataTableAndColumnIndex(nameof(v_DataTable), nameof(v_ColumnType), nameof(v_ColumnIndex), engine);
            (var myDT, var colIndex, _) = this.ExpandValueOrUserVariableAsDataTableAndColumn(engine);

            string trgColName = myDT.Columns[colIndex].ColumnName;


            //DataTable setDT = v_SetDataTableName.ExpandUserVariableAsDataTable(engine);
            var setDT = this.ExpandUserVariableAsDataTable(nameof(v_SetDataTableName), engine);

            string ifRowNotEnough = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_IfRowNotEnough), "Row Not Enough", engine);
            // rows check
            if (myDT.Rows.Count < setDT.Rows.Count)
            {
                switch (ifRowNotEnough)
                {
                    case "ignore":
                    case "add rows":
                        break;

                    case "error":
                        throw new Exception("The number of rows is less than the DataTable to set");
                }
            }

            string ifDataTableNotEnough = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_IfSetDataTableNotEnough), "DataTable Not Enough", engine);
            if ((myDT.Rows.Count > setDT.Rows.Count) && (ifDataTableNotEnough == "error"))
            {
                throw new Exception("The number of DataTable items is less than the rows to settedd");
            }

            int maxRow = (myDT.Rows.Count > setDT.Rows.Count) ? setDT.Rows.Count : myDT.Rows.Count;
            for (int i = 0; i < maxRow; i++)
            {
                myDT.Rows[i][trgColName] = setDT.Rows[i][trgColName];
            }
            if ((myDT.Rows.Count < setDT.Rows.Count) && (ifRowNotEnough == "add rows"))
            {
                for (int i = myDT.Rows.Count; i < setDT.Rows.Count; i++)
                {
                    myDT.Rows.Add();
                    myDT.Rows[i][trgColName] = setDT.Rows[i][trgColName];
                }
            }
        }
    }
}