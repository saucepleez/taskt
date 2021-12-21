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
    [Attributes.ClassAttributes.Description("This command allows you to set a column to a DataTable by a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a column to a DataTable by a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class SetDataTableColumnValuesByDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the DataTable Variable Name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a existing DataTable to add rows to.")]
        [Attributes.PropertyAttributes.SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
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
        [Attributes.PropertyAttributes.PropertyDescription("Please specify the Column Name to set")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**0** or **newColumn** or **{{{vNewColumn}}}** or **{{{vIndex}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        public string v_SetColumnName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please specify the DataTable to set new Column values")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**vDataTable** or **{{{vDataTable}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable)]
        public string v_SetDataTableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("If the number of rows is less than the DataTable to set")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Ignore** or **Add Rows** or **Error**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Ignore")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Add Rows")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true, "Ignore")]
        public string v_IfRowNotEnough { set; get; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("If the number of DataTable items is less than the rows to setted DataTable")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Ignore** or **Error**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Ignore")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true, "Ignore")]
        public string v_IfSetDataTableNotEnough { set; get; }

        public SetDataTableColumnValuesByDataTableCommand()
        {
            this.CommandName = "SetDataTableColumnByDataTableCommand";
            this.SelectionName = "Set DataTable Column By DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            DataTable myDT = v_DataTableName.GetDataTableVariable(engine);

            DataTable setDT = v_SetDataTableName.GetDataTableVariable(engine);

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
            if (myDT.Rows.Count < setDT.Rows.Count)
            {
                switch (ifRowNotEnough)
                {
                    case "ignore":
                    case "add rows":
                        break;
                    case "error":
                        throw new Exception("The number of rows is less than the DataTable to set");
                        break;
                }
            }

            //string ifListNotEnough = "Ignore";
            //if (!String.IsNullOrEmpty(v_IfSetDataTableNotEnough))
            //{
            //    ifListNotEnough = v_IfSetDataTableNotEnough.ConvertToUserVariable(engine);
            //}
            //ifListNotEnough = ifListNotEnough.ToLower();
            //switch (ifListNotEnough)
            //{
            //    case "ignore":
            //    case "error":
            //        break;
            //    default:
            //        throw new Exception("Strange value If the number of List items is less than the rows to setted " + v_IfSetDataTableNotEnough);
            //        break;
            //}
            string ifListNotEnough = v_IfSetDataTableNotEnough.GetUISelectionValue("v_IfSetDataTableNotEnough", this, engine);

            if ((myDT.Rows.Count > setDT.Rows.Count) && (ifListNotEnough == "error"))
            {
                throw new Exception("The number of DataTable items is less than the rows to settedd");
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
            string trgColName = v_SetColumnName.ConvertToUserVariable(engine);
            bool isExistsCol = false;
            if (colType == "column name")
            {
                for (int i = 0; i < setDT.Columns.Count; i++)
                {
                    if (trgColName == setDT.Columns[i].ColumnName)
                    {
                        isExistsCol = true;
                    }
                }
            }
            else
            {
                int colIndex = int.Parse(trgColName);
                if ((colIndex >= 0) && (colIndex < setDT.Columns.Count))
                {
                    isExistsCol = true;
                    trgColName = setDT.Columns[colIndex].ColumnName;
                }
            }
            if (!isExistsCol)
            {
                throw new Exception("Column " + v_SetColumnName + " does not exists in DataTable to set");
            }
            isExistsCol = false;
            for (int i = 0; i < myDT.Columns.Count; i++)
            {
                if (trgColName == myDT.Columns[i].ColumnName)
                {
                    isExistsCol = true;
                    break;
                }
            }
            if (!isExistsCol)
            {
                throw new Exception("Column " + v_SetColumnName + " does not exists in DataTable to setted");
            }

            int maxRow = (myDT.Rows.Count > setDT.Rows.Count) ? setDT.Rows.Count : myDT.Rows.Count;
            for (int i = 0; i < maxRow; i++)
            {
                myDT.Rows[i][trgColName] = setDT.Rows[i][trgColName];
            }
            if ((myDT.Rows.Count < setDT.Rows.Count) && (ifRowNotEnough == "add rows"))
            {
                for (int i = myDT.Rows.Count; i < setDT.Rows.Count; i++)
                {
                    myDT.Rows.Add();
                    myDT.Rows[i][trgColName] = setDT.Rows[i][trgColName];
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
            return base.GetDisplayValue() + " [Set DataTable '" + v_DataTableName + "' Column Name '" + v_SetColumnName + "' DataTable '" + v_SetDataTableName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_DataTableName))
            {
                this.validationResult += "DataTable Name to setted is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_SetDataTableName))
            {
                this.validationResult += "DataTable Name to set is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_SetColumnName))
            {
                this.validationResult += "Column Name is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}