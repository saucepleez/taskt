using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to get the DataTable value")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get the DataTable value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class GetDataTableValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the DataTable Variable Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a existing DataTable.")]
        [Attributes.PropertyAttributes.SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please specify the Column value type")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Index** or **Column Name**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Index")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Column Name")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true, "Column Name")]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please specify the Column Name or Index")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**0** or **id** or **{{{vIndex}}}** or **{{{vColumn}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please specify the Row Index")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**0** or **{{{vIndex}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_RowIndex { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Specify the Variable Name To Assign the Value")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        public string v_UserVariableName { get; set; }

        public GetDataTableValueCommand()
        {
            this.CommandName = "GetDataTableValueCommand";
            this.SelectionName = "Get DataTable Value";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

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

            string columnPosition = v_ColumnIndex.ConvertToUserVariable(engine);

            string vRow = v_RowIndex.ConvertToUserVariable(engine);
            int rowIndex = int.Parse(vRow);
            if ((rowIndex < 0) || (rowIndex >= myDT.Rows.Count))
            {
                throw new Exception("Row Index is less than 0 or exceeds the number of rows in the DataTable");
            }

            string v;
            if (columnType == "column name")
            {
                v = (myDT.Rows[rowIndex][columnPosition] == null) ? "" : myDT.Rows[rowIndex][columnPosition].ToString();
                
            }
            else
            {
                int colIndex = int.Parse(columnPosition);
                v = (myDT.Rows[rowIndex][colIndex] == null) ? "" : myDT.Rows[rowIndex][colIndex].ToString();
            }
            v.StoreInUserVariable(engine, v_UserVariableName);
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
            return base.GetDisplayValue() + " [DataTable '" + v_DataTableName + "' Column '" + v_ColumnIndex+ "' Row '" + v_RowIndex + "', Store In: '" + v_UserVariableName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_DataTableName))
            {
                this.validationResult += "DataTable Name is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_ColumnIndex))
            {
                this.validationResult += "Column Name or Index is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_RowIndex))
            {
                this.validationResult += "Row Index is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_UserVariableName))
            {
                this.validationResult += "Result Value variable is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}