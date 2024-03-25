﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// convert raw script to intermediate or convert intermediate script to raw
    /// </summary>
    internal static class IntermediateControls
    {
        #region fields
        /// <summary>
        /// intermediate start variable marker
        /// </summary>
        public const string INTERMEDIATE_VALIABLE_START_MARKER = "\u2983";  // like {
        /// <summary>
        /// intermediate end variable marker
        /// </summary>
        public const string INTERMEDIATE_VALIABLE_END_MARKER = "\u2984";    // like }

        // TODO: To be discontinued eventually
        /// <summary>
        /// intermediate start keyword marker
        /// </summary>
        public const string INTERMEDIATE_KEYWORD_START_MARKER = "\U0001D542";   // like k
        /// <summary>
        /// intermediate end keywrod marker
        /// </summary>
        public const string INTERMEDIATE_KEYWORD_END_MARKER = "\U0001D54E"; // like w
        #endregion

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

        /// <summary>
        /// proprety value convert to intermediate. this method use specified method by argument or default convert method.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="settings"></param>
        /// <param name="convertMethods"></param>
        /// <param name="variables"></param>
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
                        switch (convertMethods[prop.Name])
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
                                trgDT.Rows[j][i] = convertMethod(trgDT.Rows[j][i]?.ToString() ?? "");
                            }
                        }
                        prop.SetValue(command, trgDT);
                    }
                }
            }
        }

        /// <summary>
        /// proprety value convert to raw value. this method use default convert method.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="settings"></param>
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

        /// <summary>
        /// proprety value convert to raw value. this method use specified methods by argument or default convert method.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="settings"></param>
        /// <param name="convertMethods"></param>
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
                                trgDT.Rows[j][i] = convertMethod(trgDT.Rows[j][i]?.ToString() ?? "");
                            }
                        }
                        prop.SetValue(command, trgDT);
                    }
                }
            }
        }

        /// <summary>
        /// check value is wrapped intermediate keyword marker
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool IsWrappedIntermediateKeywordMarker(string value)
        {
            return (value.StartsWith(INTERMEDIATE_KEYWORD_END_MARKER) && value.EndsWith(INTERMEDIATE_KEYWORD_END_MARKER));
        }

        /// <summary>
        /// get wrapped intermediate keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static string GetWrappedIntermediateKeyword(string keyword)
        {
            if (IsWrappedIntermediateKeywordMarker(keyword))
            {
                return keyword;
            }
            else
            {
                return string.Concat(INTERMEDIATE_KEYWORD_START_MARKER, keyword, INTERMEDIATE_KEYWORD_END_MARKER);
            }
        }

        /// <summary>
        /// check value is wrapped intermediate variable marker
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool IsWrappedIntermediateVariableMarker(string value)
        {
            return (value.StartsWith(INTERMEDIATE_VALIABLE_START_MARKER) && value.EndsWith(INTERMEDIATE_VALIABLE_END_MARKER));
        }

        /// <summary>
        /// get wrapped intermediate variable marker
        /// </summary>
        /// <param name="variableName"></param>
        /// <returns></returns>
        public static string GetWrappedIntermediateVariable(string variableName)
        {
            if (IsWrappedIntermediateVariableMarker(variableName))
            {
                return variableName;
            }
            else
            {
                return string.Concat(INTERMEDIATE_VALIABLE_START_MARKER, variableName, INTERMEDIATE_VALIABLE_END_MARKER);
            }
        }
    }
}
