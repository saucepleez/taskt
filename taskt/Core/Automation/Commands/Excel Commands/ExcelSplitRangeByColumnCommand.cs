using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;
using DataTable = System.Data.DataTable;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Excel Commands")]
    [Description("This command takes a specific Excel range, splits it into separate ranges by column, and stores them in new Workbooks.")]

    public class ExcelSplitRangeByColumnCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance || {vExcelInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Range")]
        [InputSpecification("Enter the location of the range to split.")]
        [SampleUsage("A1:B10 || A1: || {vRange} || {vStart}:{vEnd} || {vStart}:")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Range { get; set; }

        [XmlAttribute]
        [PropertyDescription("Column to Split")]
        [InputSpecification("Enter the name of the column you wish to split the selected range by.")]
        [SampleUsage("ColA || {vColumnName}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ColumnName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Split Range Output Directory")]
        [InputSpecification("Enter or Select the new directory for the split range files.")]
        [SampleUsage(@"C:\temp\Split Files\ || {vFolderPath} || {ProjectPath}\Split Files")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_OutputDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output File Type")]
        [PropertyUISelectionOption("xlsx")]
        [PropertyUISelectionOption("csv")]
        [InputSpecification("Specify the file format type for the split range files.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_FileType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output DataTable List Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                 " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public ExcelSplitRangeByColumnCommand()
        {
            CommandName = "ExcelSplitRangeByColumnCommand";
            SelectionName = "Split Range By Column";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
            v_FileType = "xlsx";
            v_Range = "A1:";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var vExcelObject = engine.GetAppInstance(vInstance);
            var vRange = v_Range.ConvertToUserVariable(sender);
            var vColumnName = v_ColumnName.ConvertToUserVariable(sender);
            var vOutputDirectory = v_OutputDirectory.ConvertToUserVariable(sender);
            var excelInstance = (Application)vExcelObject;

            excelInstance.DisplayAlerts = false;
            Worksheet excelSheet = excelInstance.ActiveSheet;

            var splitRange = vRange.Split(':');
            Range cellRange;

            try
            {
                Range last = excelSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
                if (splitRange[1] == "")
                    cellRange = excelSheet.Range[splitRange[0], last];
                else
                    cellRange = excelSheet.Range[splitRange[0], splitRange[1]];
            }
            //Attempt to extract a single cell
            catch (Exception)
            {
                throw new Exception("Selected range is invalid");
            }

            //Convert Range to DataTable
            List<object> lst = new List<object>();
            int rw = cellRange.Rows.Count;
            int cl = cellRange.Columns.Count;
            int rCnt;
            int cCnt;
            string cName;
            DataTable DT = new DataTable();
            
            //start from row 2
            for (rCnt = 2; rCnt <= rw; rCnt++)
            {
                DataRow newRow = DT.NewRow();
                for (cCnt = 1; cCnt <= cl; cCnt++)
                {
                    if (((cellRange.Cells[rCnt, cCnt] as Range).Value2) != null)
                    {
                        if (!DT.Columns.Contains(cCnt.ToString()))
                        {
                            DT.Columns.Add(cCnt.ToString());
                        }
                        newRow[cCnt.ToString()] = ((cellRange.Cells[rCnt, cCnt] as Range).Value2).ToString();
                    }
                    else if (((cellRange.Cells[rCnt, cCnt] as Range).Value2) == null && ((cellRange.Cells[1, cCnt] as Range).Value2) != null)
                    {
                        if (!DT.Columns.Contains(cCnt.ToString()))
                        {
                            DT.Columns.Add(cCnt.ToString());
                        }
                        newRow[cCnt.ToString()] = string.Empty;
                    }
                }
                DT.Rows.Add(newRow);
            }

            //Set column names
            for (cCnt = 1; cCnt <= cl; cCnt++)
            {
                cName = ((cellRange.Cells[1, cCnt] as Range).Value2).ToString();
                DT.Columns[cCnt-1].ColumnName = cName;
            }

            //split table by column
            List<DataTable> result = DT.AsEnumerable()
                                       .GroupBy(row => row.Field<string>(vColumnName))
                                       .Select(g => g.CopyToDataTable())
                                       .ToList();

            //add list of datatables to output variable
            engine.AddVariable(v_OutputUserVariableName, result);

            //save split datatables in individual workbooks labeled by selected column data
            if (Directory.Exists(vOutputDirectory))
            {
                string newName;
                foreach (DataTable newDT in result)
                {
                    try
                    {
                        newName = newDT.Rows[0].Field<string>(vColumnName).ToString();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    var splitExcelInstance = new Application();
                    Workbook newWorkBook = splitExcelInstance.Workbooks.Add(Type.Missing);
                    Worksheet newSheet = newWorkBook.ActiveSheet;
                    for (int i = 1; i < newDT.Columns.Count + 1; i++)
                    {
                        newSheet.Cells[1, i] = newDT.Columns[i - 1].ColumnName;
                    }

                    for (int j = 0; j < newDT.Rows.Count; j++)
                    {
                        for (int k = 0; k < newDT.Columns.Count; k++)
                        {
                            newSheet.Cells[j + 2, k + 1] = newDT.Rows[j].ItemArray[k].ToString();
                        }
                    }

                    if (newName.Contains("."))
                    {
                        newName = newName.Replace(".", string.Empty);
                    }

                    if (v_FileType == "csv" && !newName.Equals(string.Empty))
                    {
                        newWorkBook.SaveAs(Path.Combine(vOutputDirectory, newName), XlFileFormat.xlCSV, Type.Missing, Type.Missing,
                                        Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange,
                                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);                      
                    }
                    else if (!newName.Equals(string.Empty))
                    {
                        newWorkBook.SaveAs(Path.Combine(vOutputDirectory, newName + ".xlsx"));
                    }
                    newWorkBook.Close();
                }
            }   
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Range", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ColumnName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_OutputDirectory", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_FileType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Split Range '{v_Range}' by Column '{v_ColumnName}' - Store DataTable List in '{v_OutputUserVariableName}' - Instance Name '{v_InstanceName}']";
        }
    }
}
