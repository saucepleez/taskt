using System;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command gets a range of cells and applies them against a dataset")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to quickly iterate over Excel as a dataset.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'OLEDB' to achieve automation.")]
    public class CreateDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please create a DataSet name")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate a unique reference name for later use")]
        [Attributes.PropertyAttributes.SampleUsage("vMyDataset")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please the names of your columns")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the actual names of your columns.")]
        [Attributes.PropertyAttributes.SampleUsage("name1,name2,name3,name4")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataTableColumnNames { get; set; }

        public CreateDataTableCommand()
        {
            this.CommandName = "CreateDataTableCommand";
            this.SelectionName = "Create DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vDataTableColumnNames = v_DataTableColumnNames.ConvertToUserVariable(sender);

            //DatasetCommands dataSetCommand = new DatasetCommands();
            var splittext = vDataTableColumnNames.Split(',');
            DataTable Dt = new DataTable();

            foreach(var item in splittext)
            {
                Dt.Columns.Add(item);
            }

            Script.ScriptVariable newDataTable = new Script.ScriptVariable
            {
                VariableName = v_DataTableName,
                VariableValue = Dt
            };

            engine.VariableList.Add(newDataTable);




        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataTableName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataTableColumnNames", this, editor));

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }
}