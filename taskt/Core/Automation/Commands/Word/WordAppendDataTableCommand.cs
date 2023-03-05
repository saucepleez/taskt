using System;
using System.Xml.Serialization;
using Microsoft.Office.Interop.Word;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command appends a datatable to a word document.")]
    [Attributes.ClassAttributes.CommandSettings("Append DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to append a datatable to a specific document.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WordAppendDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WordControls), nameof(WordControls.v_InstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        public string v_DataTableName { get; set; }

        public WordAppendDataTableCommand()
        {
            //this.CommandName = "WordAppendDataTableCommand";
            //this.SelectionName = "Append DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var _, var wordDocument) = v_InstanceName.GetWordInstanceAndDocument(engine);

            var dataTable = v_DataTableName.GetDataTableVariable(engine);

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
    }
}