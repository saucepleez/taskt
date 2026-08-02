using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for excel methods
    /// </summary>
    internal static class ExcelControls
    {
        #region const, field
        /// <summary>
        /// internal current worksheet keyword
        /// </summary>
        public const string INTERNAL_EXCEL_CURRENT_WORKSHEET_KEYWORD = "%kwd_excel_current_worksheet%";
        /// <summary>
        /// internal next worksheet keyword
        /// </summary>
        public const string INTERNAL_EXCEL_NEXT_WORKSHEET_KEYWORD = "%kwd_excel_next_worksheet%";
        /// <summary>
        /// internal previous worksheet keyword
        /// </summary>
        public const string INTERNAL_EXCEL_PREVIOUS_WORKSHEET_KEYWORD = "%kwd_excel_previous_worksheet%";
        /// <summary>
        /// white color value
        /// </summary>
        public const long EXCEL_WHITE_COLOR = 16777215;
        /// <summary>
        /// black color valur
        /// </summary>
        public const long EXCEL_BLACK_COLOR = 0;
        /// <summary>
        /// default format (NumberFormat)
        /// </summary>
        public const string EXCEL_DEFAULT_FORMAT = "General";

        /// <summary>
        /// disallow contains worksheet charactors
        /// </summary>
        public static readonly List<string> Disallow_Contains_Worksheet_Charactors = new List<string>()
        {
            "/", "\\", "?", "*",
            ":", "[", "]",
        };
        /// <summary>
        /// disallow starts/ends worksheet charactors
        /// </summary>
        public static readonly List<string> Disallow_Starts_Ends_Worksheet_Charactors = new List<string>()
        {
            "'",
        };

        /// <summary>
        /// excel support file extensions for FilePathSettings
        /// </summary>
        public const string EXCEL_SUPPORT_FILE_EXTENSIONS = "xlsx,xls,xlsm,xlsb,xlw,xlr,xml,prn,csv,txt";
        #endregion

        #region virtual property
        /// <summary>
        /// excel instance property
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_InputInstanceName))]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Excel Instance Name", true)]
        [PropertyDetailSampleUsage("**RPAExcel**", PropertyDetailSampleUsage.ValueType.Value, "Excel Instance Name")]
        [PropertyDetailSampleUsage("**{{{vInstance}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Excel Instance Name")]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        [Remarks("Please specify the Excel Instance Name created by **Create Excel Instance** command in advance.")]
        [PropertyFirstValue("%kwd_default_excel_instance%")]
        [PropertyValidationRule("Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Instance")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyParameterOrder(5000)]
        public static string v_InputInstanceName { get; }

        /// <summary>
        /// worksheet name property
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Worksheet Name")]
        [InputSpecification("Worksheet Name", true)]
        [PropertyDetailSampleUsage("**mySheet**", PropertyDetailSampleUsage.ValueType.Value, "Worksheet Name")]
        [PropertyDetailSampleUsage("**{{{vSheet}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Worksheet Name")]
        [PropertyDetailSampleUsage("**%kwd_excel_current_worksheet%**", "Specify Current Worksheet Name")]
        [PropertyValidationRule("Worksheet", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Worksheet")]
        [PropertyAvailableSystemVariable(Engine.SystemVariables.LimitedSystemVariableNames.Excel_Worksheet)]
        //[Remarks("")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyParameterOrder(5000)]
        public static string v_SheetName { get; }

        /// <summary>
        /// cell range location
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Cell Location")]
        [InputSpecification("Cell Location like **A1**", true)]
        [PropertyDetailSampleUsage("**A1**", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyDetailSampleUsage("**B10**", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyDetailSampleUsage("**{{{vAddress}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Cell Location")]
        [PropertyValidationRule("Cell Location", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Cell")]
        //[Remarks("")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyParameterOrder(5000)]
        public static string v_CellRangeLocation { get; }

        /// <summary>
        /// row location property
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Row Location")]
        [InputSpecification("Number", true)]
        [PropertyDetailSampleUsage("**1**", "Specify the First Row")]
        [PropertyDetailSampleUsage("**2**", PropertyDetailSampleUsage.ValueType.Value, "Row Location")]
        [PropertyDetailSampleUsage("**{{{vRow}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Row Location")]
        [PropertyValidationRule("Row", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Row")]
        //[Remarks("")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyParameterOrder(5000)]
        public static string v_RowLocation { get; }

        /// <summary>
        /// column location property
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Column Location")]
        [InputSpecification("Number", true)]
        [PropertyDetailSampleUsage("**1**", "Specify the First Column")]
        [PropertyDetailSampleUsage("**2**", PropertyDetailSampleUsage.ValueType.Value, "Column Location")]
        [PropertyDetailSampleUsage("**{{{vColumn}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Column Location")]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Column")]
        //[Remarks("")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyParameterOrder(5000)]
        public static string v_ColumnLocation { get; }

        /// <summary>
        /// cell value type property
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Value Type")]
        [PropertyDetailSampleUsage("**Cell**", "Specify the Cell Value")]
        [PropertyDetailSampleUsage("**Formula**", "Specify the Cell Formula, like **=SUM(A1:A10)**")]
        [PropertyDetailSampleUsage("**Format**", "Specify the Cell Format")]
        [PropertyDetailSampleUsage("**Font Color**", "Specify the Cell Text Color")]
        [PropertyDetailSampleUsage("**Back Color**", "Specify the Cell Background Color")]
        [PropertyUISelectionOption("Cell")]
        [PropertyUISelectionOption("Formula")]
        [PropertyUISelectionOption("Format")]
        [PropertyUISelectionOption("Font Color")]
        [PropertyUISelectionOption("Back Color")]
        [PropertyIsOptional(true, "Cell")]
        [PropertyDisplayText(true, "Type")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[InputSpecification("", true)]
        //[Remarks("")]
        //[PropertyParameterOrder(5000)]
        public static string v_ValueType { get; }

        /// <summary>
        /// cell value type property for check cell commands
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Value Type")]
        [PropertyUISelectionOption("Cell")]
        [PropertyUISelectionOption("Formula")]
        [PropertyUISelectionOption("Back Color")]
        [PropertyIsOptional(true, "Cell")]
        [PropertySecondaryLabel(true)]
        [PropertyAddtionalParameterInfo("Cell", "Check the Cell has Value or Not")]
        [PropertyAddtionalParameterInfo("Formula", "Check the Cell has Formula or Not")]
        [PropertyAddtionalParameterInfo("Back Color", "Check the Cell Background Color is Not White")]
        [PropertyDisplayText(true, "Type")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[InputSpecification("", true)]
        //[Remarks("")]
        //[PropertyParameterOrder(5000)]
        public static string v_CheckableValueType { get; }

        /// <summary>
        /// column type propertys
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Column Type")]
        [PropertyDetailSampleUsage("**Range**", "Use Range, like **A**. It means first column.")]
        [PropertyDetailSampleUsage("**RC**", "Use Row-Column, like **1**. It means first column.")]
        [PropertyIsOptional(true, "Range")]
        [PropertyUISelectionOption("Range")]
        [PropertyUISelectionOption("RC")]
        [PropertyDisplayText(true, "Column Type")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertySelectionValueSensitive(false)]
        //[InputSpecification("")]
        //[Remarks("")]
        //[PropertyParameterOrder(5000)]
        public static string v_ColumnType { get; }

        /// <summary>
        /// column location or index
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Column Location or Index")]
        [InputSpecification("Column Location (Text) or Index (Number)", true)]
        [PropertyDetailSampleUsage("**A**", "Specify the First Column when **Range** is specified for Column Type.")]
        [PropertyDetailSampleUsage("**1**", "Specify the First Column when **RC** is specified for Column Type.")]
        [PropertyDetailSampleUsage("**{{{vColumn}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Column")]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        [PropertyDisplayText(true, "Column")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[Remarks("")]
        //[PropertyParameterOrder(5000)]
        public static string v_ColumnNameOrIndex { get; }

        /// <summary>
        /// start row index
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Start Row Index")]
        [InputSpecification("Number", true)]
        [PropertyDetailSampleUsage("**1**", "Specify the First Row Index for Start Row")]
        [PropertyDetailSampleUsage("**2**", PropertyDetailSampleUsage.ValueType.Value, "Start Row")]
        [PropertyDetailSampleUsage("**{{{vRow}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Start Row")]
        [PropertyIsOptional(true, "1")]
        [PropertyDisplayText(true, "Start Row")]
        //[Remarks("")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyParameterOrder(5000)]
        public static string v_RowStart { get; }

        /// <summary>
        /// end row
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("End Row Index")]
        [InputSpecification("Number", true)]
        [PropertyDetailSampleUsage("**1**", "Specify the First Row Index for End Row")]
        [PropertyDetailSampleUsage("**2**", PropertyDetailSampleUsage.ValueType.Value, "End Row")]
        [PropertyDetailSampleUsage("**{{{vRow}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "End Row")]
        [Remarks("When End Row Index is Empty, Automatically specifies the Last Row where values are entered consecutively")]
        [PropertyIsOptional(true, "Last Row")]
        [PropertyDisplayText(true, "End Row")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyParameterOrder(5000)]
        public static string v_RowEnd { get; }

        /// <summary>
        /// column start property
        /// </summary>
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnNameOrIndex))]
        [PropertyDescription("Start Column Location or Index")]
        [PropertyValidationRule("Start Column", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        [PropertyDisplayText(true, "Start Column")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Column Location or Index", true)]
        //[PropertyDetailSampleUsage("**A**", "Specify the First Column when **Range** is specified for Column Type.")]
        //[PropertyDetailSampleUsage("**1**", "Specify the First Column when **RC** is specified for Column Type.")]
        //[PropertyDetailSampleUsage("**{{{vColumn}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Column")]
        //[Remarks("")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyParameterOrder(5000)]
        public static string v_ColumnStart { get; }

        /// <summary>
        /// column end property
        /// </summary>
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnNameOrIndex))]
        [PropertyDescription("End Column Location or Index")]
        [PropertyIsOptional(true, "Last Column")]
        [PropertyDisplayText(true, "End Column")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Column Location or Index", true)]
        //[PropertyDetailSampleUsage("**A**", "Specify the First Column when **Range** is specified for Column Type.")]
        //[PropertyDetailSampleUsage("**1**", "Specify the First Column when **RC** is specified for Column Type.")]
        //[PropertyDetailSampleUsage("**{{{vColumn}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Column")]
        //[Remarks("")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyParameterOrder(5000)]
        public static string v_ColumnEnd { get; }

        /// <summary>
        /// when items not enough
        /// </summary>
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_ComboBoxHasErrorIgnore))]
        [PropertyDescription("When A Items Not Enough")]
        [PropertyIsOptional(true, "Ignore")]
        //[InputSpecification("", true)]
        //[PropertyDetailSampleUsage("**Ignore**", "Don't Set the Value")]
        //[PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyUISelectionOption("Ignore")]
        //[PropertyUISelectionOption("Error")]
        //[PropertyParameterOrder(5000)]
        public static string v_WhenItemNotEnough { get; }

        /// <summary>
        /// excel open file path
        /// </summary>
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_NoSample_FilePath))]
        [PropertyDescription("Workbook (Excel File) Path")]
        [InputSpecification("Excel File Path", true)]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, EXCEL_SUPPORT_FILE_EXTENSIONS)]
        [PropertyDetailSampleUsage("**C:\\temp\\myfile.xlsx**", PropertyDetailSampleUsage.ValueType.Value, "File Path")]
        [PropertyDetailSampleUsage("**{{{vFilePath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Path")]
        [Remarks("If file does not contain extension, supplement extensions supported by Excel.\nIf file does not contain folder path, file will be opened in the same folder as script file.")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyValidationRule("File", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "File")]
        //[PropertyParameterOrder(5000)]
        public static string v_OpenFilePath { get; }

        /// <summary>
        /// excel save file path
        /// </summary>
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_OpenFilePath))]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport, EXCEL_SUPPORT_FILE_EXTENSIONS)]
        public static string v_SaveFilePath { get; }

        /// <summary>
        /// value to set
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_OneLineTextBox))]
        [PropertyDescription("Value to Set")]
        [InputSpecification("Text or Number", true)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Value to Set")]
        [PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableName, "Value to Set")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[Remarks("")]
        //[PropertyDisplayText(true, "Value")]
        //[PropertyTextBoxSetting(1, true)]
        //[PropertyParameterOrder(5000)]
        public static string v_ValueToSet { get; }

        /// <summary>
        /// chart name
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Chart Name")]
        [InputSpecification("Text", true)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**Chart 1**", PropertyDetailSampleUsage.ValueType.Value, "Chart Name")]
        [PropertyDetailSampleUsage("**{{{vChart}}}**", PropertyDetailSampleUsage.ValueType.VariableName, "Chart Name")]
        [PropertyValidationRule("Chart Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Chart Name")]
        public static string v_ChartName { get; }
        #endregion

        #region keyword convert method

        /// <summary>
        /// Replace Internal Keywords to SystemVariable Names
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string ReplaceKeywordsToSystemVariable(string txt, Engine.AutomationEngineInstance engine)
        {
            return txt.Replace(INTERNAL_EXCEL_CURRENT_WORKSHEET_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Excel_CurrentWorkSheet.VariableName, engine))
                        .Replace(INTERNAL_EXCEL_NEXT_WORKSHEET_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Excel_NextWorkSheet.VariableName, engine))
                        .Replace(INTERNAL_EXCEL_PREVIOUS_WORKSHEET_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Excel_PreviousWorkSheet.VariableName, engine));
        }

        /// <summary>
        /// Replace Internal Keywords to SystemVariable Names
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string ReplaceKeywordsToSystemVariable(string txt, IApplicationSettings settings)
        {
            return txt.Replace(INTERNAL_EXCEL_CURRENT_WORKSHEET_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Excel_CurrentWorkSheet.VariableName, settings))
                        .Replace(INTERNAL_EXCEL_NEXT_WORKSHEET_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Excel_NextWorkSheet.VariableName, settings))
                        .Replace(INTERNAL_EXCEL_PREVIOUS_WORKSHEET_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Excel_PreviousWorkSheet.VariableName, settings));
        }

        #endregion

        #region instance, worksheet methods
        ///// <summary>
        ///// expand value or UserVariable as Excel Instance
        ///// </summary>
        ///// <param name="instanceName"></param>
        ///// <param name="engine"></param>
        ///// <returns></returns>
        ///// <exception cref="Exception">value is not Excel Instance</exception>
        //public static Application ExpandValueOrUserVariableAsExcelInstance(this string instanceName, Automation.Engine.AutomationEngineInstance engine)
        //{
        //    string ins = instanceName.ExpandValueOrUserVariable(engine);
        //    var instanceObject = engine.GetAppInstance(ins);
        //    if (instanceObject is Application app)
        //    {
        //        return app;
        //    }
        //    else
        //    {
        //        throw new Exception("Instance '" + instanceName + "' is not Excel Instance");
        //    }
        //}

        ///// <summary>
        ///// expancd value or UserVariable as Excel instance and worksheet
        ///// </summary>
        ///// <param name="instanceName"></param>
        ///// <param name="engine"></param>
        ///// <returns></returns>
        ///// <exception cref="">value is not Excel Instance</exception>
        //public static (Application instance, Worksheet sheet) ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(this string instanceName, Automation.Engine.AutomationEngineInstance engine)
        //{
        //    var instanceObject = instanceName.ExpandValueOrUserVariableAsExcelInstance(engine);
        //    return (instanceObject, GetCurrentWorksheet(instanceObject));
        //}

        ///// <summary>
        ///// expand value or UserVariable as Excel instance and worksheet
        ///// </summary>
        ///// <param name="info"></param>
        ///// <param name="engine"></param>
        ///// <param name="returnNullIfSheetDoesNotExists"></param>
        ///// <returns></returns>
        //public static (Application instance, Worksheet sheet) ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(this string instanceName, string sheetName, Automation.Engine.AutomationEngineInstance engine, bool returnNullIfSheetDoesNotExists = false)
        //{
        //    var instanceObject = instanceName.ExpandValueOrUserVariableAsExcelInstance(engine);
        //    var sheet = sheetName.ExpandValueOrUserVariableAsExcelWorksheet(engine, instanceObject, returnNullIfSheetDoesNotExists);
        //    return (instanceObject, sheet);
        //}

        ///// <summary>
        ///// expand value or user variable as Excel Worksheet
        ///// </summary>
        ///// <param name="sheetVariable"></param>
        ///// <param name="engine"></param>
        ///// <param name="excelInstance"></param>
        ///// <param name="returnNullIfSheetDoesNotExists"></param>
        ///// <returns></returns>
        ///// <exception cref="Exception">worksheet does not exists</exception>
        //public static Worksheet ExpandValueOrUserVariableAsExcelWorksheet(this string sheetVariable, Automation.Engine.AutomationEngineInstance engine, Application excelInstance, bool returnNullIfSheetDoesNotExists = false)
        //{
        //    var sheet = sheetVariable.ExpandValueOrUserVariable(engine);
        //    if (sheet == engine.engineSettings.CurrentWorksheetKeyword)
        //    {
        //        try
        //        {
        //            //return (Worksheet)excelInstance.ActiveSheet;
        //            return GetCurrentWorksheet(excelInstance);
        //        }
        //        catch
        //        {
        //            if (returnNullIfSheetDoesNotExists)
        //            {
        //                return null;
        //            }
        //            else
        //            {
        //                throw new Exception("No Worksheet exists.");
        //            }
        //        }
        //    }
        //    else if (sheet == engine.engineSettings.NextWorksheetKeyword)
        //    {
        //        try
        //        {
        //            return GetNextWorksheet(excelInstance);
        //        }
        //        catch
        //        {
        //            if (returnNullIfSheetDoesNotExists)
        //            {
        //                return null;
        //            }
        //            else
        //            {
        //                throw new Exception("Next Worksheet not found.");
        //            }
        //        }
        //    }
        //    else if (sheet == engine.engineSettings.PreviousWorksheetKeyword)
        //    {
        //        try
        //        {
        //            return GetPreviousWorksheet(excelInstance);
        //        }
        //        catch
        //        {
        //            if (returnNullIfSheetDoesNotExists)
        //            {
        //                return null;
        //            }
        //            else
        //            {
        //                throw new Exception("Previous Worksheet not found.");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        try
        //        {
        //            return (Worksheet)excelInstance.Worksheets[sheet];
        //        }
        //        catch
        //        {
        //            if (returnNullIfSheetDoesNotExists)
        //            {
        //                return null;
        //            }
        //            else
        //            {
        //                throw new Exception("Worksheet " + sheet + " does not exists.");
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// get current worksheet
        ///// </summary>
        ///// <param name="excelInstance"></param>
        ///// <returns></returns>
        //private static Worksheet GetCurrentWorksheet(Application excelInstance)
        //{
        //    if (excelInstance.Sheets.Count == 0)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return excelInstance.ActiveSheet;
        //    }
        //}

        ///// <summary>
        ///// get next worksheet
        ///// </summary>
        ///// <param name="excelInstance"></param>
        ///// <param name="mySheet"></param>
        ///// <returns></returns>
        //private static Worksheet GetNextWorksheet(Application excelInstance, Worksheet mySheet = null)
        //{
        //    Worksheet currentSheet;
        //    if (mySheet == null)
        //    {
        //        try
        //        {
        //            //currentSheet = (Worksheet)excelInstance.ActiveSheet;
        //            currentSheet = GetCurrentWorksheet(excelInstance);
        //        }
        //        catch
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        currentSheet = mySheet;
        //    }

        //    int idx = 1;
        //    foreach (Worksheet sht in excelInstance.Worksheets)
        //    {
        //        if (sht.Name == currentSheet.Name)
        //        {
        //            break;
        //        }
        //        idx++;
        //    }
        //    if (idx < excelInstance.Worksheets.Count)
        //    {
        //        return (Worksheet)excelInstance.Worksheets[idx + 1];
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// get previous worksheet
        ///// </summary>
        ///// <param name="excelInstance"></param>
        ///// <param name="mySheet"></param>
        ///// <returns></returns>
        //private static Worksheet GetPreviousWorksheet(Application excelInstance, Worksheet mySheet = null)
        //{
        //    Worksheet currentSheet;
        //    if (mySheet == null)
        //    {
        //        try
        //        {
        //            //currentSheet = (Worksheet)excelInstance.ActiveSheet;
        //            currentSheet = GetCurrentWorksheet(excelInstance);
        //        }
        //        catch
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        currentSheet = mySheet;
        //    }

        //    int idx = 1;
        //    foreach (Worksheet sht in excelInstance.Worksheets)
        //    {
        //        if (sht.Name == currentSheet.Name)
        //        {
        //            break;
        //        }
        //        idx++;
        //    }
        //    if (idx > 1)
        //    {
        //        return (Worksheet)excelInstance.Worksheets[idx - 1];
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        #endregion

        #region Func methods

        ///// <summary>
        ///// get CheckCellValueFunction
        ///// </summary>
        ///// <param name="parameterName"></param>
        ///// <param name="command"></param>
        ///// <param name="engine"></param>
        ///// <returns></returns>
        //public static Func<Range, bool> CheckCellValueFunctionFromRange(string parameterName, ScriptCommand command, Engine.AutomationEngineInstance engine)
        //{
        //    var valueType = command.ExpandValueOrUserVariableAsSelectionItem(parameterName, engine);
        //    Func<Range, bool> func = null;
        //    switch (valueType)
        //    {
        //        case "cell":
        //            func = (rg) => { return !String.IsNullOrEmpty((string)rg.Text); };
        //            break;

        //        case "formula":
        //            func = (rg) => { return ((string)rg.Formula).StartsWith("="); };
        //            break;

        //        case "back color":
        //            func = (rg) => { return ((long)rg.Interior.Color) != 16777215; };
        //            break;
        //    }
        //    return func;
        //}

        //public static Func<Range, string> GetCellValueFunctionFromRange(string valueType)
        //{
        //    Func<Range, string> getFunc = null;
        //    switch (valueType)
        //    {
        //        case "cell":
        //            getFunc = (rg) =>
        //            {
        //                return (string)rg.Text;
        //            };
        //            break;
        //        case "formula":
        //            getFunc = (rg) =>
        //            {
        //                return (string)rg.Formula;
        //            };
        //            break;
        //        case "format":
        //            getFunc = (rg) =>
        //            {
        //                return (string)rg.NumberFormatLocal;
        //            };
        //            break;
        //        case "fore color":
        //            getFunc = (rg) =>
        //            {
        //                return ((long)rg.Font.Color).ToString();
        //            };
        //            break;
        //        case "back color":
        //            getFunc = (rg) =>
        //            {
        //                return ((long)rg.Interior.Color).ToString();
        //            };
        //            break;
        //    }
        //    return getFunc;
        //}

        //public static Func<Worksheet, int, int, string> GetCellValueFunction(string valueType)
        //{
        //    Func<Worksheet, int, int, string> getFunc = null;
        //    switch (valueType)
        //    {
        //        case "cell":
        //            getFunc = (sheet, column, row) =>
        //            {
        //                return (string)((Range)sheet.Cells[row, column]).Text;
        //            };
        //            break;
        //        case "formula":
        //            getFunc = (sheet, column, row) =>
        //            {
        //                return (string)((Range)sheet.Cells[row, column]).Formula;
        //            };
        //            break;
        //        case "format":
        //            getFunc = (sheet, column, row) =>
        //            {
        //                return (string)((Range)sheet.Cells[row, column]).NumberFormatLocal;
        //            };
        //            break;
        //        case "fore color":
        //            getFunc = (sheet, column, row) =>
        //            {
        //                return ((long)((Range)sheet.Cells[row, column]).Font.Color).ToString();
        //            };
        //            break;
        //        case "back color":
        //            getFunc = (sheet, column, row) =>
        //            {
        //                return ((long)((Range)sheet.Cells[row, column]).Interior.Color).ToString();
        //            };
        //            break;
        //    }
        //    return getFunc;
        //}

        //public static Action<string, Worksheet, Range> SetCellValueFunctionFromRange(string valueType)
        //{
        //    Action<string, Worksheet, Range> setFunc = null;

        //    Func<string, long> longConvert = (str) =>
        //    {
        //        if (long.TryParse(str, out long v))
        //        {
        //            return v;
        //        }
        //        else
        //        {
        //            throw new Exception("Value '" + str + "' is not color.");
        //        }
        //    };

        //    switch (valueType)
        //    {
        //        case "cell":
        //            setFunc = (value, sheet, rg) =>
        //            {
        //                rg.Value = value;
        //            };
        //            break;
        //        case "formula":
        //            setFunc = (value, sheet, rg) =>
        //            {
        //                rg.Formula = value;
        //            };
        //            break;
        //        case "format":
        //            setFunc = (value, sheet, rg) =>
        //            {
        //                rg.NumberFormatLocal = value;
        //            };
        //            break;
        //        case "fore color":
        //            setFunc = (value, sheet, rg) =>
        //            {
        //                rg.Font.Color = longConvert(value);
        //            };
        //            break;
        //        case "back color":
        //            setFunc = (value, sheet, rg) =>
        //            {
        //                rg.Interior.Color = longConvert(value);
        //            };
        //            break;
        //    }

        //    return setFunc;
        //}

        //public static Action<string, Worksheet, int, int> SetCellValueFunction(string valueType)
        //{
        //    Action<string, Worksheet, int, int> setFunc = null;
        //    switch (valueType)
        //    {
        //        case "cell":
        //            setFunc = (value, sheet, column, row) =>
        //            {
        //                ((Range)sheet.Cells[row, column]).Value = value;
        //            };
        //            break;
        //        case "formula":
        //            setFunc = (value, sheet, column, row) =>
        //            {
        //                ((Range)sheet.Cells[row, column]).Formula = value;
        //            };
        //            break;
        //        case "format":
        //            setFunc = (value, sheet, column, row) =>
        //            {
        //                ((Range)sheet.Cells[row, column]).NumberFormatLocal = value;
        //            };
        //            break;
        //        case "fore color":
        //            setFunc = (value, sheet, column, row) =>
        //            {
        //                ((Range)sheet.Cells[row, column]).Font.Color = long.Parse(value);
        //            };
        //            break;
        //        case "back color":
        //            setFunc = (value, sheet, column, row) =>
        //            {
        //                ((Range)sheet.Cells[row, column]).Interior.Color = long.Parse(value);
        //            };
        //            break;
        //    }

        //    return setFunc;
        //}
        #endregion

        #region cell-range methods

        //public static int GetColumnIndex(Worksheet sheet, string columnName)
        //{
        //    if (CheckCorrectColumnName(columnName, sheet))
        //    {
        //        return ((Range)sheet.Columns[columnName]).Column;
        //    }
        //    else
        //    {
        //        throw new Exception("Strange Column Name '" + columnName + "'");
        //    }
        //}

        //public static string GetColumnName(Worksheet sheet, int columnIndex)
        //{
        //    if (columnIndex < 1)
        //    {
        //        return "";
        //    }
        //    else
        //    {
        //        return ((Range)sheet.Cells[1, columnIndex]).Address.Split('$')[1];
        //    }
        //}

        //public static string GetAddress(Worksheet sheet, int row, int column)
        //{
        //    if (CheckCorrectRC(row, column, sheet))
        //    {
        //        return ((Range)sheet.Cells[row, column]).Address.Replace("$", "");
        //    }
        //    else
        //    {
        //        throw new Exception("Strange Excel Location. Row: " + row + ", Column: " + column);
        //    }
        //}

        //public static int GetLastRowIndex(Worksheet sheet, string column, int startRow, string targetType)
        //{
        //    return GetLastRowIndex(sheet, GetColumnIndex(sheet, column), startRow, targetType);
        //}

        //public static int GetLastRowIndex(Worksheet sheet, int column, int startRow, string targetType)
        //{
        //    int lastRow = startRow;
        //    switch (targetType.ToLower())
        //    {
        //        case "formula":
        //            while ((string)(((Range)sheet.Cells[lastRow, column]).Formula) != "")
        //            {
        //                lastRow++;
        //            }
        //            break;

        //        default:
        //            while((string)(((Range)sheet.Cells[lastRow, column]).Text) != "")
        //            {
        //                lastRow++;
        //            }
        //            break;
        //    }
        //    return --lastRow;
        //}

        //public static int GetLastColumnIndex(Worksheet sheet, int row, string startColum, string targetType)
        //{
        //    return GetLastColumnIndex(sheet, row, GetColumnIndex(sheet, startColum), targetType);
        //}

        //public static int GetLastColumnIndex(Worksheet sheet, int row, int startColum, string targetType)
        //{
        //    int lastColumn = startColum;
        //    switch (targetType.ToLower())
        //    {
        //        case "formula":
        //            while ((string)(((Range)sheet.Cells[row, lastColumn]).Formula) != "")
        //            {
        //                lastColumn++;
        //            }
        //            break;

        //        default:
        //            while ((string)(((Range)sheet.Cells[row, lastColumn]).Text) != "")
        //            {
        //                lastColumn++;
        //            }
        //            break;
        //    }
        //    return --lastColumn;
        //}

        //public static (int columnIndex, int rowStartIndex, int rowEndIndex, string valueType) GetRangeIndeiesColumnDirection(string columnValueName, string columnTypeName, string rowStartName, string rowEndName, string valueTypeName, Automation.Engine.AutomationEngineInstance engine, Worksheet excelSheet, ScriptCommand command, object targetObject = null)
        //{
        //    string columnType = command.ExpandValueOrUserVariableAsSelectionItem(columnTypeName, "Column Type", engine);

        //    int columnIndex = 0;
        //    switch (columnType)
        //    {
        //        case "range":
        //            string col = command.ExpandValueOrUserVariable(columnValueName, "Column", engine);
        //            columnIndex = GetColumnIndex(excelSheet, col);
        //            break;
        //        case "rc":
        //            columnIndex = command.ExpandValueOrUserVariableAsInteger(columnValueName, "Column", engine);
        //            break;
        //    }

        //    string valueType = command.ExpandValueOrUserVariableAsSelectionItem(valueTypeName, "Value Type", engine);

        //    int rowStartIndex = command.ExpandValueOrUserVariableAsInteger(rowStartName, "Start Row", engine);
        //    string rowEndValue = command.GetRawPropertyValueAsString(rowEndName, "End Row");
        //    int rowEndIndex;
        //    if (String.IsNullOrEmpty(rowEndValue))
        //    {
        //        if (targetObject == null)
        //        {
        //            rowEndIndex = GetLastRowIndex(excelSheet, columnIndex, rowStartIndex, valueType);
        //        }
        //        else
        //        {
        //            int size;
        //            if (targetObject is List<string>)
        //            {
        //                size = ((List<string>)targetObject).Count;
        //            }
        //            else if (targetObject is Dictionary<string, string>)
        //            {
        //                size = ((Dictionary<string, string>)targetObject).Count;
        //            }
        //            else if (targetObject is System.Data.DataTable)
        //            {
        //                size = ((System.Data.DataTable)targetObject).Rows.Count;
        //            }
        //            else if (targetObject is int)
        //            {
        //                size = (int)targetObject;
        //            }
        //            else
        //            {
        //                throw new Exception("target object is strange data.");
        //            }
        //            rowEndIndex = rowStartIndex + size - 1;
        //        }
        //    }
        //    else
        //    {
        //        rowEndIndex = rowEndValue.ExpandValueOrUserVariableAsInteger("End Row", engine);
        //    }

        //    if (rowStartIndex > rowEndIndex)
        //    {
        //        int t = rowStartIndex;
        //        rowStartIndex = rowEndIndex;
        //        rowEndIndex = t;
        //    }

        //    CheckCorrectRCRange(rowStartIndex, columnIndex, rowEndIndex, columnIndex, excelSheet);

        //    return (columnIndex, rowStartIndex, rowEndIndex, valueType);
        //}

        //public static (int rowIndex, int columnStartIndex, int columnEndIndex, string valueType) GetRangeIndeiesRowDirection(string rowValueName, string columnTypeName, string columnStartName, string columnEndName, string valueTypeName, Automation.Engine.AutomationEngineInstance engine, Worksheet excelSheet, ScriptCommand command, object targetObject = null)
        //{
        //    int rowIndex = command.ExpandValueOrUserVariableAsInteger(rowValueName, "Row Index", engine);

        //    string valueType = command.ExpandValueOrUserVariableAsSelectionItem(valueTypeName, "Value Type", engine);

        //    int columnStartIndex = 0;
        //    int columnEndIndex = 0;

        //    string columnStartValue = command.GetRawPropertyValueAsString(columnStartName, "Start Column");
        //    string columnEndValue = command.GetRawPropertyValueAsString(columnEndName, "End Column");

        //    Func<int> getLastRowFromObject = () =>
        //    {
        //        int size;
        //        if (targetObject is List<string>)
        //        {
        //            size = ((List<string>)targetObject).Count;
        //        }
        //        else if (targetObject is Dictionary<string, string>)
        //        {
        //            size = ((Dictionary<string, string>)targetObject).Count;
        //        }
        //        else if (targetObject is System.Data.DataTable)
        //        {
        //            size = ((System.Data.DataTable)targetObject).Columns.Count;
        //        }
        //        else if (targetObject is int)
        //        {
        //            size = (int)targetObject;
        //        }
        //        else
        //        {
        //            throw new Exception("target object is strange data.");
        //        }
        //        return columnStartIndex + size - 1;
        //    };

        //    string columnType = command.ExpandValueOrUserVariableAsSelectionItem(columnTypeName, "Column Type", engine);
        //    switch (columnType)
        //    {
        //        case "range":
        //            if (String.IsNullOrEmpty(columnStartValue))
        //            {
        //                columnStartValue = "A";
        //            }
        //            columnStartIndex = ExcelControls.GetColumnIndex(excelSheet, columnStartValue.ExpandValueOrUserVariable(engine));


        //            if (String.IsNullOrEmpty(columnEndValue))
        //            {
        //                if (targetObject == null)
        //                {
        //                    columnEndIndex = ExcelControls.GetLastColumnIndex(excelSheet, rowIndex, columnStartIndex, valueType);
        //                }
        //                else
        //                {
        //                    columnEndIndex = getLastRowFromObject();
        //                }
        //            }
        //            else
        //            {
        //                columnEndIndex = ExcelControls.GetColumnIndex(excelSheet, columnEndValue.ExpandValueOrUserVariable(engine));

        //            }
        //            break;

        //        case "rc":
        //            if (String.IsNullOrEmpty(columnStartValue))
        //            {
        //                columnStartValue = "1";
        //            }
        //            columnStartIndex = columnStartValue.ExpandValueOrUserVariableAsInteger("Start Column", engine);

        //            if (String.IsNullOrEmpty(columnEndValue))
        //            {
        //                if (targetObject == null)
        //                {
        //                    columnEndIndex = ExcelControls.GetLastColumnIndex(excelSheet, rowIndex, columnStartIndex, valueType);
        //                }
        //                else
        //                {
        //                    columnEndIndex = getLastRowFromObject();
        //                }
        //            }
        //            else
        //            {
        //                columnEndIndex = columnEndValue.ExpandValueOrUserVariableAsInteger("Column End", engine);
        //            }

        //            break;
        //    }

        //    if (columnStartIndex > columnEndIndex)
        //    {
        //        int t = columnStartIndex;
        //        columnStartIndex = columnEndIndex;
        //        columnEndIndex = t;
        //    }

        //    CheckCorrectRCRange(rowIndex, columnStartIndex, rowIndex, columnEndIndex, excelSheet);

        //    return (rowIndex, columnStartIndex, columnEndIndex, valueType);
        //}
        #endregion

        #region convert methods
        //public static string GetoExcelRangeLocation(this string value, Automation.Engine.AutomationEngineInstance engine, Application excelInstance)
        //{
        //    var location = value.ExpandValueOrUserVariable(engine);
        //    if (CheckCorrectRange(location, excelInstance))
        //    {
        //        return location;
        //    }
        //    else
        //    {
        //        throw new Exception("Location '" + value + "' is not Range. Value: '" + location + "'.");
        //    }
        //}

        //public static (int row, int column) GetExcelRCLocation(this ScriptCommand command, string rowPropertyName, string columnPropertyName, Automation.Engine.AutomationEngineInstance engine, Application excelInstance)
        //{
        //    int row = command.ExpandValueOrUserVariableAsInteger(rowPropertyName, "Row", engine);
        //    int column = command.ExpandValueOrUserVariableAsInteger(columnPropertyName, "Column", engine);
        //    if (CheckCorrectRC(row, column, excelInstance))
        //    {
        //        return (row, column);
        //    }
        //    else
        //    {
        //        throw new Exception("Invalid Location. Row: " + row + ", Column: " + column);
        //    }
        //}

        //public static Range GetExcelRange(this string location, Automation.Engine.AutomationEngineInstance engine, Application excelInstance, Worksheet excelSheet, ScriptCommand command)
        //{
        //    string pos = location.GetoExcelRangeLocation(engine, excelInstance);
        //    return excelSheet.Range[pos];
        //}

        //public static Range GetExcelRange(this ScriptCommand command, string rowPropertyName, string columnPropertyName, Automation.Engine.AutomationEngineInstance engine, Application excelInstance, Worksheet excelSheet)
        //{
        //    var rc = command.GetExcelRCLocation(rowPropertyName, columnPropertyName, engine, excelInstance);
        //    return excelSheet.Cells[rc.row, rc.column];
        //}
        #endregion

        #region check methods
        //public static bool CheckCorrectColumnName(string columnName, Worksheet excelSheet)
        //{
        //    return CheckCorrectRange(columnName + "1", excelSheet);
        //}

        //public static bool CheckCorrectColumnIndex(int columnIndex, Worksheet excelSheet)
        //{
        //    return CheckCorrectRC(1, columnIndex, excelSheet);
        //}

        //public static bool CheckCorrectRange(string range, Worksheet excelSheet)
        //{
        //    try
        //    {
        //        var rg = excelSheet.Range[range];
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public static bool CheckCorrectRC(int row, int column, Worksheet excelSheet)
        //{
        //    try
        //    {
        //        var rc = excelSheet.Cells[row, column];
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public static bool CheckCorrectColumnName(string columnName, Application excelInstance)
        //{
        //    return CheckCorrectRange(columnName + "1", excelInstance);
        //}

        //public static bool CheckCorrectRowIndex(int rowIndex, Application excelInstance)
        //{
        //    return CheckCorrectRC(rowIndex, 1, excelInstance);
        //}

        //public static bool CheckCorrectColumnIndex(int columnIndex, Application excelInstance)
        //{
        //    return CheckCorrectRC(1, columnIndex, excelInstance);
        //}

        //public static bool CheckCorrectRange(this string range, Application excelInstance)
        //{
        //    try
        //    {
        //        var rg = excelInstance.Range[range];
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public static bool CheckCorrectRC(int row, int column, Application excelInstance)
        //{
        //    try
        //    {
        //        var rc = excelInstance.Cells[row, column];
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public static bool CheckCorrectRCRange(int startRow, int startColumn, int endRow, int endColumn, Application excelInstance, bool throwExceptionWhenInvalidRange = true)
        //{
        //    if (!CheckCorrectRC(startRow, startColumn, excelInstance))
        //    {
        //        if (throwExceptionWhenInvalidRange)
        //        {
        //            throw new Exception("Invalid Start Location. Row: " + startRow + ", Column: " + startColumn);
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    if (!CheckCorrectRC(endRow, endColumn, excelInstance))
        //    {
        //        if (throwExceptionWhenInvalidRange)
        //        {
        //            throw new Exception("Invalid End Location. Row: " + endRow + ", Column: " + endColumn);
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //public static bool CheckCorrectRCRange(int startRow, int startColumn, int endRow, int endColumn, Worksheet excelSheet, bool throwExceptionWhenInvalidRange = true)
        //{
        //    if (!CheckCorrectRC(startRow, startColumn, excelSheet))
        //    {
        //        if (throwExceptionWhenInvalidRange)
        //        {
        //            throw new Exception("Invalid Start Location. Row: " + startRow + ", Column: " + startColumn);
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    if (!CheckCorrectRC(endRow, endColumn, excelSheet))
        //    {
        //        if (throwExceptionWhenInvalidRange)
        //        {
        //            throw new Exception("Invalid End Location. Row: " + endRow + ", Column: " + endColumn);
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
        #endregion
    }
}
