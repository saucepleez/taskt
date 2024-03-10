using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("File/Book")]
    [Attributes.ClassAttributes.Description("This command Open a File and Get Cell Values as a Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Open a File and Get Cell Values as a Dictionary")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_dictionary))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class LoadDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the Dictionary Variable Name")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter a name for a Dictionary.")]
        //[SampleUsage("**myDictionary** or **{{{vDictionary}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyValidationRule("Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Dictionary")]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_OutputDictionaryName))]
        public string v_DictionaryName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please indicate the Workbook File Path")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        //[InputSpecification("Enter or Select the path to the applicable file that should be loaded into the Dictionary.")]
        //[SampleUsage("**C:\\temp\\myfile.xlsx** or **{{{vFilePath}}}**")]
        //[Remarks("If file does not contain extension, supplement extensions supported by Excel.\nIf file does not contain folder path, file will be opened in the same folder as script file.")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("File Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "File Path")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_FilePath))]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "xlsx,xlsm,xls,xlm,csv,ods")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please indicate the Sheet Name")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter the sheet name of the workbook to be read.")]
        //[SampleUsage("**Sheet1** or **{{{vSheet}}}**")]
        //[Remarks("Sheet has one table")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Sheet Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Sheet Name")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
        public string v_SheetName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnType))]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please indicate the Key Column")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter the key column name to create a Dictionary off of.")]
        //[SampleUsage("**Key** or **{{{vKeyColumn}}}**")]
        //[Remarks("This value is NOT Column Index Value like A, B. Please specify table column name.")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Key Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Key Column")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnNameOrIndex))]
        [PropertyDescription("Key Column")]
        public string v_KeyColumn { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please indicate the Value Column")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter a value column name to create a Dictionary off of.")]
        //[SampleUsage("**Value** or **{{{vValueColumn}}}**")]
        //[Remarks("This value is NOT Column Index Value like A, B. Please specify table column name.")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Value Colmun", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Value Column")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnNameOrIndex))]
        [PropertyDescription("Value Column")]
        public string v_ValueColumn { get; set; }

        public LoadDictionaryCommand()
        {
            this.CommandName = "LoadDictionaryCommand";
            this.SelectionName = "Load Dictionary";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            ////var vInstance = DateTime.Now.ToString();

            //var vSheet = v_SheetName.ExpandValueOrUserVariable(engine);
            //var vKeyColumn = v_KeyColumn.ExpandValueOrUserVariable(engine);
            //var vValueColumn = v_ValueColumn.ExpandValueOrUserVariable(engine);

            ////string vFilePath = FilePathControls.FormatFilePath_NoFileCounter(v_FilePath, engine, new List<string>() { "xlsx", "xlsm", "xls" }, true);
            //var vFilePath = v_FilePath.ExpandValueOrUserVariable(engine);

            ////Query required from workbook using OLEDB
            ////DataTableControls dataSetCommand = new DataTableControls();

            //string param = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""" + vFilePath + @""";Extended Properties=""Excel 12.0;HDR=YES;IMEX=1""";
            //string queue = "Select " + vKeyColumn + "," + vValueColumn + " From " + "[" + vSheet + "$]";

            //// DBG
            ////MessageBox.Show(param + "\n" + queue);

            //DataTable requiredData = DataTableControls.CreateDataTable(param, queue);
            //var dictlist = requiredData.AsEnumerable().Select(x => new
            //{
            //    keys = (string)x[v_KeyColumn],
            //    values = (string)x[v_ValueColumn]
            //}).ToList();
            //Dictionary<string, string> outputDictionary = new Dictionary<string, string>();
            //foreach (var dict in dictlist)
            //{
            //    outputDictionary.Add(dict.keys, dict.values);
            //}

            //outputDictionary.StoreInUserVariable(engine, v_DictionaryName);

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

            var keyListName = VariableNameControls.GetInnerVariableName(0, engine);
            var getListKey = new ExcelGetColumnValuesAsListCommand()
            {
                v_InstanceName = instanceName,
                v_ColumnType = this.v_ColumnType,
                v_ColumnIndex = this.v_KeyColumn,
                v_Result = keyListName,
            };
            getListKey.RunCommand(engine);

            var valueListName = VariableNameControls.GetInnerVariableName(1, engine);
            var getListValue = new ExcelGetColumnValuesAsListCommand()
            {
                v_InstanceName = instanceName,
                v_ColumnType = this.v_ColumnType,
                v_ColumnIndex = this.v_ValueColumn,
                v_Result = valueListName,
            };
            getListValue.RunCommand(engine);

            var closeInstance = new ExcelCloseExcelInstanceCommand()
            {
                v_InstanceName = instanceName,
            };
            closeInstance.RunCommand(engine);

            var keyList = keyListName.ExpandUserVariableAsList(engine);
            var valueList = valueListName.ExpandUserVariableAsList(engine);

            var myDic = new Dictionary<string, string>();
            foreach(var key in keyList)
            {
                myDic.Add(key, "");
            }
            int max = (keyList.Count <= valueList.Count) ? keyList.Count : valueList.Count; 
            for (int i = 0; i < max; i++) 
            {
                myDic[keyList[i]] = valueList[i];
            }

            myDic.StoreInUserVariable(engine, v_DictionaryName);
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
        //    return base.GetDisplayValue() + " [Load Dictionary from '" + v_FilePath + "' and store in: '" +v_DictionaryName+ "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(v_DictionaryName))
        //    {
        //        this.IsValid = false;
        //        this.validationResult += "Dictionary Variable Name is empty.\n";
        //    }
        //    if (String.IsNullOrEmpty(v_FilePath))
        //    {
        //        this.IsValid = false;
        //        this.validationResult += "Workbook file path is empty.\n";
        //    }
        //    if (String.IsNullOrEmpty(v_SheetName))
        //    {
        //        this.IsValid = false;
        //        this.validationResult += "Sheet name is empty.\n";
        //    }
        //    if (String.IsNullOrEmpty(v_KeyColumn))
        //    {
        //        this.IsValid = false;
        //        this.validationResult += "Key column is empty.\n";
        //    }
        //    if (String.IsNullOrEmpty(v_ValueColumn))
        //    {
        //        this.IsValid = false;
        //        this.validationResult += "Value column is empty.\n";
        //    }

        //    return this.IsValid;
        //}
    }
}