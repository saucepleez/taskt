using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("DataTable Commands")]
    [Description("This command creates a DataTable with the Column Names provided.")]

    public class CreateDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("New DataTable Name")]
        [InputSpecification("Indicate a unique reference name for later use.")]
        [SampleUsage("MyDatatable")]
        [Remarks("")]
        public string v_DataTable { get; set; }

        [XmlElement]
        [PropertyDescription("Column Names")]
        [InputSpecification("Enter the Column Names required for each column of data.")]
        [SampleUsage("MyColumn || {vColumn}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_ColumnNameDataTable { get; set; }

        public CreateDataTableCommand()
        {
            CommandName = "CreateDataTableCommand";
            SelectionName = "Create DataTable";
            CommandEnabled = true;
            CustomRendering = true;

            //initialize data table
            v_ColumnNameDataTable = new DataTable
            {
                TableName = "ColumnNamesDataTable" + DateTime.Now.ToString("MMddyy.hhmmss")
            };

            v_ColumnNameDataTable.Columns.Add("Column Name");
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            DataTable Dt = new DataTable();

            foreach(DataRow rwColumnName in v_ColumnNameDataTable.Rows)
            {
                Dt.Columns.Add(rwColumnName.Field<string>("Column Name").ConvertToUserVariable(engine));
            }

            engine.AddVariable(v_DataTable, Dt);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataTable", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDataGridViewGroupFor("v_ColumnNameDataTable", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Create '{v_DataTable}' With {v_ColumnNameDataTable.Rows.Count} Column(s)]";
        }
    }
}