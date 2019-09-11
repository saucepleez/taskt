using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to parse a dataset row column into a variable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to extract data from a dataset variable")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ParseDatasetRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Supply the name of the variable containing the datasource")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DatasetName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select Column Parse Type")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("By Column Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("By Column Index")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ColumnParseType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Specify Column Name or Index")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ColumnParameter { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the extracted column data")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Specify Alternate Row Number")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("If not executing within a loop, select the applicable index of the row required")]
        [Attributes.PropertyAttributes.SampleUsage("**0** or **vRowNumber**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SpecifiedRow { get; set; }
        public ParseDatasetRowCommand()
        {
            this.CommandName = "ParseRowCommand";
            this.SelectionName = "Parse Dataset Row";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_SpecifiedRow = "N/A";
            this.v_ColumnParseType = "By Column Name";   
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            //try to find dataset based on variable name
            var dataSourceVariable = engine.VariableList.FirstOrDefault(f => engine.engineSettings.VariableStartMarker + f.VariableName + engine.engineSettings.VariableEndMarker == v_DatasetName);

            if (dataSourceVariable == null)
            {
                //see if match is found without encasing
                dataSourceVariable = engine.VariableList.FirstOrDefault(f => f.VariableName == v_DatasetName);

                //no match was found
                if (dataSourceVariable == null)
                {
                    throw new Exception($"Data Source {v_DatasetName} Not Found! Did you input the correct name?");
                }

            }

            var columnName = v_ColumnParameter.ConvertToUserVariable(sender);
            var parseStrat = v_ColumnParseType.ConvertToUserVariable(sender);
            //get datatable
            var dataTable = (System.Data.DataTable)dataSourceVariable.VariableValue;

            int requiredRowNumber;

            if (!int.TryParse(this.v_SpecifiedRow.ConvertToUserVariable(sender), out requiredRowNumber))
            {
                requiredRowNumber = dataSourceVariable.CurrentPosition;
            }

            //get required row
            var requiredRow = dataTable.Rows[requiredRowNumber];

            //parse column name based on requirement
            object requiredColumn;
            if (parseStrat == "By Column Index")
            {
                requiredColumn = requiredRow[int.Parse(columnName)];
            }
            else
            {
                requiredColumn = requiredRow[columnName];
            }


            //store value in variable
            requiredColumn.ToString().StoreInUserVariable(sender, v_applyToVariableName);

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DatasetName", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ColumnParseType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ColumnParameter", this, editor));

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SpecifiedRow", this, editor));

            return RenderedControls;

        }



        public override string GetDisplayValue()
        {
            return $"{base.GetDisplayValue()} - [Select '{v_ColumnParameter}' {v_ColumnParseType} from '{v_DatasetName}', Apply Result(s) To Variable: {v_applyToVariableName}]";
        }
    }
}