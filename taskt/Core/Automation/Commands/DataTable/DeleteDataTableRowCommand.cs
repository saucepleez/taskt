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
    [Attributes.ClassAttributes.Description("This command allows you to delete a DataTable Row")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to delete a DataTable Row.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class DeleteDataTableRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the DataTable Variable Name to be delete a row")]
        [InputSpecification("Enter a existing DataTable Variable Name")]
        [SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the Row Index to delete")]
        [InputSpecification("")]
        [SampleUsage("**0** or **1** or **-1** or **{{{vRow}}}**")]
        [Remarks("**-1** means index of the last row.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "Current Row")]
        public string v_RowIndex { get; set; }

        public DeleteDataTableRowCommand()
        {
            this.CommandName = "DeleteDataTableRowCommand";
            this.SelectionName = "Delete DataTable Row";
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

            //var vIndex = v_RowIndex.ConvertToUserVariable(engine);
            //int index = int.Parse(vIndex);

            //if ((index < 0) || (index >= myDT.Rows.Count))
            //{
            //    throw new Exception("Row index " + v_RowIndex + " does not exists");
            //}
            int index = DataTableControl.GetRowIndex(v_DataTableName, v_RowIndex, engine);

            myDT.Rows[index].Delete();
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Delete DataTable '" + v_DataTableName + "' Row Index '" + v_RowIndex + "']";
        }

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_DataTableName))
        //    {
        //        this.validationResult += "DataTable Name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_RowIndex))
        //    {
        //        this.validationResult += "Row index is empty.\n";
        //        this.IsValid = false;
        //    }
        //    else
        //    {
        //        int index;
        //        if (int.TryParse(this.v_RowIndex, out index))
        //        {
        //            if (index < 0)
        //            {
        //                this.validationResult += "Row index is less than 0.\n";
        //                this.IsValid = false;
        //            }
        //        }
        //    }

        //    return this.IsValid;
        //}
    }
}