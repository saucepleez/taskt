using System;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("DataTable Action")]
    [Attributes.ClassAttributes.Description("This command gets a range of cells and applies them against a dataset")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to quickly iterate over Excel as a dataset.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'OLEDB' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class LoadDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please create a DataTable Variable Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Indicate a unique reference name for later use")]
        [SampleUsage("**vMyDataset** or **{{{vMyDataset}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable")]
        public string v_DataSetName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the workbook file path")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("Enter or Select the path to the workbook file")]
        [SampleUsage("**C:\\temp\\myfile.xlsx** or **{{{vFilePath}}}**")]
        [Remarks("This command does not require Excel to be opened.  A snapshot will be taken of the workbook as it exists at the time this command runs.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("File", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "File")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the sheet name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Indicate the specific sheet that should be retrieved.")]
        [SampleUsage("**Sheet1** or **mySheet** or **{{{vSheet}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Sheet", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Sheet")]
        public string v_SheetName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Indicate if Header Row Exists")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]    
        [InputSpecification("Select the necessary indicator")]
        [SampleUsage("Select **Yes**, **No**.  Data will be loaded as column headers if **YES** is selected.")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_ContainsHeaderRow { get; set; }

        public LoadDataTableCommand()
        {
            this.CommandName = "LoadDataTableCommand";
            this.SelectionName = "Load DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_ContainsHeaderRow = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //DataTableControls dataSetCommand = new DataTableControls();

            //string filePath = FilePathControls.FormatFilePath_NoFileCounter(v_FilePath, engine, new List<string>() { "xlsx", "xlsm", "xls" }, true);
            var filePath = v_FilePath.ConvertToUserVariable(engine);

            DataTable requiredData = DataTableControls.CreateDataTable(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + $@";Extended Properties=""Excel 12.0;HDR={v_ContainsHeaderRow.ConvertToUserVariable(sender)};IMEX=1""", "Select * From [" + v_SheetName.ConvertToUserVariable(sender) + "$]");

            requiredData.StoreInUserVariable(engine, v_DataSetName);
        }
        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Get '" + v_SheetName + "' from '" + v_FilePath + "' and apply to '" + v_DataSetName + "']";
        //}
    }
}