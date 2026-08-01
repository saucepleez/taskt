using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.ExcelGroup;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Chart")]
    [Attributes.ClassAttributes.CommandSettings("Rename Chart By Name")]
    [Attributes.ClassAttributes.Description("This command allows you to rename Chart by Name")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to rename Chart by Name")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelRenameChartByNameCommand : AExcelChartActionCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("New Chart Name")]
        [PropertyDisplayText(false, "New  Name")]
        [PropertyDetailSampleUsage("Chart10", PropertyDetailSampleUsage.ValueType.Value, "Chart Name")]
        [PropertyDetailSampleUsage("{{{vChart}}}", PropertyDetailSampleUsage.ValueType.VariableValue, "Chart Name")]
        [PropertyValidationRule("New Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyParameterOrder(8000)]
        public string v_NewName { get; set; }

        public ExcelRenameChartByNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.ExcelChartAction(engine, new Action<Microsoft.Office.Interop.Excel.ChartObject>(chart =>
            {
                var newName = this.ExpandValueOrUserVariable(nameof(v_NewName), "New Name", engine);

                if (chart.Name != newName)
                {
                    using (var re = new InnerScriptVariable(engine))
                    {
                        var checkChart = new ExcelCheckChartExistsByNameCommand()
                        {
                            v_InstanceName = this.v_InstanceName,
                            v_ChartName = newName,
                            v_Result = re.VariableName,
                        };
                        checkChart.RunCommand(engine);

                        var isExists = bool.Parse(re.VariableValue.ToString());
                        if (!isExists)
                        {
                            chart.Name = newName;
                        }
                        else
                        {
                            throw new Exception($"Chart Name is already Used. Name: {v_NewName}, Expanded Value: '{newName}'");
                        }
                    }
                }
            }));
        }
    }
}