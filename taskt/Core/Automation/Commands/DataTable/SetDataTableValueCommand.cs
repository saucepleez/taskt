using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("DataTable Action")]
    [Attributes.ClassAttributes.CommandSettings("Set DataTable Value")]
    [Attributes.ClassAttributes.Description("This command allows you to set the DataTable value")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set the DataTable value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetDataTableValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnType))]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnNameIndex))]
        public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Value to Set")]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyDetailSampleUsage("**123**", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyDisplayText(true, "Value to Set")]
        public string v_NewValue { get; set; }

        public SetDataTableValueCommand()
        {
            //this.CommandName = "SetDataTableValueCommand";
            //this.SelectionName = "Set DataTable Value";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var myDT, var rowIndex, var columnIndex) = this.GetDataTableVariableAndRowColumnIndeies(nameof(v_DataTableName), nameof(v_RowIndex), nameof(v_ColumnType), nameof(v_ColumnIndex), engine);

            string newValue = v_NewValue.ConvertToUserVariable(engine);

            myDT.Rows[rowIndex][columnIndex] = newValue;
        }
    }
}