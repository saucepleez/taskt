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
    [Attributes.ClassAttributes.SubGruop("Column Action")]
    [Attributes.ClassAttributes.Description("This command allows you to delete a column to a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to delete a column to a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class DeleteDataTableColumnCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the DataTable Variable Name")]
        [InputSpecification("Enter a existing DataTable to add rows to.")]
        [SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the Column type (Default is Column Name)")]
        [InputSpecification("")]
        [SampleUsage("**Column Name** or **Index**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Column Name")]
        [PropertyUISelectionOption("Index")]
        [PropertyIsOptional(true, "Column Name")]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the Column Name to delete")]
        [InputSpecification("")]
        [SampleUsage("**0** or **newColumn** or **{{{vColumn}}}** or **-1**")]
        [Remarks("If **-1** is specified for Column Index, it means the last column.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_DeleteColumnName { get; set; }

        public DeleteDataTableColumnCommand()
        {
            this.CommandName = "DeleteDataTableColumnCommand";
            this.SelectionName = "Delete DataTable Column";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            DataTable myDT = v_DataTableName.GetDataTableVariable(engine);

            //string colType = "Column Name";
            //if (!String.IsNullOrEmpty(v_ColumnType))
            //{
            //    colType = v_ColumnType.ConvertToUserVariable(engine);
            //}
            //colType = colType.ToLower();
            string colType = v_ColumnType.GetUISelectionValue("v_ColumnType", this, engine);

            //switch (colType)
            //{
            //    case "column name":
            //    case "index":
            //        break;
            //    default:
            //        throw new Exception("Strange column type " + v_ColumnType);
            //        break;
            //}

            if (colType == "column name")
            {
                //string trgColumn = v_DeleteColumnName.ConvertToUserVariable(engine);

                //for (int i = 0; i < myDT.Columns.Count; i++)
                //{
                //    if (myDT.Columns[i].ColumnName == trgColumn)
                //    {
                //        myDT.Columns.Remove(trgColumn);
                //        return;
                //    }
                //}
                //throw new Exception("Column " + v_DeleteColumnName + " does not exists");
                string trgColumn = DataTableControls.GetColumnName(myDT, v_DeleteColumnName, engine);
                myDT.Columns.Remove(trgColumn);
            }
            else
            {
                //string tCol = v_DeleteColumnName.ConvertToUserVariable(engine);
                //int colIndex = int.Parse(tCol);
                //if ((colIndex >= 0) && (colIndex < myDT.Columns.Count))
                //{
                //    myDT.Columns.RemoveAt(colIndex);
                //}
                //else
                //{
                //    throw new Exception("Column index " + v_DeleteColumnName + " does not exists");
                //}
                int colIndex = DataTableControls.GetColumnIndex(myDT, v_DeleteColumnName, engine);
                myDT.Columns.RemoveAt(colIndex);
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
            return base.GetDisplayValue() + " [Delete DataTable '" + v_DataTableName + "' Column '" + v_DeleteColumnName + "']";
        }

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_DataTableName))
        //    {
        //        this.validationResult += "DataTable Name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_DeleteColumnName))
        //    {
        //        this.validationResult += "Column Name is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}