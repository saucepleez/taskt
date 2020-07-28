using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Script;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("DataTable Commands")]
    [Description("This command gets a DataRow Value from a DataTable.")]

    public class GetDataRowValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("DataRow")]
        [InputSpecification("Enter an existing DataRow to get Values from.")]
        [SampleUsage("{vDataRow}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataRow { get; set; }

        [XmlAttribute]
        [PropertyDescription("Search Option")]
        [PropertyUISelectionOption("Column Name")]
        [PropertyUISelectionOption("Column Index")]
        [InputSpecification("Select whether the DataRow value should be found by column index or column name.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_Option { get; set; }

        [XmlAttribute]
        [PropertyDescription("Search Value")]
        [InputSpecification("Enter a valid DataRow index or column name.")]
        [SampleUsage("0 || {vIndex} || Column1 || {vColumnName}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataValueIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Value Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                 " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public GetDataRowValueCommand()
        {
            CommandName = "GetDataRowValueCommand";
            SelectionName = "Get DataRow Value";
            CommandEnabled = true;
            CustomRendering = true;
            v_Option = "Column Index";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var dataRowVariable = LookupVariable(engine);
            var variableList = engine.VariableList;
            DataRow dataRow;
            //check if currently looping through datatable using BeginListLoopCommand
            if (dataRowVariable.VariableValue is DataTable && engine.VariableList.Exists(x => x.VariableName == "Loop.CurrentIndex"))
            {
                var loopIndexVariable = engine.VariableList.Where(x => x.VariableName == "Loop.CurrentIndex").FirstOrDefault();
                int loopIndex = int.Parse(loopIndexVariable.VariableValue.ToString());
                dataRow = ((DataTable)dataRowVariable.VariableValue).Rows[loopIndex-1];
            }
            else 
                dataRow = (DataRow)dataRowVariable.VariableValue;

            var valueIndex = v_DataValueIndex.ConvertToUserVariable(engine);
            string value = "";
            if (v_Option == "Column Index")
            {
                int index = int.Parse(valueIndex);
                value = dataRow[index].ToString();

            }
            else if (v_Option == "Column Name")
            {
                string index = valueIndex;
                value = dataRow.Field<string>(index);
            }

            value.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataRow", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_Option", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataValueIndex", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Get Value From Column '{v_DataValueIndex}' in '{v_DataRow}' - Store Value in '{v_OutputUserVariableName}']";
        }

        private ScriptVariable LookupVariable(AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_DataRow).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if (requiredVariable == null && v_DataRow.StartsWith(sendingInstance.EngineSettings.VariableStartMarker)
                                         && v_DataRow.EndsWith(sendingInstance.EngineSettings.VariableEndMarker))
            {
                //reformat and attempt
                var reformattedVariable = v_DataRow.Replace(sendingInstance.EngineSettings.VariableStartMarker, "")
                                                   .Replace(sendingInstance.EngineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
    }
}