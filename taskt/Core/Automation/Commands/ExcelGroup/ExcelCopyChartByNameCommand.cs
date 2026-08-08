using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.ExcelGroup;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Chart")]
    [Attributes.ClassAttributes.CommandSettings("Copy Chart By Name")]
    [Attributes.ClassAttributes.Description("This command allows you to copy Chart by Name")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to copy Chart by Name")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelCopyChartByNameCommand : AExcelChartActionCommands, IResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ChartName))]
        [PropertyDescription("New Chart Name")]
        [PropertyValidationRule("New Chart Name", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "New Chart Name")]
        [PropertyParameterOrder(8000)]
        public string v_NewName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store New Chart Name")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Store New Chart Name", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Store New Chart Name")]
        [PropertyParameterOrder(8001)]
        public string v_Result { get; set; }

        public ExcelCopyChartByNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.ExcelChartAction(engine, new Action<Microsoft.Office.Interop.Excel.ChartObject>(chart =>
            {
                using (var bef = new InnerScriptVariable(engine))
                {
                    var getCharts = new ExcelGetChartNamesAsListCommand()
                    {
                        v_InstanceName = this.v_InstanceName,
                    };

                    getCharts.v_Result = bef.VariableName;
                    getCharts.RunCommand(engine);
                    chart.Duplicate();

                    string tempNewChartName;
                    using (var aft = new InnerScriptVariable(engine))
                    {
                        getCharts.v_Result = aft.VariableName;
                        getCharts.RunCommand(engine);

                        using (var uncommon = new InnerScriptVariable(engine))
                        {
                            var getUncommon = new GetCommonValuesOfListsCommand()
                            {
                                v_ListA = bef.VariableName,
                                v_ListB = aft.VariableName,
                                v_ListBOnly = uncommon.VariableName,
                            };
                            getUncommon.RunCommand(engine);

                            var lst = (List<string>)uncommon.VariableValue;
                            if (lst.Count == 0)
                            {
                                throw new Exception("New chart does not exists");
                            }
                            tempNewChartName = lst[0];
                        }
                    }

                    if (string.IsNullOrEmpty(v_NewName))
                    {
                        StoreNewChartName(tempNewChartName, engine);
                    }
                    else
                    {
                        // rename new chart
                        var newChart = this.ExpandValueOrUserVariable(nameof(v_NewName), "New Name", engine);
                        var renameChart = new ExcelRenameChartByNameCommand()
                        {
                            v_InstanceName = this.v_InstanceName,
                            v_ChartName = tempNewChartName,
                            v_NewName = newChart,
                        };
                        renameChart.RunCommand(engine);

                        StoreNewChartName(newChart, engine);
                    }
                }
            }));
        }

        /// <summary>
        /// store new chart name to v_Result
        /// </summary>
        /// <param name="newChartName"></param>
        /// <param name="engine"></param>
        private void StoreNewChartName(string newChartName, Engine.AutomationEngineInstance engine)
        {
            if (!string.IsNullOrEmpty(v_Result))
            {
                newChartName.StoreInUserVariable(engine, v_Result);
            }
        }
    }
}