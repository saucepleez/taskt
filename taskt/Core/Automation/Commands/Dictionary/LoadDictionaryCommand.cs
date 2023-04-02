using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Action")]
    [Attributes.ClassAttributes.Description("This command Reads a Config file and stores it into a Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to load a config file.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop and OLEDB to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class LoadDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Enter the Dictionary Variable Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a name for a Dictionary.")]
        [SampleUsage("**myDictionary** or **{{{vDictionary}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Dictionary")]
        public string v_DictionaryName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the Workbook File Path")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("Enter or Select the path to the applicable file that should be loaded into the Dictionary.")]
        [SampleUsage("**C:\\temp\\myfile.xlsx** or **{{{vFilePath}}}**")]
        [Remarks("If file does not contain extension, supplement extensions supported by Excel.\nIf file does not contain folder path, file will be opened in the same folder as script file.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("File Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "File Path")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the Sheet Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the sheet name of the workbook to be read.")]
        [SampleUsage("**Sheet1** or **{{{vSheet}}}**")]
        [Remarks("Sheet has one table")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Sheet Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Sheet Name")]
        public string v_SheetName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the Key Column")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the key column name to create a Dictionary off of.")]
        [SampleUsage("**Key** or **{{{vKeyColumn}}}**")]
        [Remarks("This value is NOT Column Index Value like A, B. Please specify table column name.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Key Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Key Column")]
        public string v_KeyColumn { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the Value Column")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a value column name to create a Dictionary off of.")]
        [SampleUsage("**Value** or **{{{vValueColumn}}}**")]
        [Remarks("This value is NOT Column Index Value like A, B. Please specify table column name.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Value Colmun", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Value Column")]
        public string v_ValueColumn { get; set; }

        public LoadDictionaryCommand()
        {
            this.CommandName = "LoadDictionaryCommand";
            this.SelectionName = "Load Dictionary";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            //var vInstance = DateTime.Now.ToString();

            var vSheet = v_SheetName.ConvertToUserVariable(sender);
            var vKeyColumn = v_KeyColumn.ConvertToUserVariable(sender);
            var vValueColumn = v_ValueColumn.ConvertToUserVariable(sender);

            //string vFilePath = FilePathControls.FormatFilePath_NoFileCounter(v_FilePath, engine, new List<string>() { "xlsx", "xlsm", "xls" }, true);
            var vFilePath = v_FilePath.ConvertToUserVariable(sender);

            //Query required from workbook using OLEDB
            //DataTableControls dataSetCommand = new DataTableControls();

            string param = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""" + vFilePath + @""";Extended Properties=""Excel 12.0;HDR=YES;IMEX=1""";
            string queue = "Select " + vKeyColumn + "," + vValueColumn + " From " + "[" + vSheet + "$]";
            
            // DBG
            //MessageBox.Show(param + "\n" + queue);

            DataTable requiredData = DataTableControls.CreateDataTable(param, queue);
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

            outputDictionary.StoreInUserVariable(engine, v_DictionaryName);
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