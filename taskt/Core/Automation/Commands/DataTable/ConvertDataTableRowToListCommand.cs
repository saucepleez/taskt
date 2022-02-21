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
    [Attributes.ClassAttributes.SubGruop("Convert Row")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Row to List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Row to List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ConvertDataTableRowToListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the DataTable Variable Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a existing DataTable to fet rows from.")]
        [SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please enter the index of the Row")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a valid DataRow index value")]
        [SampleUsage("**0** or **1** or **-1** or **{{{vRowIndex}}}**")]
        [Remarks("**-1** means index of the last row.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyIsOptional(true, "Current Row")]
        public string v_DataRowIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify the Variable Name To Assign The List")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_OutputVariableName { get; set; }

        public ConvertDataTableRowToListCommand()
        {
            this.CommandName = "ConvertDataTableRowToListCommand";
            this.SelectionName = "Convert DataTable Row To List";
            this.CommandEnabled = true;
            this.CustomRendering = true;         
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //DataTable srcDT = (DataTable)v_DataTableName.GetRawVariable(engine).VariableValue;
            DataTable srcDT = v_DataTableName.GetDataTableVariable(engine);

            //int index = 0;
            //if (String.IsNullOrEmpty(v_DataRowIndex))
            //{
            //    index = v_DataTableName.GetRawVariable(engine).CurrentPosition;
            //}
            //else
            //{
            //    index = int.Parse(v_DataRowIndex.ConvertToUserVariable(engine));
            //    if (index < 0)
            //    {
            //        index = srcDT.Rows.Count + index;
            //    }
            //}

            //if ((index < 0) || (index >= srcDT.Rows.Count))
            //{
            //    throw new Exception("Strange Row Index " + v_DataRowIndex + ", parsed " + index);
            //}
            int index = DataTableControls.GetRowIndex(v_DataTableName, v_DataRowIndex, engine);

            List<string> myList = new List<string>();

            int cols = srcDT.Columns.Count;
            for (int i = 0; i < cols; i++)
            {
                if (srcDT.Rows[index][i] != null)
                {
                    myList.Add(srcDT.Rows[index][i].ToString());
                }
                else
                {
                    myList.Add("");
                }
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
            return base.GetDisplayValue() + " [Convert DataTable '" + v_DataTableName + "'Row '" + v_DataRowIndex + "' to List '" + v_OutputVariableName + "']";
        }

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);
        //    if (String.IsNullOrEmpty(this.v_DataTableName))
        //    {
        //        this.validationResult += "DataTable is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_DataRowIndex))
        //    {
        //        this.validationResult += "Index is empty.\n";
        //        this.IsValid = false;
        //    }
        //    else
        //    {
        //        int index;
        //        if (int.TryParse(this.v_DataRowIndex, out index))
        //        {
        //            if (index < 0)
        //            {
        //                this.validationResult += "Index value is < 0.\n";
        //                this.IsValid = false;
        //            }
        //        }
        //    }
        //    if (String.IsNullOrEmpty(this.v_OutputVariableName))
        //    {
        //        this.validationResult += "Result List is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}