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
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.Description("This command allows you to get a DataRow Value from a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a datarow to a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class GetDataRowValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the DataRow Variable Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a existing DataRow to get Values from.")]
        [SampleUsage("**myDataRow** or **{{{vMyDataRow}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_DataRowName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Select value by Index or Column Name (Default is Index)")]
        [PropertyUISelectionOption("Index")]
        [PropertyUISelectionOption("Column Name")]
        [InputSpecification("Select whether the DataRow value should be found by index or column name")]
        [SampleUsage("Select from **Index** or **Column Name**")]
        [Remarks("")]
        [PropertyIsOptional(true)]
        public string v_Option { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please enter the index of the DataRow Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a valid DataRow index value")]
        [SampleUsage("**0** or **ColName** or **{{{vIndex}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        public string v_DataValueIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify the Variable to Assign the Value")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        public string v_UserVariableName { get; set; }

        public GetDataRowValueCommand()
        {
            this.CommandName = "GetDataRowValueCommand";
            this.SelectionName = "Get DataRow Value";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            
            this.v_Option = "Index";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            DataRow dataRow = (DataRow)v_DataRowName.GetRawVariable(engine).VariableValue;


            var valueIndex = v_DataValueIndex.ConvertToUserVariable(sender);

            if (String.IsNullOrEmpty(v_Option))
            {
                v_Option = "Index";
            }

            string value = "";
            if (v_Option == "Index")
            {
                int index = int.Parse(valueIndex);
                value = dataRow[index].ToString();
            }
            else if (v_Option == "Column Name")
            {
                string index = valueIndex;
                value = dataRow.Field<string>(index);
            }

            value.StoreInUserVariable(sender, v_UserVariableName);

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
            return base.GetDisplayValue() + $" [Get Value '{v_DataValueIndex}' from '{v_DataRowName}', Store In: '{v_UserVariableName}']";
        }
    }
}