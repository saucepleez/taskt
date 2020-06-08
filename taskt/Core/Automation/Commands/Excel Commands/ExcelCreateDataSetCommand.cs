using System;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using System.Data.OleDb;
using taskt.Core.Automation.Engine;
using taskt.Core.Utilities.CommonUtilities;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command gets a range of cells and applies them against a dataset")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to quickly iterate over Excel as a dataset.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'OLEDB' to achieve automation.")]
    public class ExcelCreateDataSetCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please create a DataSet name")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate a unique reference name for later use")]
        [Attributes.PropertyAttributes.SampleUsage("vMyDataset")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataSetName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the workbook file path")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the workbook file")]
        [Attributes.PropertyAttributes.SampleUsage(@"C:\temp\myfile.xlsx")]
        [Attributes.PropertyAttributes.Remarks("This command does not require Excel to be opened.  A snapshot will be taken of the workbook as it exists at the time this command runs.")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the sheet name")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate the specific sheet that should be retrieved.")]
        [Attributes.PropertyAttributes.SampleUsage("Sheet1, mySheet, [vSheet]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SheetName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate if Header Row Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Select the necessary indicator")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Yes**, **NO**.  Data will be loaded as column headers if **YES** is selected.")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ContainsHeaderRow { get; set; }

        public ExcelCreateDataSetCommand()
        {
            CommandName = "ExcelCreateDataSetCommand";
            SelectionName = "Create DataSet";
            CommandEnabled = true;
            CustomRendering = true;
            v_ContainsHeaderRow = "Yes";
        }

        public override void RunCommand(object sender)
        {
            DataTable requiredData = CreateDataTable(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                v_FilePath.ConvertToUserVariable(sender) +
                $@";Extended Properties=""Excel 12.0;HDR={v_ContainsHeaderRow.ConvertToUserVariable(sender)};IMEX=1""",
                "Select * From [" + v_SheetName.ConvertToUserVariable(sender) + "$]");

            var engine = (AutomationEngineInstance)sender;

            Script.ScriptVariable newDataset = new Script.ScriptVariable
            {
                VariableName = v_DataSetName,
                VariableValue = requiredData
            };

            engine.VariableList.Add(newDataset);

        }
        public DataTable CreateDataTable(string connection, string query)
        {
            //create vars
            var dataTable = new DataTable();
            var oleConnection = new OleDbConnection(connection);
            var oleCommand = new OleDbCommand(query, oleConnection);

            //get data
            OleDbDataAdapter adapter = new OleDbDataAdapter(oleCommand);
            oleConnection.Open();
            adapter.Fill(dataTable);
            oleConnection.Close();

            //clean up
            oleConnection.Dispose();
            adapter.Dispose();
            oleCommand.Dispose();

            return dataTable;
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataSetName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SheetName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ContainsHeaderRow", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Get '" + v_SheetName + "' from '" + v_FilePath + "' and apply to '" + v_DataSetName + "']";
        }
    }
}