using System;
using System.Collections.Generic;
using System.Reflection;
using static taskt.Core.Automation.Commands.PropertyControls;

namespace taskt.Core.Automation.Commands
{
    internal static class IntermediateControls
    {
        /// <summary>
        /// proprety value convert to intermediate. this method use default convert method.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="settings"></param>
        /// <param name="variables"></param>
        public static void ConvertToIntermediate(ScriptCommand command, EngineSettings settings, List<Script.ScriptVariable> variables)
        {
            var props = command.GetParameterProperties(true);
            foreach (var prop in props)
            {
                var targetValue = prop.GetValue(command);
                if (targetValue != null)
                {
                    if (targetValue is string)
                    {
                        targetValue = settings.convertToIntermediate(targetValue.ToString());
                        prop.SetValue(command, targetValue);
                    }
                    else if (targetValue is System.Data.DataTable table)
                    {
                        table.AcceptChanges();
                        var trgDT = table.Copy();
                        var rows = trgDT.Rows.Count;
                        var cols = trgDT.Columns.Count;
                        for (int i = 0; i < cols; i++)
                        {
                            if (trgDT.Columns[i].ReadOnly)
                            {
                                continue;
                            }
                            for (int j = 0; j < rows; j++)
                            {
                                var v = settings.convertToIntermediate(trgDT.Rows[j][i]?.ToString() ?? "");
                                trgDT.Rows[j][i] = v;
                            }
                        }
                        prop.SetValue(command, trgDT);
                    }
                }
            }
        }

        public static void ConvertToIntermediate(ScriptCommand command, EngineSettings settings, Dictionary<string, string> convertMethods, List<Script.ScriptVariable> variables)
        {
            Type settingsType = settings.GetType();
            var props = command.GetParameterProperties(true);
            foreach (var prop in props)
            {
                var targetValue = prop.GetValue(command);
                if (targetValue != null)
                {
                    // set method
                    Func<string, object> convertMethod;
                    MethodInfo convertMethodInfo = null;
                    if (convertMethods.ContainsKey(prop.Name))
                    {
                        switch (prop.Name)
                        {
                            case nameof(settings.convertToIntermediateVariableParser):
                                convertMethodInfo = settingsType.GetMethod(convertMethods[prop.Name], new Type[] { typeof(string), typeof(List<Script.ScriptVariable>) });
                                convertMethod = new Func<string, object>((str) =>
                                {
                                    return convertMethodInfo.Invoke(settings, new object[] { str, variables });
                                });
                                break;
                            default:
                                convertMethodInfo = settingsType.GetMethod(convertMethods[prop.Name], new Type[] { typeof(string) });
                                convertMethod = new Func<string, object>((str) =>
                                {
                                    return convertMethodInfo.Invoke(settings, new object[] { str });
                                });
                                break;
                        }
                    }
                    else
                    {
                        convertMethodInfo = settingsType.GetMethod(nameof(settings.convertToIntermediate), new Type[] { typeof(string) });
                        convertMethod = new Func<string, object>((str) =>
                        {
                            return convertMethodInfo.Invoke(settings, new object[] { str });
                        });
                    }

                    // converting
                    if (targetValue is string targetStr)
                    {
                        //switch (methodOfConverting.Name)
                        //{
                        //    case nameof(settings.convertToIntermediateVariableParser):
                        //        targetValue = methodOfConverting.Invoke(settings, new object[] { targetValue.ToString(), variables });
                        //        break;
                        //    default:
                        //        targetValue = methodOfConverting.Invoke(settings, new object[] { targetValue.ToString() });
                        //        break;
                        //}
                        prop.SetValue(command, convertMethod(targetStr));
                    }
                    else if (targetValue is System.Data.DataTable table)
                    {
                        table.AcceptChanges();
                        var trgDT = table.Copy();
                        var rows = trgDT.Rows.Count;
                        var cols = trgDT.Columns.Count;
                        for (int i = 0; i < cols; i++)
                        {
                            if (trgDT.Columns[i].ReadOnly)
                            {
                                continue;
                            }
                            for (int j = 0; j < rows; j++)
                            {
                                //object newCellValue;
                                //switch (methodOfConverting.Name)
                                //{
                                //    case nameof(settings.convertToIntermediateVariableParser):
                                //        newCellValue = methodOfConverting.Invoke(settings, new object[] { trgDT.Rows[j][i].ToString(), variables });
                                //        break;
                                //    default:
                                //        newCellValue = methodOfConverting.Invoke(settings, new object[] { trgDT.Rows[j][i].ToString() });
                                //        break;
                                //}
                                trgDT.Rows[j][i] = convertMethod(trgDT.Rows[j][i]?.ToString() ?? "");
                            }
                        }
                        prop.SetValue(command, trgDT);
                    }
                }
            }
        }

