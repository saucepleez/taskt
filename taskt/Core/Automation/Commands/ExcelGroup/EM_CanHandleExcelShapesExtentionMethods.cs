using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;

namespace taskt.Core.Automation.Commands.ExcelGroup
{
    public static class EM_CanHandleExcelShapesExtentionMethods
    {
        /// <summary>
        /// get shapes from workhseet
        /// </summary>
        /// <param name="command"></param>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static List<Shape> GetShapesFromWorksheet(this ICanHandleExcelShapes command, Worksheet sheet)
        {
            var shapes = sheet.Shapes;

            var ret = new List<Shape>();
            foreach (Shape shape in shapes)
            {
                ret.Add(shape);
            }
            return ret;
        }

        /// <summary>
        /// get shape from shape name
        /// </summary>
        /// <param name="command"></param>
        /// <param name="sheet"></param>
        /// <param name="shapeName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Shape GetShapeFromName(this ICanHandleExcelShapes command, Worksheet sheet, string shapeName)
        {
            var shapes = GetShapesFromWorksheet(command, sheet);

            foreach (var shape in shapes)
            {
                if (shape.Name == shapeName)
                {
                    return shape;
                }
            }

            throw new Exception($"Shape Not Found. Shape Name: '{shapeName}', Sheet Name: '{sheet.Name}'");
        }

        /// <summary>
        /// get shape from index
        /// </summary>
        /// <param name="command"></param>
        /// <param name="sheet"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Shape GetShapeFromIndex(this ICanHandleExcelShapes command, Worksheet sheet, int index)
        {
            var shapes = GetShapesFromWorksheet(command, sheet);

            if (index < 0)
            {
                index += shapes.Count;
            }
            if (index >= 0 && index < shapes.Count)
            {
                return shapes[index];
            }
            else
            {
                throw new Exception($"Shape Not Found. Shape Index: '{index}', Sheet Name: '{sheet.Name}'");
            }
        }
    }
}
