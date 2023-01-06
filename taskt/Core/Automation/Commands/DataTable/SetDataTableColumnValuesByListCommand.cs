using System;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Column Action")]
    [Attributes.ClassAttributes.Description("This command allows you to set a column to a DataTable by a List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a column to a DataTable by a List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetDataTableColumnValuesByListCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please indicate the DataTable Variable Name")]
        //[InputSpecification("Enter a existing DataTable to add rows to.")]
        //[SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "DataTable")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify Column type")]
        //[InputSpecification("")]
        //[SampleUsage("**Column Name** or **Index**")]
        //[Remarks("")]
        //[PropertyUISelectionOption("Column Name")]
        //[PropertyUISelectionOption("Index")]
        //[PropertyIsOptional(true, "Column Name")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyDisplayText(true, "Column Type")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnType))]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify the Column Name to set")]
        //[InputSpecification("")]
        //[SampleUsage("**0** or **newColumn** or **{{{vNewColumn}}}** or **-1**")]
        //[Remarks("If **-1** is specified for Column Index, it means the last column.")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Column")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnNameIndex))]
        public string v_SetColumnName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify the List to set new Column values")]
        //[InputSpecification("")]
        //[SampleUsage("**vList** or **{{{vList}}}**")]
        //[Remarks("")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        //[PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "List")]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        public string v_SetListName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("If the number of rows is less than the List")]
        //[InputSpecification("")]
        //[SampleUsage("**Ignore** or **Add Rows** or **Error**")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyUISelectionOption("Ignore")]
        //[PropertyUISelectionOption("Add Rows")]
        //[PropertyUISelectionOption("Error")]
        //[PropertyIsOptional(true, "Ignore")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_WhenLessRows))]
        [PropertyDescription("When there are Less Rows than List to set")]
        public string v_IfRowNotEnough { set; get; }

        [XmlAttribute]
        //[PropertyDescription("If the number of List items is less than the rows")]
        //[InputSpecification("")]
        //[SampleUsage("**Ignore** or **Error**")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyUISelectionOption("Ignore")]
        //[PropertyUISelectionOption("Error")]
        //[PropertyIsOptional(true, "Ignore")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_WhenGreaterRows))]
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

            (var myDT, var colIndex) = this.GetDataTableVariableAndColumnIndex(nameof(v_DataTableName), nameof(v_ColumnType), nameof(v_SetColumnName), engine);
            string trgColumnName = myDT.Columns[colIndex].ColumnName;

            List<string> myList = v_SetListName.GetListVariable(engine);

            string ifRowNotEnough = this.GetUISelectionValue(nameof(v_IfRowNotEnough), "Row Not Enough", engine);
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
                }
            }

            string ifListNotEnough = this.GetUISelectionValue(nameof(v_IfListNotEnough), "List Not Enough", engine);
            if ((myDT.Rows.Count > myList.Count) && (ifListNotEnough == "error"))
            {
                throw new Exception("The number of List items is less than the rows");
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
    }
}