        public static void ConvertToRaw(ScriptCommand command, EngineSettings settings)
        {
            var myPropaties = command.GetParameterProperties(true);
            foreach (var prop in myPropaties)
            {
                var targetValue = prop.GetValue(command);
                if (targetValue != null)
                {
                    if (targetValue is string)
                    {
                        targetValue = settings.convertToRaw(targetValue.ToString());
                        prop.SetValue(command, targetValue);
                    }
                    else if (targetValue is System.Data.DataTable table)
                    {
                        var rows = table.Rows.Count;
                        var cols = table.Columns.Count;
                        for (int i = 0; i < cols; i++)
                        {
                            if (table.Columns[i].ReadOnly)
                            {
                                continue;
                            }
                            for (int j = 0; j < rows; j++)
                            {
                                var v = settings.convertToRaw(table.Rows[j][i]?.ToString() ?? "");
                                table.Rows[j][i] = v;
                            }
                        }
                        prop.SetValue(command, table);
                    }
                }
            }
        }

        public static void ConvertToRaw(ScriptCommand command, EngineSettings settings, Dictionary<string, string> convertMethods)
        {
            Type settingsType = settings.GetType();
            var myPropaties = command.GetParameterProperties(true);
            foreach (var prop in myPropaties)
            {
                var targetValue = prop.GetValue(command);
                if (targetValue != null)
                {
                    MethodInfo methodOfConverting = null;
                    //foreach (var meth in convertMethods)
                    //{
                    //    if (meth.Key == prop.Name)
                    //    {
                    //        methodOfConverting = settingsType.GetMethod(meth.Value);
                    //        break;
                    //    }
                    //}
                    Func<string, string> convertMethod = null;
                    if (convertMethods.ContainsKey(prop.Name))
                    {
                        methodOfConverting = settingsType.GetMethod(convertMethods[prop.Name]);
                    }
                    else
                    {
                        methodOfConverting = settingsType.GetMethod(nameof(settings.convertToRaw));
                    }
                    convertMethod = new Func<string, string>((str) =>
                    {
                        return methodOfConverting.Invoke(settings, new object[] { str }).ToString();
                    });

                    if (targetValue is string)
                    {
                        //targetValue = methodOfConverting.Invoke(settings, new object[] { targetValue.ToString() });
                        //prop.SetValue(command, targetValue);
                        prop.SetValue(command, convertMethod(targetValue.ToString()));
                    }
                    else if (targetValue is System.Data.DataTable table)
                    {
                        table.AcceptChanges();
                        var trgDT = table.Copy();
                        var rows = trgDT.Rows.Count;
                        var cols = trgDT.Columns.Count;
                        for (int i = 0; i < cols; i++)
                        {
                            if (trgDT.Columns[i].ReadOnly)
                            {
                                continue;
                            }
                            for (int j = 0; j < rows; j++)
                            {
                                //var v = methodOfConverting.Invoke(settings, new object[] { trgDT.Rows[j][i].ToString() });
                                //trgDT.Rows[j][i] = v;
                                trgDT.Rows[j][i] = convertMethod(trgDT.Rows[j][i]?.ToString() ?? "");
                            }
                        }
                        prop.SetValue(command, trgDT);
                    }
                }
            }
        }

    }
}
