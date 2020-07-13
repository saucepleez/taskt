using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Word;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command appends a datatable to a word document.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to append a datatable to a specific document.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    public class WordAppendDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Word** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **wordInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Word** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the DataTable Name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the DataTable you would like to append.")]
        [Attributes.PropertyAttributes.SampleUsage("**myData**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataTableName { get; set; }

        public WordAppendDataTableCommand()
        {
            this.CommandName = "WordAppendDataTableCommand";
            this.SelectionName = "Append DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var wordObject = engine.GetAppInstance(vInstance);
            var dataSetVariable = LookupVariable(engine);

            System.Data.DataTable dataTable = new System.Data.DataTable();
            dataTable = (System.Data.DataTable)dataSetVariable.VariableValue;

            //selecting the word instance and open document
            Microsoft.Office.Interop.Word.Application wordInstance = (Microsoft.Office.Interop.Word.Application)wordObject;
            Document wordDocument = wordInstance.ActiveDocument;


            //converting System DataTable to Word DataTable
            int RowCount = dataTable.Rows.Count; 
            int ColumnCount = dataTable.Columns.Count;
            Object[,] DataArray = new object[RowCount + 1, ColumnCount + 1];
           
            int r = 0;
            for (int c = 0; c <= ColumnCount - 1; c++)
            {
                DataArray[r, c] = dataTable.Columns[c].ColumnName;
                for (r = 0; r <= RowCount - 1; r++)
                {
                    DataArray[r, c] = dataTable.Rows[r][c];
                } //end row loop
            } //end column loop

            object collapseEnd = WdCollapseDirection.wdCollapseEnd;
            dynamic docRange = wordDocument.Content; 
            docRange.Collapse(ref collapseEnd);

            String tempString = "";
            for (r = 0; r <= RowCount - 1; r++)
            {
                for (int c = 0; c <= ColumnCount - 1; c++)
                {
                    tempString = tempString + DataArray[r, c] + "\t";
                }
            }

            //appending data row text after all text/images
            docRange.Text = tempString;

            //converting and formatting data table
            object Separator = WdTableFieldSeparator.wdSeparateByTabs;
            object Format = WdTableFormat.wdTableFormatGrid1;
            object ApplyBorders = true;
            object AutoFit = true;

            object AutoFitBehavior = WdAutoFitBehavior.wdAutoFitContent;
            docRange.ConvertToTable(ref Separator, ref RowCount, ref ColumnCount, Type.Missing, ref Format,
                                    ref ApplyBorders, Type.Missing, Type.Missing, Type.Missing,Type.Missing, 
                                    Type.Missing, Type.Missing, Type.Missing, ref AutoFit, ref AutoFitBehavior, Type.Missing);

            docRange.Select();
            wordDocument.Application.Selection.Tables[1].Select();
            wordDocument.Application.Selection.Tables[1].Rows.AllowBreakAcrossPages = 0;
            wordDocument.Application.Selection.Tables[1].Rows.Alignment = 0;
            wordDocument.Application.Selection.Tables[1].Rows[1].Select();
            wordDocument.Application.Selection.InsertRowsAbove(1);
            wordDocument.Application.Selection.Tables[1].Rows[1].Select();

            //Adding header row manually
            for (int c = 0; c <= ColumnCount - 1; c++)
            {
                wordDocument.Application.Selection.Tables[1].Cell(1, c + 1).Range.Text = dataTable.Columns[c].ColumnName;
            }

            //Formatting header row
            wordDocument.Application.Selection.Tables[1].Rows[1].Select();
            wordDocument.Application.Selection.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            wordDocument.Application.Selection.Font.Bold = 1;

        }

        private Script.ScriptVariable LookupVariable(Core.Automation.Engine.AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_DataTableName).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if ((requiredVariable == null) && (v_DataTableName.StartsWith(sendingInstance.engineSettings.VariableStartMarker)) && (v_DataTableName.EndsWith(sendingInstance.engineSettings.VariableEndMarker)))
            {
                //reformat and attempt
                var reformattedVariable = v_DataTableName.Replace(sendingInstance.engineSettings.VariableStartMarker, "").Replace(sendingInstance.engineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataTableName", this, editor));

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " ['" + v_DataTableName + "' To Instance Name: '" + v_InstanceName + "']";
        }
    }
}