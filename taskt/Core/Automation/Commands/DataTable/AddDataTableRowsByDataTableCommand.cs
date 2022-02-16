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
    [Attributes.ClassAttributes.SubGruop("Row Action")]
    [Attributes.ClassAttributes.Description("This command allows you to add a DataTable Row to a DataTable by a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a DataTable Row to a DataTable by a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
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

            //Script.ScriptVariable dtVar = v_DataTableName.GetRawVariable(engine);
            //DataTable myDT;
            //if (!(dtVar.VariableValue is DataTable))
            //{
            //    throw new Exception(v_DataTableName + " is not DataTable");
            //}
            //else
            //{
            //    myDT = (DataTable)dtVar.VariableValue;
            //}
            DataTable myDT = v_DataTableName.GetDataTableVariable(engine);

            //Script.ScriptVariable rowVar = v_RowName.GetRawVariable(engine);
            //DataTable addDT;
            //if (!(rowVar.VariableValue is DataTable))
            //{
            //    throw new Exception(v_RowName + " is not DataTable");
            //}
            //else
            //{
            //    addDT = (DataTable)rowVar.VariableValue;
            //}
            DataTable addDT = v_RowName.GetDataTableVariable(engine);

            //string notExistsKey;
            //if (String.IsNullOrEmpty(v_NotExistsKey))
            //{
            //    notExistsKey = "Ignore";
            //}
            //else
            //{
            //    notExistsKey = v_NotExistsKey.ConvertToUserVariable(engine);
            //}
            //notExistsKey = notExistsKey.ToLower();
            //switch (notExistsKey)
            //{
            //    case "ignore":
            //    case "error":
            //        break;
            //    default:
            //        throw new Exception("Strange value in if Dictionary key does not exists " + v_NotExistsKey);
            //}
            string notExistsKey = v_NotExistsKey.GetUISelectionValue("v_NotExistsKey", this, engine);

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

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Add DataTable '" + v_DataTableName + "' Row By DataTable '" + v_RowName + "']";
        }

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_DataTableName))
        //    {
        //        this.validationResult += "Added DataTable Name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_RowName))
        //    {
        //        this.validationResult += "Add DataTable Name is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}