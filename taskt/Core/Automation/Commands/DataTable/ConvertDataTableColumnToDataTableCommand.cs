using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert Column")]
    [Attributes.ClassAttributes.CommandSettings("Convert DataTable Column To DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Column to DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Column to DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertDataTableColumnToDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        [PropertyDescription("DataTable Variable Name to Converted")]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnType))]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnNameIndex))]
        public string v_DataColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_NewOutputDataTableName))]
        public string v_OutputVariableName { get; set; }

        public ConvertDataTableColumnToDataTableCommand()
        {
            //this.CommandName = "ConvertDataTableColumnToDataTableCommand";
            //this.SelectionName = "Convert DataTable Column To DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;         
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var srcDT, var colIndex) = this.GetDataTableVariableAndColumnIndex(nameof(v_DataTableName), nameof(v_ColumnType), nameof(v_DataColumnIndex), engine);
            DataTable myDT = new DataTable();
            myDT.Columns.Add(srcDT.Columns[colIndex].ColumnName);
            for (int i = 0; i < srcDT.Rows.Count; i++)
            {
                myDT.Rows.Add();
                myDT.Rows[i][0] = srcDT.Rows[i][colIndex]?.ToString() ?? "";
            }

            myDT.StoreInUserVariable(engine, v_OutputVariableName);
        }
    }
}