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
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Row to Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Row to Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ConvertDataTableRowToDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the DataTable Variable Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a existing DataTable to fet rows from.")]
        [Attributes.PropertyAttributes.SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the index of the Row")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a valid DataRow index value")]
        [Attributes.PropertyAttributes.SampleUsage("**0** or **{{{vRowIndex}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        public string v_DataRowIndex { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Specify the Variable Name To Assign The Dictionary")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        [Attributes.PropertyAttributes.PropertyParameterDirection(Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary)]
        public string v_OutputVariableName { get; set; }

        public ConvertDataTableRowToDictionaryCommand()
        {
            this.CommandName = "ConvertDataTableRowToDictionaryCommand";
            this.SelectionName = "Convert DataTable Row To Dictionary";
            this.CommandEnabled = true;
            this.CustomRendering = true;         
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            DataTable srcDT = (DataTable)v_DataTableName.GetRawVariable(engine).VariableValue;

            int index = int.Parse(v_DataRowIndex.ConvertToUserVariable(engine));
            if (index < 0)
            {
                throw new Exception("Index of row is < 0");
            }
            else if (index > srcDT.Rows.Count)
            {
                throw new Exception("Index exceeds the number of rows");
            }

            Dictionary<string, string> myDic = new Dictionary<string, string>();

            int cols = srcDT.Columns.Count;
            for (int i = 0; i < cols; i++)
            {
                if (srcDT.Rows[index][i] != null)
                {
                    myDic.Add(srcDT.Columns[i].ColumnName, srcDT.Rows[index][i].ToString());
                }
                else
                {
                    myDic.Add(srcDT.Columns[i].ColumnName, "");
                }
            }

            myDic.StoreInUserVariable(engine, v_OutputVariableName);
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
            return base.GetDisplayValue() + " [Convert DataTable '" + v_DataTableName + "'Row '" + v_DataRowIndex + "' to Dictionary '" + v_OutputVariableName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);
            if (String.IsNullOrEmpty(this.v_DataTableName))
            {
                this.validationResult += "DataTable is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_DataRowIndex))
            {
                this.validationResult += "Index is empty.\n";
                this.IsValid = false;
            }
            else
            {
                int index;
                if (int.TryParse(this.v_DataRowIndex, out index))
                {
                    if (index < 0)
                    {
                        this.validationResult += "Index value is < 0.\n";
                        this.IsValid = false;
                    }
                }
            }
            if (String.IsNullOrEmpty(this.v_OutputVariableName))
            {
                this.validationResult += "Result Dictionary is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}