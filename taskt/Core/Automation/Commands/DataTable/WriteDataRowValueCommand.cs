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
    [Attributes.ClassAttributes.Description("This command allows you to write a Value to a DataRow")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to write a Value to a DataRow.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class WriteDataRowValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the DataRow Variable Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a existing DataTable to add rows to.")]
        [SampleUsage("**myDataRow** or **{{{vMyDataRow}}}**")]
        [Remarks("")]
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
        [SampleUsage("**0** or **{{{vIndex}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        public string v_DataValueIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please enter the Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the value to write to the DataRow cell")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("")]
        public string v_DataRowValue { get; set; }

        public WriteDataRowValueCommand()
        {
            this.CommandName = "WriteDataRowValueCommand";
            this.SelectionName = "Write DataRow Value";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            v_Option = "Index";

        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var dataRowValue = v_DataRowValue.ConvertToUserVariable(sender);

            DataRow dataRow = (DataRow)v_DataRowName.GetRawVariable(engine).VariableValue;

            var valueIndex = v_DataValueIndex.ConvertToUserVariable(sender);

            if (String.IsNullOrEmpty(v_Option))
            {
                v_Option = "Index";
            }

            if (v_Option == "Index")
            {
                int index = int.Parse(valueIndex);
                dataRow[index] = dataRowValue;

            }
            else if (v_Option == "Column Name")
            {
                string index = valueIndex;
                dataRow.SetField(index, dataRowValue);
            }
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
            return base.GetDisplayValue() + $" [Write Value '{v_DataRowValue}' to '{v_DataValueIndex}' in '{v_DataRowName}']";
        }
    }
}