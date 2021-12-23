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
    [Attributes.ClassAttributes.SubGruop("Row Action")]
    [Attributes.ClassAttributes.Description("This command allows you to add a DataTable Row to a DataTable by a Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a DataTable Row to a DataTable by a Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class AddDataTableRowByDictionaryCommand : ScriptCommand
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
        [Attributes.PropertyAttributes.PropertyDescription("Please specify the Dictionary Variable Name to add to the DataTable")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary)]
        public string v_RowName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please specify the if Dictionary key does not exists")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Ignore** or **Error**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Ignore")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true, "Ignore")]
        public string v_NotExistsKey { get; set; }

        public AddDataTableRowByDictionaryCommand()
        {
            this.CommandName = "AddDataTableRowByDictionaryCommand";
            this.SelectionName = "Add DataTable Row By Dictionary";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

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
            //Dictionary<string, string> myDic;
            //if (!(rowVar.VariableValue is Dictionary<string, string>))
            //{
            //    throw new Exception(v_RowName + " is not supported Dictionary");
            //}
            //else
            //{
            //    myDic = (Dictionary<string, string>)rowVar.VariableValue;
            //}
            Dictionary<string, string> myDic = v_RowName.GetDictionaryVariable(engine);

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

            DataRow row = myDT.NewRow();
            foreach(var item in myDic)
            {
                if (columns.Contains(item.Key))
                {
                    row[item.Key] = item.Value;
                }
                else if (notExistsKey == "error")
                {
                    throw new Exception("Column name " + item.Key + " does not exists");
                }
            }
            myDT.Rows.Add(row);
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Add DataTable '" + v_DataTableName + "' Row By Dictionary '" + v_RowName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_DataTableName))
            {
                this.validationResult += "DataTable Name is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_RowName))
            {
                this.validationResult += "Dictionary Name is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}