using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using System.Data;
using System.Text;
using System.Linq;
using System.IO;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command gets text from a specified Excel Range and splits it into separate ranges by column.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to split a range into separate ranges.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    public class ExcelSplitRangeByColumnCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **excelInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the First Cell Location (ex. A1 or B2)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the actual location of the cell.")]
        [Attributes.PropertyAttributes.SampleUsage("A1, B10, [vAddress]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ExcelCellAddress1 { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Second Cell Location (ex. A1 or B2, Leave Blank for All)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the actual location of the cell.")]
        [Attributes.PropertyAttributes.SampleUsage("A1, B10, [vAddress]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ExcelCellAddress2 { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Column Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the name of the column you wish to split by.")]
        [Attributes.PropertyAttributes.SampleUsage("ColA, [vColumn]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ColumnName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the output directory")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the new directory for the split Excel Files.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\new path\\ or [vTextFolderPath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_OutputDirectory { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate the File Type to save as")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("xlsx")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("csv")]
        [Attributes.PropertyAttributes.InputSpecification("Specify the file format type for the split ranges")]
        [Attributes.PropertyAttributes.SampleUsage("Select either **xlsx* or **csv**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FileType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Assign DataTable List to Variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_userVariableName { get; set; }

        public ExcelSplitRangeByColumnCommand()
        {
            this.CommandName = "ExcelSplitRangeByColumnCommand";
            this.SelectionName = "Split Range By Column";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            v_FileType = "xlsx";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var vExcelObject = engine.GetAppInstance(vInstance);
            var vTargetAddress1 = v_ExcelCellAddress1.ConvertToUserVariable(sender);
            var vTargetAddress2 = v_ExcelCellAddress2.ConvertToUserVariable(sender);
            var vColumnName = v_ColumnName.ConvertToUserVariable(sender);
            var vOutputDirectory = v_OutputDirectory.ConvertToUserVariable(sender);

            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)vExcelObject;
            excelInstance.Visible = false;
            excelInstance.DisplayAlerts = false;
            Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;

            Microsoft.Office.Interop.Excel.Range cellValue;
            if (vTargetAddress2 != "")
                cellValue = excelSheet.Range[vTargetAddress1, vTargetAddress2];
            else
            {
                Microsoft.Office.Interop.Excel.Range last = excelSheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                cellValue = excelSheet.Range[vTargetAddress1, last];
            }

            //Convert Range to DataTable
            List<object> lst = new List<object>();
            int rw = cellValue.Rows.Count;
            int cl = cellValue.Columns.Count;
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
                    if (((cellValue.Cells[rCnt, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2) != null)
                    {
                        if (!DT.Columns.Contains(cCnt.ToString()))
                        {
                            DT.Columns.Add(cCnt.ToString());
                        }
                        newRow[cCnt.ToString()] = ((cellValue.Cells[rCnt, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2).ToString();
                    }
                    else if (((cellValue.Cells[rCnt, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2) == null && ((cellValue.Cells[1, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2) != null)
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
                cName = ((cellValue.Cells[1, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2).ToString();
                DT.Columns[cCnt-1].ColumnName = cName;
            }

            //split table by column
            List<DataTable> result = DT.AsEnumerable()
                                       .GroupBy(row => row.Field<string>(vColumnName))
                                       .Select(g => g.CopyToDataTable())
                                       .ToList();

            //add list of datatables to output variable
            Script.ScriptVariable splitDataset = new Script.ScriptVariable
            {
                VariableName = v_userVariableName,
                VariableValue = result
            };
            engine.VariableList.Add(splitDataset);

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
                    catch (Exception ex)
                    {
                        continue;
                    }
                     
                    Console.WriteLine(newName);
                    Microsoft.Office.Interop.Excel.Workbook newWorkBook = excelInstance.Workbooks.Add(Type.Missing);
                    Microsoft.Office.Interop.Excel.Worksheet newSheet = newWorkBook.ActiveSheet;
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
                        newWorkBook.SaveAs(Path.Combine(vOutputDirectory, newName), Microsoft.Office.Interop.Excel.XlFileFormat.xlCSV, Type.Missing, Type.Missing,
                                        Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing); 
                    }
                    else if (!newName.Equals(string.Empty))
                    {
                        newWorkBook.SaveAs(Path.Combine(vOutputDirectory, newName + ".xlsx"));
                    }
                    
                }
            }   
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ExcelCellAddress1", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ExcelCellAddress2", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ColumnName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_OutputDirectory", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_FileType", this, editor));
            //create control for variable name
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_userVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            //RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_Output", this, editor));


            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Get Value Between '" + v_ExcelCellAddress1 + " and " + v_ExcelCellAddress2 + "' and apply to variable '" + v_userVariableName + "'from, Instance Name: '" + v_InstanceName + "', Split By Column: '" + v_ColumnName + "']";
        }
    }
}
