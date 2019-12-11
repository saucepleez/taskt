using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command opens an Excel Workbook.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to open an existing Excel Workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class LoadDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Dictionary Name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DictionaryName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the workbook file path")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the applicable file that should be opened by Excel.")]
        [Attributes.PropertyAttributes.SampleUsage(@"C:\temp\myfile.xlsx or [vFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FilePath { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the Sheet Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the applicable file that should be opened by Excel.")]
        [Attributes.PropertyAttributes.SampleUsage(@"C:\temp\myfile.xlsx or [vFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SheetName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the Key column")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the applicable file that should be opened by Excel.")]
        [Attributes.PropertyAttributes.SampleUsage(@"C:\temp\myfile.xlsx or [vFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_KeyColumn { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the Value Column")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the applicable file that should be opened by Excel.")]
        [Attributes.PropertyAttributes.SampleUsage(@"C:\temp\myfile.xlsx or [vFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ValueColumn { get; set; }
        public LoadDictionaryCommand()
        {
            this.CommandName = "ExcelOpenWorkbookCommand";
            this.SelectionName = "Load Dictionary";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = "Test";
            var vFilePath = v_FilePath.ConvertToUserVariable(sender);

            var newExcelSession = new Microsoft.Office.Interop.Excel.Application
            {
                Visible = true
            };

            engine.AddAppInstance("Test", newExcelSession);


            var excelObject = engine.GetAppInstance(vInstance);
            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
            //excelInstance.Workbooks.Open(vFilePath);

            DatasetCommands dataSetCommand = new DatasetCommands();
            DataTable requiredData = dataSetCommand.CreateDataTable(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + v_FilePath + @";Extended Properties=""Excel 12.0;HDR=YES;IMEX=1""", "Select "+v_KeyColumn+ ","+ v_ValueColumn + " From [" + v_SheetName + "$]");
            //Console.WriteLine(requiredData.Columns[0].ColumnName);

              //  requiredData.Columns[requiredData.Columns[0].ToString()].ColumnName = v_KeyColumn;
                //requiredData.Columns[requiredData.Columns[1].ToString()].ColumnName = v_ValueColumn;
            var dictlist = requiredData.AsEnumerable().Select(x => new
            {
                keys = (string)x[v_KeyColumn],
                values = (string)x[v_ValueColumn]
            }).ToList();
            Dictionary<string, string> outputDictionary = new Dictionary<string, string>();
            foreach (var dict in dictlist)
            {
                outputDictionary.Add(dict.keys, dict.values);
            }


            Script.ScriptVariable newDictionary = new Script.ScriptVariable
            {
                VariableName = v_DictionaryName,
                VariableValue = outputDictionary
            };
            //close excel
            excelInstance.Quit();

            //remove instance
            engine.RemoveAppInstance(vInstance);
            engine.VariableList.Add(newDictionary);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DictionaryName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SheetName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_KeyColumn", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ValueColumn", this, editor));


            return RenderedControls;

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Open from '" + v_FilePath + "', Instance Name: '"  + "']";
        }
    }
}