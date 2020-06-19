using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Script;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("DataTable Commands")]
    [Description("This command merges a source DataTable into a destination DataTable.")]

    public class MergeDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Source DataTable")]
        [InputSpecification("Enter an existing DataTable to merge into another one.")]
        [SampleUsage("{vSrcDataTable}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SourceDataTable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Destination DataTable")]
        [InputSpecification("Enter an existing DataTable to apply the merge operation to.")]
        [SampleUsage("{vDestDataTable}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DestinationDataTable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Missing Schema Action")]
        [PropertyUISelectionOption("Add")]
        [PropertyUISelectionOption("AddWithKey")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Ignore")]
        [InputSpecification("Select any Missing Schema Action.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_MissingSchemaAction { get; set; }

        public MergeDataTableCommand()
        {
            CommandName = "MergeDataTableCommand";
            SelectionName = "Merge DataTable";
            CommandEnabled = true;
            CustomRendering = true;
            v_MissingSchemaAction = "Add";
        }

        public override void RunCommand(object sender)
        {
            /* ------------Before Merge Operation, following conditions must be checked---------------

            1. None of the (Source, Destination) DataTable Variables is null            -->     (Null Check)
            2. Data Type of both (Source, Destination) Variables must be DataTable      -->     (Data Type Check)
            3. Source and Destination DataTable Varibales must not be the same          -->     (Same Variable Check)

             */
            var engine = (AutomationEngineInstance)sender;

            // Get Variable Objects
            var v_SourceDTVariable = LookupVariable(engine, v_SourceDataTable);
            var v_DestinationDTVariable = LookupVariable(engine, v_DestinationDataTable);

            // (Null Check)
            if (v_SourceDTVariable is null)
                throw new Exception("Source DataTable Variable '" + v_SourceDataTable + "' is not initialized.");

            if (v_DestinationDTVariable is null)
                throw new Exception("Destination DataTable Variable '" + v_DestinationDataTable + "' is not initialized.");

            // (Data Type Check)
            if (!(v_SourceDTVariable.VariableValue is DataTable))
                throw new Exception("Type of Source DataTable Variable '" + v_SourceDataTable + "' is not DataTable.");

            if (!(v_DestinationDTVariable.VariableValue is DataTable))
                throw new Exception("Type of Destination DataTable Variable '" + v_DestinationDataTable + "' is not DataTable.");

            // Same Variable Check
            if (v_SourceDataTable != v_DestinationDataTable)
            {
                var sourceDT = (DataTable)v_SourceDTVariable.VariableValue;
                var destinationDT = (DataTable)v_DestinationDTVariable.VariableValue;

                switch (v_MissingSchemaAction)
                {
                    case "Add":
                        destinationDT.Merge(sourceDT, false, MissingSchemaAction.Add);
                        break;
                    case "AddWithKey":
                        destinationDT.Merge(sourceDT, false, MissingSchemaAction.AddWithKey);
                        break;
                    case "Error":
                        destinationDT.Merge(sourceDT, false, MissingSchemaAction.Error);
                        break;
                    case "Ignore":
                        destinationDT.Merge(sourceDT, false, MissingSchemaAction.Ignore);
                        break;
                    default:
                        throw new NotImplementedException("Missing Schema Action '" + v_MissingSchemaAction + "' not implemented");
                }

                // Update Destination Variable Value
                v_DestinationDTVariable.VariableValue = destinationDT;
            }

        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceDataTable", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DestinationDataTable", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_MissingSchemaAction", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Merge Source '{v_SourceDataTable}' Into Destination '{v_DestinationDataTable}']";
        }

        private ScriptVariable LookupVariable(AutomationEngineInstance sendingInstance, string v_VariableName)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_VariableName).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if (requiredVariable == null && v_VariableName.StartsWith(sendingInstance.EngineSettings.VariableStartMarker) 
                                         && v_VariableName.EndsWith(sendingInstance.EngineSettings.VariableEndMarker))
            {
                //reformat and attempt
                var reformattedVariable = v_VariableName.Replace(sendingInstance.EngineSettings.VariableStartMarker, "")
                                                        .Replace(sendingInstance.EngineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
    }
}