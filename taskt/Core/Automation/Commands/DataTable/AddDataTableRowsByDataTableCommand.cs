using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Row Action")]
    [Attributes.ClassAttributes.Description("This command allows you to add a DataTable Row to a DataTable by a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a DataTable Row to a DataTable by a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class AddDataTableRowsByDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the DataTable Variable Name to be added a row")]
        [InputSpecification("Enter a existing DataTable Variable Name")]
        [SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("DataTable to be added", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable to be added")]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the DataTable Variable Name to add to the DataTable")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyValidationRule("DataTable to add", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable to add")]
        public string v_RowName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the if DataTable column does not exists")]
        [InputSpecification("")]
        [SampleUsage("**Ignore** or **Error**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyIsOptional(true, "Ignore")]
        public string v_NotExistsKey { get; set; }

        public AddDataTableRowsByDataTableCommand()
        {
            this.CommandName = "AddDataTableRowByDataTableCommand";
            this.SelectionName = "Add DataTable Row By DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            DataTable myDT = v_DataTableName.GetDataTableVariable(engine);

            DataTable addDT = v_RowName.GetDataTableVariable(engine);

            //string notExistsKey = v_NotExistsKey.GetUISelectionValue("v_NotExistsKey", this, engine);
            string notExistsKey = this.GetUISelectionValue(nameof(v_NotExistsKey), "Key Does Not Exists", engine);

            // get columns list
            List<string> columns = myDT.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            if (notExistsKey == "error")
            {
                for (int i = 0; i < addDT.Columns.Count; i++)
                {
                    if (!columns.Contains(addDT.Columns[i].ColumnName))
                    {
                        throw new Exception("Column name " + addDT.Columns[i].ColumnName + " does not exists");
                    }
                }
            }
            for (int i = 0; i < addDT.Rows.Count; i++)
            {
                DataRow row = myDT.NewRow();
                for (int j = 0; j < addDT.Columns.Count; j++)
                {
                    if (columns.Contains(addDT.Columns[j].ColumnName))
                    {
                        row[addDT.Columns[j].ColumnName] = addDT.Rows[i][addDT.Columns[j].ColumnName];
                    }
                }
                myDT.Rows.Add(row);
            }
        }
    }
}