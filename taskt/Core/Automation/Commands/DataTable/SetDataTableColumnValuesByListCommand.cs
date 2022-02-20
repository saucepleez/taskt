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
    [Attributes.ClassAttributes.Description("This command allows you to set a column to a DataTable by a List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a column to a DataTable by a List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class SetDataTableColumnValuesByListCommand : ScriptCommand
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
        [PropertyDescription("Please specify the Column Name to set")]
        [InputSpecification("")]
        [SampleUsage("**0** or **newColumn** or **{{{vNewColumn}}}** or **-1**")]
        [Remarks("If **-1** is specified for Column Index, it means the last column.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_SetColumnName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the List to set new Column values")]
        [InputSpecification("")]
        [SampleUsage("**vList** or **{{{vList}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_SetListName { get; set; }

        [XmlAttribute]
        [PropertyDescription("If the number of rows is less than the List")]
        [InputSpecification("")]
        [SampleUsage("**Ignore** or **Add Rows** or **Error**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Add Rows")]
        [PropertyUISelectionOption("Error")]
        [PropertyIsOptional(true, "Ignore")]
        public string v_IfRowNotEnough { set; get; }

        [XmlAttribute]
        [PropertyDescription("If the number of List items is less than the rows")]
        [InputSpecification("")]
        [SampleUsage("**Ignore** or **Error**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyIsOptional(true, "Ignore")]
        public string v_IfListNotEnough { set; get; }

        public SetDataTableColumnValuesByListCommand()
        {
            this.CommandName = "SetDataTableColumnByListCommand";
            this.SelectionName = "Set DataTable Column By List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            DataTable myDT = v_DataTableName.GetDataTableVariable(engine);

            List<string> myList = v_SetListName.GetListVariable(engine);

            //string ifRowNotEnough = "Ignore";
            //if (!String.IsNullOrEmpty(v_IfRowNotEnough))
            //{
            //    ifRowNotEnough = v_IfRowNotEnough.ConvertToUserVariable(engine);
            //}
            //ifRowNotEnough = ifRowNotEnough.ToLower();
            //switch (ifRowNotEnough)
            //{
            //    case "ignore":
            //    case "add rows":
            //    case "error":
            //        break;
            //    default:
            //        throw new Exception("Strange value If the number of rows is less than the List " + v_IfRowNotEnough);
            //        break;
            //}
            string ifRowNotEnough = v_IfRowNotEnough.GetUISelectionValue("v_IfRowNotEnough", this, engine);

            // rows check
            if (myDT.Rows.Count < myList.Count)
            {
                switch (ifRowNotEnough)
                {
                    case "ignore":
                    case "add rows":
                        break;
                    case "error":
                        throw new Exception("The number of rows is less than the List");
                        break;
                }
            }

            //string ifListNotEnough = "Ignore";
            //if (!String.IsNullOrEmpty(v_IfListNotEnough))
            //{
            //    ifListNotEnough = v_IfListNotEnough.ConvertToUserVariable(engine);
            //}
            //ifListNotEnough = ifListNotEnough.ToLower();
            //switch (ifListNotEnough)
            //{
            //    case "ignore":
            //    case "error":
            //        break;
            //    default:
            //        throw new Exception("Strange value If the number of List items is less than the rows " + v_IfListNotEnough);
            //        break;
            //}
            string ifListNotEnough = v_IfListNotEnough.GetUISelectionValue("v_IfListNotEnough", this, engine);

            if ((myDT.Rows.Count > myList.Count) && (ifListNotEnough == "error"))
            {
                throw new Exception("The number of List items is less than the rows");
            }

            //string colType = "Column Name";
            //if (!String.IsNullOrEmpty(v_ColumnType))
            //{
            //    colType = v_ColumnType.ConvertToUserVariable(engine);
            //}
            //colType = colType.ToLower();
            //switch (colType)
            //{
            //    case "column name":
            //    case "index":
            //        break;
            //    default:
            //        throw new Exception("Strange column type " + v_ColumnType);
            //        break;
            //}
            string colType = v_ColumnType.GetUISelectionValue("v_ColumnType", this, engine);

            // column name check
            //string trgColName = v_SetColumnName.ConvertToUserVariable(engine);
            //bool isExistsCol = false;
            //if (colType == "column name")
            //{
            //    for (int i = 0; i < myDT.Columns.Count; i++)
            //    {
            //        if (trgColName == myDT.Columns[i].ColumnName)
            //        {
            //            isExistsCol = true;
            //        }
            //    }
            //}
            //else
            //{
            //    int colIndex = int.Parse(trgColName);
            //    if ((colIndex >= 0) && (colIndex < myDT.Columns.Count))
            //    {
            //        isExistsCol = true;
            //        trgColName = myDT.Columns[colIndex].ColumnName;
            //    }
            //}
            //if (!isExistsCol)
            //{
            //    throw new Exception("Column " + v_SetColumnName + " does not exists");
            //}
            string trgColumnName;
            if (colType == "column name")
            {
                trgColumnName = DataTableControl.GetColumnName(myDT, v_SetColumnName, engine);
            }
            else
            {
                int colIndex = DataTableControl.GetColumnIndex(myDT, v_SetColumnName, engine);
                trgColumnName = myDT.Columns[colIndex].ColumnName;
            }

            int maxRow = (myDT.Rows.Count > myList.Count) ? myList.Count : myDT.Rows.Count;
            for (int i = 0; i < maxRow; i++)
            {
                myDT.Rows[i][trgColumnName] = myList[i];
            }
            if ((myDT.Rows.Count < myList.Count) && (ifRowNotEnough == "add rows"))
            {
                for (int i = myDT.Rows.Count; i < myList.Count; i++)
                {
                    myDT.Rows.Add();
                    myDT.Rows[i][trgColumnName] = myList[i];
                }
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
            return base.GetDisplayValue() + " [Set DataTable '" + v_DataTableName + "' Column Name '" + v_SetColumnName + "' List '" + v_SetListName + "']";
        }

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_DataTableName))
        //    {
        //        this.validationResult += "DataTable Name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_SetListName))
        //    {
        //        this.validationResult += "List Name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_SetColumnName))
        //    {
        //        this.validationResult += "Column Name is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}