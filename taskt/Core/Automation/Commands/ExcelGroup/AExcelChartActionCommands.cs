using Microsoft.Office.Interop.Excel;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.ExcelGroup
{
    /// <summary>
    /// for excel chart action commands
    /// </summary>
    public abstract class AExcelChartActionCommands : AExcelDoSomethingToChartByChartName, IWhenFailActionBehaviorProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_WhenFailAction))]
        [PropertyParameterOrder(10000)]
        public virtual string v_WhenFailAction { get; set; }

        /// <summary>
        /// excel chart action when error use v_WhenFailAction
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="chartFunc"></param>
        protected void ExcelChartAction(Engine.AutomationEngineInstance engine, Action<ChartObject> chartFunc)
        {
            this.ExcelChartAction(engine, chartFunc, new Action<Exception>(ex => 
            {
                switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenFailAction), engine))
                {
                    case "ignore":
                        break;
                    case "error":
                        throw ex;
                }
            }));
        }
    }
}
