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
    [Attributes.ClassAttributes.Description("This command allows you to add a DataTable Row to a DataTable by a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a DataTable Row to a DataTable by a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class AddDataTableRowByDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the DataTable Variable Name to be added a row")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a existing DataTable Variable Name")]
        [Attributes.PropertyAttributes.SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please specify the DataTable Variable Name to add to the DataTable")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable)]
        public string v_RowName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please specify the if DataTable column does not exists (Default is Ignore)")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Ignore** or **Error**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Ignore")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        public string v_NotExistsKey { get; set; }

        public AddDataTableRowByDataTableCommand()
        {
            this.CommandName = "AddDataTableRowByDataTableCommand";
            this.SelectionName = "Add DataTable Row By DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            Script.ScriptVariable dtVar = v_DataTableName.GetRawVariable(engine);
            DataTable myDT;
            if (!(dtVar.VariableValue is DataTable))
            {
                throw new Exception(v_DataTableName + " is not DataTable");
            }
            else
            {
                myDT = (DataTable)dtVar.VariableValue;
            }

            Script.ScriptVariable rowVar = v_RowName.GetRawVariable(engine);
            DataTable addDT;
            if (!(rowVar.VariableValue is DataTable))
            {
                throw new Exception(v_RowName + " is not DataTable");
            }
            else
            {
                addDT = (DataTable)rowVar.VariableValue;
            }

            string notExistsKey;
            if (String.IsNullOrEmpty(v_NotExistsKey))
            {
                notExistsKey = "Ignore";
            }
            else
            {
                notExistsKey = v_NotExistsKey.ConvertToUserVariable(engine);
            }
            notExistsKey = notExistsKey.ToLower();
            switch (notExistsKey)
            {
                case "ignore":
                case "error":
                    break;
                default:
                    throw new Exception("Strange value in if Dictionary key does not exists " + v_NotExistsKey);
            }

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
    }
}