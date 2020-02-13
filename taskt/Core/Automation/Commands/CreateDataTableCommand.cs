using System;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using System.Drawing;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.Description("This command created a DataTable with the column names provided")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create a new DataTable")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class CreateDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Indicate DataTable Name")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate a unique reference name for later use")]
        [Attributes.PropertyAttributes.SampleUsage("vMyDatatable")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataTableName { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Define Column Names")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the Column Names required for each column of data")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_ColumnNameDataTable { get; set; }


        public CreateDataTableCommand()
        {
            this.CommandName = "CreateDataTableCommand";
            this.SelectionName = "Create DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            //initialize data table
            this.v_ColumnNameDataTable = new System.Data.DataTable
            {
                TableName = "ColumnNamesDataTable" + DateTime.Now.ToString("MMddyy.hhmmss")
            };

            this.v_ColumnNameDataTable.Columns.Add("Column Name");


        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var dataTableName = v_DataTableName.ConvertToUserVariable(sender);

            DataTable Dt = new DataTable();

            foreach(DataRow rwColumnName in v_ColumnNameDataTable.Rows)
            {
                Dt.Columns.Add(rwColumnName.Field<string>("Column Name"));
            }


            

            //add or override existing variable
            if (engine.VariableList.Any(f => f.VariableName == dataTableName))
            {
                var selectedVariable = engine.VariableList.Where(f => f.VariableName == dataTableName).FirstOrDefault();
                selectedVariable.VariableValue = Dt;
            }
            else
            {
                Script.ScriptVariable newDataTable = new Script.ScriptVariable
                {
                    VariableName = dataTableName,
                    VariableValue = Dt
                };

                engine.VariableList.Add(newDataTable);
            }


     
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataTableName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDataGridViewGroupFor("v_ColumnNameDataTable", this, editor));
            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Name: '{v_DataTableName}' with {v_ColumnNameDataTable.Rows.Count} Columns]";
        }
    }
}