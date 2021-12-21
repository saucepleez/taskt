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
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Column to List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Column to List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ConvertDataTableColumnToListCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the DataTable Variable Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a existing DataTable to fet rows from.")]
        [Attributes.PropertyAttributes.SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please specify Column type")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Column Name** or **Index**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Column Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Index")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true, "Column Name")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the Name or Index of the Column")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a valid Column index value")]
        [Attributes.PropertyAttributes.SampleUsage("**id** or **0** or **{{{vColumn}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        public string v_DataColumnIndex { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Specify the Variable Name To Assign The List")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        [Attributes.PropertyAttributes.PropertyParameterDirection(Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List)]
        public string v_OutputVariableName { get; set; }

        public ConvertDataTableColumnToListCommand()
        {
            this.CommandName = "ConvertDataTableColumnToListCommand";
            this.SelectionName = "Convert DataTable Column To List";
            this.CommandEnabled = true;
            this.CustomRendering = true;         
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            //DataTable srcDT = (DataTable)v_DataTableName.GetRawVariable(engine).VariableValue;
            DataTable srcDT = v_DataTableName.GetDataTableVariable(engine);

            //if (String.IsNullOrEmpty(v_ColumnType))
            //{
            //    v_ColumnType = "Column Name";
            //}
            string colType = v_ColumnType.GetUISelectionValue("v_ColumnType", this, engine);

            List<string> myList = new List<string>();
            switch (colType)
            {
                case "column name":
                    var colName = v_DataColumnIndex.ConvertToUserVariable(engine);
                    for (int i = 0; i < srcDT.Rows.Count; i++)
                    {
                        myList.Add((srcDT.Rows[i][colName] != null) ? srcDT.Rows[i][colName].ToString() : "");
                    }
                    break;

                case "index":
                    int colIdx = int.Parse(v_DataColumnIndex.ConvertToUserVariable(engine));
                    for (int i = 0; i < srcDT.Rows.Count; i++)
                    {
                        myList.Add((srcDT.Rows[i][colIdx] != null) ? srcDT.Rows[i][colIdx].ToString() : "");
                    }
                    break;
            }

            myList.StoreInUserVariable(engine, v_OutputVariableName);
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
            return base.GetDisplayValue() + " [Convert DataTable '" + v_DataTableName + "' Column '" + v_DataColumnIndex + "' to List '" + v_OutputVariableName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);
            if (String.IsNullOrEmpty(this.v_DataTableName))
            {
                this.validationResult += "DataTable is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_DataColumnIndex))
            {
                this.validationResult += "Column Name or Index is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_OutputVariableName))
            {
                this.validationResult += "Result List is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}