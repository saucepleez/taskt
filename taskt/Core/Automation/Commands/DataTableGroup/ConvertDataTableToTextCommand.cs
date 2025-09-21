using System;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Convert DataTable To Text")]
    [Attributes.ClassAttributes.Description("This command allows you to Convert DataTable to Text.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Convert DataTable to Text.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertDataTableToTextCommand : ADataTableGetFromDataTableCommands
    {
        //[XmlAttribute]
        //public string v_DataTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(6000)]
        public override string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Export Header")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [PropertyValidationRule("Export Header", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(7000)]
        public string v_ExportHeader { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Export Row Index")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [PropertyValidationRule("Export Row Index", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(7001)]
        public string v_ExportIndex { get; set; }

        public ConvertDataTableToTextCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var dt = this.ExpandUserVariableAsDataTable(engine);
            var columnNum = dt.Columns.Count;

            var exportHeader = this.ExpandValueOrUserVariableAsYesNo(nameof(v_ExportHeader), engine);
            var exportIndex = this.ExpandValueOrUserVariableAsYesNo(nameof(v_ExportIndex), engine);

            string txt = "";
            if (exportHeader)
            {
                if (exportIndex)
                {
                    txt = "index,";
                }
                string t = "";
                foreach (DataColumn c in dt.Columns)
                {
                    t += $"{c.ColumnName},";
                }
                txt += t.Substring(0, t.Length - 1) + "\r\n";
            }

            string rowTextAction(DataRow row, int column)
            {
                string v = "";
                
                for (int i = 0; i < column; i++)
                {
                    v += $"{row[i]?.ToString() ?? ""},";
                }
                return v.Substring(0, v.Length - 1) + "\r\n";
            }

            Func<DataRow, int, string> textAction;
            if (exportIndex)
            {
                textAction = new Func<DataRow, int, string>((r, index) =>
                {
                    return $"{index},{rowTextAction(r, columnNum)}";
                });
            }
            else
            {
                textAction = new Func<DataRow, int, string>((r, index) =>
                {
                    return rowTextAction(r, columnNum);
                });
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt += textAction(dt.Rows[i], i);
            }

            txt.Trim().StoreInUserVariable(engine, v_Result);
        }
    }
}