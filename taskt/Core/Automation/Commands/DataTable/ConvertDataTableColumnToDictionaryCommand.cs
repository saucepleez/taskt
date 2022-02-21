using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert Column")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Column to Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Column to Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ConvertDataTableColumnToDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the DataTable Variable Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a existing DataTable to fet rows from.")]
        [SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Column type")]
        [InputSpecification("")]
        [SampleUsage("**Column Name** or **Index**")]
        [Remarks("")]
        [PropertyUISelectionOption("Column Name")]
        [PropertyUISelectionOption("Index")]
        [PropertyIsOptional(true, "Column Name")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please enter the Name or Index of the Column")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a valid Column index value")]
        [SampleUsage("**id** or **0** or **{{{vColumn}}}** or **-1**")]
        [Remarks("If **-1** is specified for Column Index, it means the last column.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_DataColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please enter the Dictionary Key prefix")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter Dictionary Key Prefix")]
        [SampleUsage("**row** or **{{{vPrefix}}}**")]
        [Remarks("If enter **row**, Dictionary key is row0, row1, ...")]
        [PropertyIsOptional(true, "row")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        public string v_KeyPrefix { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify the Variable Name To Assign The Dictionary")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_OutputVariableName { get; set; }

        public ConvertDataTableColumnToDictionaryCommand()
        {
            this.CommandName = "ConvertDataTableColumnToDictionaryCommand";
            this.SelectionName = "Convert DataTable Column To Dictionary";
            this.CommandEnabled = true;
            this.CustomRendering = true;         
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //DataTable srcDT = (DataTable)v_DataTableName.GetRawVariable(engine).VariableValue;
            DataTable srcDT = v_DataTableName.GetDataTableVariable(engine);

            //if (String.IsNullOrEmpty(v_ColumnType))
            //{
            //    v_ColumnType = "Column Name";
            //}
            string colType = v_ColumnType.GetUISelectionValue("v_ColumnType", this, engine);

            string prefix;
            if (String.IsNullOrEmpty(v_KeyPrefix))
            {
                prefix = "row";
            }
            else
            {
                prefix = v_KeyPrefix.ConvertToUserVariable(engine);
            }

            Dictionary<string, string> myDic = new Dictionary<string, string>();
            switch (colType)
            {
                case "column name":
                    //var colName = v_DataColumnIndex.ConvertToUserVariable(engine);
                    string colName = DataTableControls.GetColumnName(srcDT, v_DataColumnIndex, engine);
                    for (int i = 0; i < srcDT.Rows.Count; i++)
                    {
                        myDic.Add(prefix + i.ToString(), (srcDT.Rows[i][colName] != null) ? srcDT.Rows[i][colName].ToString() : "");
                    }
                    break;

                case "index":
                    //int colIdx = int.Parse(v_DataColumnIndex.ConvertToUserVariable(engine));
                    int colIndex = DataTableControls.GetColumnIndex(srcDT, v_DataColumnIndex, engine);
                    for (int i = 0; i < srcDT.Rows.Count; i++)
                    {
                        myDic.Add(prefix + i.ToString(), (srcDT.Rows[i][colIndex] != null) ? srcDT.Rows[i][colIndex].ToString() : "");
                    }
                    break;
            }

            myDic.StoreInUserVariable(engine, v_OutputVariableName);
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            return RenderedControls;
        }
        
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Convert DataTable '" + v_DataTableName + "' Column '" + v_DataColumnIndex + "' to Dictionary '" + v_OutputVariableName + "']";
        }

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);
        //    if (String.IsNullOrEmpty(this.v_DataTableName))
        //    {
        //        this.validationResult += "DataTable is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_DataColumnIndex))
        //    {
        //        this.validationResult += "Column Name or Index is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_OutputVariableName))
        //    {
        //        this.validationResult += "Result Dictionary is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}