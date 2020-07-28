using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Script;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;
using Application = Microsoft.Office.Interop.Word.Application;
using DataTable = System.Data.DataTable;
using Group = taskt.Core.Attributes.ClassAttributes.Group;

namespace taskt.Commands
{
    [Serializable]
    [Group("Word Commands")]
    [Description("This command appends a DataTable to a Word Document.")]

    public class WordAppendDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Word Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyWordInstance || {vWordInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("DataTable")]
        [InputSpecification("Enter the DataTable to append to the Document.")]
        [SampleUsage("{vDataTable}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataTable { get; set; }

        public WordAppendDataTableCommand()
        {
            CommandName = "WordAppendDataTableCommand";
            SelectionName = "Append DataTable";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultWord";
        }
        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var wordObject = engine.GetAppInstance(vInstance);
            var dataSetVariable = LookupVariable(engine);

            DataTable dataTable = (DataTable)dataSetVariable.VariableValue;

            //selecting the word instance and open document
            Application wordInstance = (Application)wordObject;
            Document wordDocument = wordInstance.ActiveDocument;

            //converting System DataTable to Word DataTable
            int RowCount = dataTable.Rows.Count; 
            int ColumnCount = dataTable.Columns.Count;
            object[,] DataArray = new object[RowCount + 1, ColumnCount + 1];
           
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

            string tempString = "";
            for (r = 0; r <= RowCount - 1; r++)
            {
                for (int c = 0; c <= ColumnCount - 1; c++)
                    tempString = tempString + DataArray[r, c] + "\t";
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
                                    Type.Missing, Type.Missing, Type.Missing, ref AutoFit, ref AutoFitBehavior, 
                                    Type.Missing);

            docRange.Select();
            wordDocument.Application.Selection.Tables[1].Select();
            wordDocument.Application.Selection.Tables[1].Rows.AllowBreakAcrossPages = 0;
            wordDocument.Application.Selection.Tables[1].Rows.Alignment = 0;
            wordDocument.Application.Selection.Tables[1].Rows[1].Select();
            wordDocument.Application.Selection.InsertRowsAbove(1);
            wordDocument.Application.Selection.Tables[1].Rows[1].Select();

            //Adding header row manually
            for (int c = 0; c <= ColumnCount - 1; c++)
                wordDocument.Application.Selection.Tables[1].Cell(1, c + 1).Range.Text = dataTable.Columns[c].ColumnName;

            //Formatting header row
            wordDocument.Application.Selection.Tables[1].Rows[1].Select();
            wordDocument.Application.Selection.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            wordDocument.Application.Selection.Font.Bold = 1;
        }
        
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataTable", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Append '{v_DataTable}' - Instance Name '{v_InstanceName}']";
        }

        private ScriptVariable LookupVariable(AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_DataTable).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if (requiredVariable == null && v_DataTable.StartsWith(sendingInstance.EngineSettings.VariableStartMarker)
                                         && v_DataTable.EndsWith(sendingInstance.EngineSettings.VariableEndMarker))
            {
                //reformat and attempt
                var reformattedVariable = v_DataTable.Replace(sendingInstance.EngineSettings.VariableStartMarker, "")
                                                         .Replace(sendingInstance.EngineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }
            return requiredVariable;
        }
    }
}