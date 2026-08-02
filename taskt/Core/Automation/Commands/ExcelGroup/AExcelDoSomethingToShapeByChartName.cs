using Microsoft.Office.Interop.Excel;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.ExcelGroup
{
    public abstract class AExcelDoSomethingToShapeByChartName : AExcelShapesCommands, IExcelShapeNameProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ShapeName))]
        [PropertyParameterOrder(7000)]
        public virtual string v_ShapeName { get; set; }

        /// <summary>
        /// one chart action by name
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="shapeFunc"></param>
        /// <param name="errorFunc"></param>
        protected void ExcelChartAction(Engine.AutomationEngineInstance engine, Action<Shape> shapeFunc, Action<Exception> errorFunc = null)
        {
            (_, var sht) = this.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);

            var shapeName = this.ExpandValueOrUserVariable(nameof(v_ShapeName), "Shape Name", engine);

            try
            {
                var shape = this.GetShapeFromName(sht, shapeName);
                shapeFunc(shape);
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
