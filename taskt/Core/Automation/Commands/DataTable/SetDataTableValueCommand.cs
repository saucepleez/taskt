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
    [Attributes.ClassAttributes.SubGruop("DataTable Action")]
    [Attributes.ClassAttributes.Description("This command allows you to set the DataTable value")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set the DataTable value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class SetDataTableValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the DataTable Variable Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a existing DataTable.")]
        [SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the Column value type")]
        [InputSpecification("")]
        [SampleUsage("**Index** or **Column Name**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Index")]
        [PropertyUISelectionOption("Column Name")]
        [PropertyIsOptional(true, "Column Name")]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the Column Name or Index")]
        [InputSpecification("")]
        [SampleUsage("**0** or **id** or **{{{vColumn}}}** or **-1**")]
        [Remarks("If **-1** is specified for Column Index, it means the last column.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the Row Index")]
        [InputSpecification("")]
        [SampleUsage("**0** or **1** or **-1** or **{{{vIndex}}}**")]
        [Remarks("**-1** means index of the last row.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "Current Row")]
        public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify the Value to set DataTable")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**value** or **123** or **{{{vValue}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_NewValue { get; set; }

        public SetDataTableValueCommand()
        {
            this.CommandName = "SetDataTableValueCommand";
            this.SelectionName = "Set DataTable Value";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            DataTable myDT = v_DataTableName.GetDataTableVariable(engine);
            //string columnType = "Column Name";
            //if (!String.IsNullOrEmpty(v_ColumnType))
            //{
            //    columnType = v_ColumnType.ConvertToUserVariable(engine);
            //}
            //columnType = columnType.ToLower();
            //switch (columnType)
            //{
            //    case "column name":
            //    case "index":
            //        break;
            //    default:
            //        throw new Exception("Strange column type " + v_ColumnType);
            //        break;
            //}
            string columnType = v_ColumnType.GetUISelectionValue("v_ColumnType", this, engine);

            //string columnPosition = v_ColumnIndex.ConvertToUserVariable(engine);

            //string vRow = v_RowIndex.ConvertToUserVariable(engine);
            //int rowIndex = int.Parse(vRow);
            //if ((rowIndex < 0) || (rowIndex >= myDT.Rows.Count))
            //{
            //    throw new Exception("Row Index is less than 0 or exceeds the number of rows in the DataTable");
            //}
            int rowIndex = DataTableControls.GetRowIndex(v_DataTableName, v_RowIndex, engine);

            string newValue = v_NewValue.ConvertToUserVariable(engine);
            
            if (columnType == "column name")
            {
                string columnName = DataTableControls.GetColumnName(myDT, v_ColumnIndex, engine);
                myDT.Rows[rowIndex][columnName] = newValue;
                
            }
            else
            {
                int colIndex = DataTableControls.GetColumnIndex(myDT, v_ColumnIndex, engine);
                myDT.Rows[rowIndex][colIndex] = newValue;
            }
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
            return base.GetDisplayValue() + " [DataTable '" + v_DataTableName + "' Column '" + v_ColumnIndex+ "' Row '" + v_RowIndex + "', Set '" + v_NewValue + "']";
        }

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_DataTableName))
        //    {
        //        this.validationResult += "DataTable Name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_ColumnIndex))
        //    {
        //        this.validationResult += "Column Name or Index is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_RowIndex))
        //    {
        //        this.validationResult += "Row Index is empty.\n";
        //        this.IsValid = false;
        //    }
        //    else
        //    {
        //        int index;
        //        if (int.TryParse(v_RowIndex, out index))
        //        {
        //            if (index < 0)
        //            {
        //                this.validationResult += "Row Index is less than 0.\n";
        //                this.IsValid = false;
        //            }
        //        }
        //    }
        //    if (String.IsNullOrEmpty(this.v_NewValue))
        //    {
        //        this.validationResult += "Value to set is emtpy.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}