using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;

namespace taskt.Core.Automation.Commands.ExcelGroup
{
    /// <summary>
    /// do something to Excel Shapes commands
    /// </summary>
    public abstract class AExcelShapesCommands : AExcelInstanceCommands, ICanHandleExcelShapes
    {
        /// <summary>
        /// excel shapes action
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="shapesFunc"></param>
        /// <param name="errorFunc"></param>
        protected void ExcelShapesAction(Engine.AutomationEngineInstance engine, Action<List<Shape>> shapesFunc, Action<Exception> errorFunc = null)
        {
            (_, var sht) = this.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);
            var charts = this.GetShapesFromWorksheet(sht);
            try
            {
                shapesFunc(charts);
            }
            catch (Exception ex)
            {
                if (errorFunc != null)
                {
                    errorFunc(ex);
                }
                else
                {
                    throw ex;
                }
            }
        }
    }
}
