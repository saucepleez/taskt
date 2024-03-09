using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("File/Book")]
    [Attributes.ClassAttributes.Description("This command Open a File and Get Cell Values as a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Open a File and Get Cell Values as a DataTable")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class LoadDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please create a DataTable Variable Name")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Indicate a unique reference name for later use")]
        //[SampleUsage("**vMyDataset** or **{{{vMyDataset}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "DataTable")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        public string v_DataSetName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please indicate the workbook file path")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        //[InputSpecification("Enter or Select the path to the workbook file")]
        //[SampleUsage("**C:\\temp\\myfile.xlsx** or **{{{vFilePath}}}**")]
        //[Remarks("This command does not require Excel to be opened.  A snapshot will be taken of the workbook as it exists at the time this command runs.")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("File", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "File")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_FilePath))]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "xlsx,xlsm,xls,xlm,csv,ods")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please indicate the sheet name")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Indicate the specific sheet that should be retrieved.")]
        //[SampleUsage("**Sheet1** or **mySheet** or **{{{vSheet}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Sheet", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Sheet")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
        public string v_SheetName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnType))]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnNameOrIndex))]
        [PropertyValidationRule("Column Index", PropertyValidationRule.ValidationRuleFlags.None)]
        public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowLocation))]
        [PropertyIsOptional(true, "1")]
        [PropertyValidationRule("Row Index", PropertyValidationRule.ValidationRuleFlags.None)]
        public string v_RowIndex { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Indicate if Header Row Exists")]
        //[PropertyUISelectionOption("Yes")]
        //[PropertyUISelectionOption("No")]    
        //[InputSpecification("Select the necessary indicator")]
        //[SampleUsage("Select **Yes**, **No**.  Data will be loaded as column headers if **YES** is selected.")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Use the First Row as the Column Names (Value Type is Cell only)")]
        [PropertyIsOptional(true, "No")]
        public string v_ContainsHeaderRow { get; set; }

        public LoadDataTableCommand()
        {
            this.CommandName = "LoadDataTableCommand";
            this.SelectionName = "Load DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_ContainsHeaderRow = "Yes";
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //DataTableControls dataSetCommand = new DataTableControls();

            //string filePath = FilePathControls.FormatFilePath_NoFileCounter(v_FilePath, engine, new List<string>() { "xlsx", "xlsm", "xls" }, true);
            //var filePath = v_FilePath.ExpandValueOrUserVariable(engine);

            //DataTable requiredData = DataTableControls.CreateDataTable(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + $@";Extended Properties=""Excel 12.0;HDR={v_ContainsHeaderRow.ExpandValueOrUserVariable(engine)};IMEX=1""", "Select * From [" + v_SheetName.ExpandValueOrUserVariable(engine) + "$]");

            //requiredData.StoreInUserVariable(engine, v_DataSetName);

            var instanceName = engine.GetNewAppInstanceName();

            var createInstance = new ExcelCreateExcelInstanceCommand()
            {
                v_InstanceName = instanceName,
            };
            createInstance.RunCommand(engine);

            var openFile = new ExcelOpenWorkbookCommand()
            {
                v_InstanceName = instanceName,
                v_FilePath = this.v_FilePath,
            };
            openFile.RunCommand(engine);

            var activateSheet = new ExcelActivateWorksheetCommand()
            {
                v_InstanceName = instanceName,
                v_SheetName = this.v_SheetName,
            };
            activateSheet.RunCommand(engine);

            var columnType = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ColumnType), "Column Type", engine);
            var columnIndex = this.ExpandValueOrUserVariable(nameof(v_ColumnIndex), "Column Index", engine);
            if (string.IsNullOrEmpty(columnIndex))
            {
                switch (columnType)
                {
                    case "range":
                        columnIndex = "A";
                        break;
                    case "rc":
                        columnIndex = "1";
                        break;
                }
            }
            var rowIndex = this.ExpandValueOrUserVariableAsInteger(nameof(v_RowIndex), "Row Index", engine);

            var convertDataTable = new ExcelGetRangeValuesAsDataTableCommand()
            {
                v_InstanceName = instanceName,
                v_RowStart = rowIndex.ToString(),
                v_ColumnStart = columnIndex,
                v_ColumnType = this.v_ColumnType,
                v_Result = this.v_DataSetName,
                v_FirstRowAsColumnName = this.v_ContainsHeaderRow,
            };
            convertDataTable.RunCommand(engine);

            var closeInstance = new ExcelCloseExcelInstanceCommand()
            {
                v_InstanceName = instanceName,
            };
            closeInstance.RunCommand(engine);
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