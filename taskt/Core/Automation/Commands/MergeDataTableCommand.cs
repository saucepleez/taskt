using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to merge Source DataTable into Destination DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to merge a DataTable into another DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class MergeDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the Destination DataTable Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter an existing DataTable to apply merging into.")]
        [Attributes.PropertyAttributes.SampleUsage("**myDestDataTable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_Destination_DataTableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the Source DataTable Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter an existing DataTable to merge into another one.")]
        [Attributes.PropertyAttributes.SampleUsage("**mySrcDataTable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_Source_DataTableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select Missing Schema Action")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Add")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("AddWithKey")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Ignore")]
        [Attributes.PropertyAttributes.InputSpecification("Select any Missing Schema Action")]
        [Attributes.PropertyAttributes.SampleUsage("**mySchemaAction**, **{mySchemaAction}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_MissingSchemaAction { get; set; }


        public MergeDataTableCommand()
        {
            this.CommandName = "MergeDataTableCommand";
            this.SelectionName = "Merge DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_MissingSchemaAction = "Add";
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
            var v_SourceDTVariable = LookupVariable(engine, v_Source_DataTableName);
            var v_DestinationDTVariable = LookupVariable(engine, v_Destination_DataTableName);


            // (Null Check)
            if (v_SourceDTVariable is null)
                throw new Exception("Source DataTable Variable '" + v_Source_DataTableName + "' is not initialized.");

            if (v_DestinationDTVariable is null)
                throw new Exception("Destination DataTable Variable '" + v_Destination_DataTableName + "' is not initialized.");


            // (Data Type Check)
            if (!(v_SourceDTVariable.VariableValue is DataTable))
                throw new Exception("Type of Source DataTable Variable '" + v_Source_DataTableName + "' is not DataTable.");

            if (!(v_DestinationDTVariable.VariableValue is DataTable))
                throw new Exception("Type of Destination DataTable Variable '" + v_Destination_DataTableName + "' is not DataTable.");


            // Same Variable Check
            if (v_Source_DataTableName != v_Destination_DataTableName)
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

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Destination_DataTableName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Source_DataTableName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_MissingSchemaAction", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Source '{v_Source_DataTableName}' into Destination '{v_Destination_DataTableName}']";
        }

        private Script.ScriptVariable LookupVariable(AutomationEngineInstance sendingInstance, string v_VariableName)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_VariableName).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if ((requiredVariable == null) && (v_VariableName.StartsWith(sendingInstance.engineSettings.VariableStartMarker)) && (v_VariableName.EndsWith(sendingInstance.engineSettings.VariableEndMarker)))
            {
                //reformat and attempt
                var reformattedVariable = v_VariableName.Replace(sendingInstance.engineSettings.VariableStartMarker, "").Replace(sendingInstance.engineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
    }
}