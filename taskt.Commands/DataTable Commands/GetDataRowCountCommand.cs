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
    [Description("This command gets the DataRow Count of a DataTable.")]

    public class GetDataRowCountCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("DataTable")]
        [InputSpecification("Enter an existing DataTable.")]
        [SampleUsage("{vDataTable}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataTable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Count Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                 " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public GetDataRowCountCommand()
        {
            CommandName = "GetDataRowCountCommand";
            SelectionName = "Get DataRow Count";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var dataSetVariable = LookupVariable(engine);
            DataTable dataTable = (DataTable)dataSetVariable.VariableValue;
            var count = dataTable.Rows.Count.ToString();

            count.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataTable", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Count Rows in '{v_DataTable}' - Store Count in '{v_OutputUserVariableName}']";
        }

        private ScriptVariable LookupVariable(AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_DataTable).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if (requiredVariable == null && v_DataTable.StartsWith(sendingInstance.EngineSettings.VariableStartMarker)
                                         && v_DataTable.EndsWith(sendingInstance.EngineSettings.VariableEndMarker))
            {
                //reformat and attempt
                var reformattedVariable = v_DataTable.Replace(sendingInstance.EngineSettings.VariableStartMarker, "")
                                                     .Replace(sendingInstance.EngineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
    }
}