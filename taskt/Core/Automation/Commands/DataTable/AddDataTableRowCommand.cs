﻿using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to add a row to a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a row to a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class AddDataTableRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the DataTable Variable Name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a existing DataTable to add rows to.")]
        [Attributes.PropertyAttributes.SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_DataTableName { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Define Data")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the Column Names required for each column of data")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [Attributes.PropertyAttributes.PropertyDataGridViewSetting(true, true, true)]
        [Attributes.PropertyAttributes.PropertyDataGridViewColumnSettings("Column Name", "Column Name", false)]
        [Attributes.PropertyAttributes.PropertyDataGridViewColumnSettings("Data", "Data", false)]
        [Attributes.PropertyAttributes.PropertyDataGridViewCellEditEvent("AddDataGridViewHelper_CellClick", Attributes.PropertyAttributes.PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        [Attributes.PropertyAttributes.PropertyCustomUIHelper("Load Column Names From Existing Table", "LoadSchemaControl_Click", "load_column")]
        public DataTable v_AddDataDataTable { get; set; }


        [XmlIgnore]
        [NonSerialized]
        private List<CreateDataTableCommand> DataTableCreationCommands;

        [XmlIgnore]
        [NonSerialized]
        private DataGridView AddDataGridViewHelper;

        public AddDataTableRowCommand()
        {
            this.CommandName = "AddDataTableRowCommand";
            this.SelectionName = "Add DataTable Row";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            DataTable dataTable = v_DataTableName.GetDataTableVariable(engine);

            var newRow = dataTable.NewRow();

            foreach (DataRow rw in v_AddDataDataTable.Rows)
            {
                var columnName = rw.Field<string>("Column Name").ConvertToUserVariable(sender);
                var data = rw.Field<string>("Data").ConvertToUserVariable(sender);
                newRow.SetField(columnName, data); 
            }

            dataTable.Rows.Add(newRow);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            AddDataGridViewHelper = (DataGridView)ctrls.GetControlsByName("v_AddDataDataTable")[0];

            DataTableCreationCommands = editor.configuredCommands.Where(f => f is CreateDataTableCommand).Select(f => (CreateDataTableCommand)f).ToList();

            return RenderedControls;
        }

        private void LoadSchemaControl_Click(object sender, EventArgs e)
        {
            var items = new List<string>();
            foreach (var item in DataTableCreationCommands)
            {
                items.Add(item.v_DataTableName);
            }
            UI.Forms.Supplemental.frmItemSelector selectionForm = new UI.Forms.Supplemental.frmItemSelector(items, "Load Schema", "Select a table from the list");
            //selectionForm.Text = "Load Schema";
            //selectionForm.lblHeader.Text = "Select a table from the list";
            //foreach (var item in DataTableCreationCommands)
            //{
            //    selectionForm.lstVariables.Items.Add(item.v_DataTableName);
            //}

            var result = selectionForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                //var tableName = selectionForm.lstVariables.SelectedItem.ToString();
                var tableName = selectionForm.selectedItem.ToString();
                var schema = DataTableCreationCommands.Where(f => f.v_DataTableName == tableName).FirstOrDefault();

                v_AddDataDataTable.Rows.Clear();

                foreach (DataRow rw in schema.v_ColumnNameDataTable.Rows)
                {
                    v_AddDataDataTable.Rows.Add(rw.Field<string>("Column Name"), "");
                }
            }   
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_AddDataDataTable.Rows.Count} Fields, apply to '{v_DataTableName}']";
        }

        private void AddDataGridViewHelper_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                AddDataGridViewHelper.BeginEdit(false);
            }
            else
            {
                AddDataGridViewHelper.EndEdit();
            }
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            if (AddDataGridViewHelper.IsCurrentCellDirty || AddDataGridViewHelper.IsCurrentRowDirty)
            {
                AddDataGridViewHelper.CommitEdit(DataGridViewDataErrorContexts.Commit);
                var newRow = v_AddDataDataTable.NewRow();
                v_AddDataDataTable.Rows.Add(newRow);
                for (var i = v_AddDataDataTable.Rows.Count - 1; i >= 0; i--)
                {
                    if (v_AddDataDataTable.Rows[i][0].ToString() == "" && v_AddDataDataTable.Rows[i][1].ToString() == "")
                    {
                        v_AddDataDataTable.Rows[i].Delete();
                    }
                }
            }
        }
    }
}