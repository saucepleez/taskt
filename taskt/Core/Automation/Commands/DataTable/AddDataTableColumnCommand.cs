using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Column Action")]
    [Attributes.ClassAttributes.CommandSettings("Add DataTable Column")]
    [Attributes.ClassAttributes.Description("This command allows you to add a column to a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a column to a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class AddDataTableColumnCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Column Name to Add")]
        [PropertyDetailSampleUsage("**newColumn**", PropertyDetailSampleUsage.ValueType.Value, "New Column")]
        [PropertyDetailSampleUsage("**{{{vColumn}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "New Column")]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Column")]
        public string v_AddColumnName { get; set; }

        [XmlAttribute]
        [PropertyDescription("When Column Exists")]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [PropertyDetailSampleUsage("**Ignore**", "Do not add New Column")]
        [PropertyDetailSampleUsage("**Replace**", "Remove Current Column and Add New Column")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Replace")]
        [PropertyIsOptional(true, "Error")]
        public string v_IfColumnExists { get; set; }

        public AddDataTableColumnCommand()
        {
            //this.CommandName = "AddDataTableColumnCommand";
            //this.SelectionName = "Add DataTable Column";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            DataTable myDT = v_DataTableName.GetDataTableVariable(engine);

            string newColName = v_AddColumnName.ConvertToUserVariable(engine);

            string ifColumnExists = this.GetUISelectionValue(nameof(v_IfColumnExists), "If Column Exists", engine);

            for (int i = 0; i < myDT.Columns.Count; i++)
            {
                if (newColName == myDT.Columns[i].ColumnName)
                {
                    switch (ifColumnExists)
                    {
                        case "error":
                            throw new Exception("Column Name " + v_AddColumnName + " is already exists");

                        case "ignore":
                            return;

                        case "replace":
                            myDT.Columns.Remove(newColName);
                            break;
                    }
                }
            }

            myDT.Columns.Add(newColName);
        }
    }
}