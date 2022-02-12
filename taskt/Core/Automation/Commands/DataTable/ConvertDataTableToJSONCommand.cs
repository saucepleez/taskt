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
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable to JSON")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable to JSON.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ConvertDataTableToJSONCommand : ScriptCommand
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
        [Attributes.PropertyAttributes.PropertyDescription("Please Specify the Variable Name To Assign The JSON")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        [Attributes.PropertyAttributes.PropertyParameterDirection(Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.JSON, true)]
        public string v_OutputVariableName { get; set; }

        public ConvertDataTableToJSONCommand()
        {
            this.CommandName = "ConvertDataTableToJSONCommand";
            this.SelectionName = "Convert DataTable To JSON";
            this.CommandEnabled = true;
            this.CustomRendering = true;         
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            //DataTable srcDT = (DataTable)v_DataTableName.GetRawVariable(engine).VariableValue;
            DataTable srcDT = v_DataTableName.GetDataTableVariable(engine);

            List<Dictionary<string, string>> jsonList = new List<Dictionary<string, string>>();
            for (int j = 0; j < srcDT.Rows.Count; j++)
            {
                Dictionary<string, string> tDic = new Dictionary<string, string>();
                for (int i = 0; i < srcDT.Columns.Count; i++)
                {
                    tDic.Add(srcDT.Columns[i].ColumnName, srcDT.Rows[j][i].ToString());
                }

                jsonList.Add(tDic);
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(jsonList);
            json.StoreInUserVariable(engine, v_OutputVariableName);
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
            return base.GetDisplayValue() + " [Convert DataTable '" + v_DataTableName + "' to JSON '" + v_OutputVariableName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);
            if (String.IsNullOrEmpty(this.v_DataTableName))
            {
                this.validationResult += "DataTable is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_OutputVariableName))
            {
                this.validationResult += "Result JSON is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}