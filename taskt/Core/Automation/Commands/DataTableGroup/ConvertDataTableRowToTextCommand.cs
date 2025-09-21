using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("Convert Row")]
    [Attributes.ClassAttributes.CommandSettings("Convert DataTable Row To Text")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Row to Text.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Row to Text.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertDataTableRowToTextCommand : ADataTableGetFromDataTableRowCommands
    {
        //public string v_DataTable { get; set; }

        //public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(7000)]
        public override string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Export Header")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [PropertyValidationRule("Export Header", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(9000)]
        public string v_ExportHeader { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Export Row Index")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [PropertyValidationRule("Export Row Index", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(9001)]
        public string v_ExportIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("When Exporting Row Index, Export the Actual Row Index")]
        [PropertyIsOptional(true, "Yes")]
        [PropertyFirstValue("Yes")]
        [PropertyValidationRule("Actual Row Index", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(10000)]
        public string v_ActualRowIndex { get; set; }

        public ConvertDataTableRowToTextCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            using (var myDT = new InnerScriptVariable(engine))
            {
                var rowDT = new ConvertDataTableRowToDataTableCommand()
                {
                    v_DataTable = this.v_DataTable,
                    v_RowIndex = this.v_RowIndex,
                    v_Result = myDT.VariableName,
                };
                rowDT.RunCommand(engine);

                using (var myRes = new InnerScriptVariable(engine))
                {
                    var convertText = new ConvertDataTableToTextCommand()
                    {
                        v_DataTable = myDT.VariableName,
                        v_ExportHeader = this.v_ExportHeader,
                        v_ExportIndex = this.v_ExportIndex,
                        v_Result = myRes.VariableName,
                    };
                    convertText.RunCommand(engine);

                    if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ExportIndex), engine) &&
                        this.ExpandValueOrUserVariableAsYesNo(nameof(v_ActualRowIndex), engine))
                    {
                        (_ , var row) = this.ExpandValueOrUserVariableAsDataTableAndRow(engine);

                        var t = myRes.VariableValue.ToString();
                        if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ExportHeader), engine))
                        {
                            // line 2
                            var spt = t.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                            var values = spt[1].Substring(spt[1].IndexOf(','));
                            $"{spt[0]}\r\n{row}{values}".StoreInUserVariable(engine, v_Result);
                        }
                        else
                        {
                            // line 1
                            var values = t.Substring(t.IndexOf(','));
                            $"{row}{values}".StoreInUserVariable(engine, v_Result);
                        }
                    }
                    else
                    {
                        myRes.VariableValue.ToString().StoreInUserVariable(engine, v_Result);
                    }
                }
            }
        }
    }
}