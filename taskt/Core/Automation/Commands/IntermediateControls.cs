using System;
using System.Collections.Generic;
using System.Reflection;
using static taskt.Core.Automation.Commands.PropertyControls;

namespace taskt.Core.Automation.Commands
{
    internal static class IntermediateControls
    {
        public static void ConvertToIntermediate(ScriptCommand command, EngineSettings settings, List<Script.ScriptVariable> variables)
        {
            var props = PropertyControls.GetParameterProperties(command);
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
                    else if (targetValue is System.Data.DataTable)
                    {
                        ((System.Data.DataTable)targetValue).AcceptChanges();
                        var trgDT = ((System.Data.DataTable)targetValue).Copy();
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
                                var v = settings.convertToIntermediate(trgDT.Rows[j][i].ToString());
                                trgDT.Rows[j][i] = v;
                            }
                        }
                        prop.SetValue(command, trgDT);
                    }
                }
            }
        }

        public static void convertToIntermediate(EngineSettings settings, Dictionary<string, string> convertMethods, List<Script.ScriptVariable> variables)
        {
            Type settingsType = settings.GetType();
            var myPropaties = this.GetType().GetProperties();
            foreach (var prop in myPropaties)
            {
                if (prop.Name.StartsWith("v_"))
                {
                    var targetValue = prop.GetValue(this);
                    if (targetValue != null)
                    {
                        MethodInfo methodOfConverting = null;
                        foreach (var meth in convertMethods)
                        {
                            if (meth.Key == prop.Name)
                            {
                                switch (meth.Value)
                                {
                                    case "convertToIntermediateVariableParser":
                                        methodOfConverting = settingsType.GetMethod(meth.Value, new Type[] { typeof(string), typeof(List<Script.ScriptVariable>) });
                                        break;
                                    default:
                                        methodOfConverting = settingsType.GetMethod(meth.Value, new Type[] { typeof(string) });
                                        break;
                                }
                                break;
                            }
                        }
                        if (methodOfConverting == null)
                        {
                            methodOfConverting = settingsType.GetMethod("convertToIntermediate", new Type[] { typeof(string) });
                        }

                        if (targetValue is string)
                        {
                            switch (methodOfConverting.Name)
                            {
                                case "convertToIntermediateVariableParser":
                                    targetValue = methodOfConverting.Invoke(settings, new object[] { targetValue.ToString(), variables });
                                    break;
                                default:
                                    targetValue = methodOfConverting.Invoke(settings, new object[] { targetValue.ToString() });
                                    break;
                            }
                            prop.SetValue(this, targetValue);
                        }
                        else if (targetValue is System.Data.DataTable)
                        {
                            ((System.Data.DataTable)targetValue).AcceptChanges();
                            var trgDT = ((System.Data.DataTable)targetValue).Copy();
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
                                    //var v = methodOfConverting.Invoke(settings, new object[] { trgDT.Rows[j][i].ToString(), variables });
                                    //string v = settings.convertToIntermediate(trgDT.Rows[j][i].ToString());
                                    object newCellValue;
                                    switch (methodOfConverting.Name)
                                    {
                                        case "convertToIntermediateVariableParser":
                                            newCellValue = methodOfConverting.Invoke(settings, new object[] { trgDT.Rows[j][i].ToString(), variables });
                                            break;
                                        default:
                                            newCellValue = methodOfConverting.Invoke(settings, new object[] { trgDT.Rows[j][i].ToString() });
                                            break;
                                    }
                                    trgDT.Rows[j][i] = newCellValue;
                                }
                            }
                            prop.SetValue(this, trgDT);
                        }
                    }
                }
            }
        }

        public static void convertToRaw(EngineSettings settings)
        {
            var myPropaties = this.GetType().GetProperties();
            foreach (var prop in myPropaties)
            {
                if (prop.Name.StartsWith("v_"))
                {
                    var targetValue = prop.GetValue(this);
                    if (targetValue != null)
                    {
                        if (targetValue is string)
                        {
                            targetValue = settings.convertToRaw(targetValue.ToString());
                            prop.SetValue(this, targetValue);
                        }
                        else if (targetValue is System.Data.DataTable)
                        {
                            var trgDT = (System.Data.DataTable)targetValue;
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
                                    var v = settings.convertToRaw(trgDT.Rows[j][i].ToString());
                                    trgDT.Rows[j][i] = v;
                                }
                            }
                            prop.SetValue(this, trgDT);
                        }
                    }
                }
            }
        }

        public static void convertToRaw(EngineSettings settings, Dictionary<string, string> convertMethods)
        {
            Type settingsType = settings.GetType();
            var myPropaties = this.GetType().GetProperties();
            foreach (var prop in myPropaties)
            {
                if (prop.Name.StartsWith("v_"))
                {
                    var targetValue = prop.GetValue(this);
                    if (targetValue != null)
                    {
                        MethodInfo methodOfConverting = null;
                        foreach (var meth in convertMethods)
                        {
                            if (meth.Key == prop.Name)
                            {
                                methodOfConverting = settingsType.GetMethod(meth.Value);
                                break;
                            }
                        }
                        if (methodOfConverting == null)
                        {
                            methodOfConverting = settingsType.GetMethod("convertToRaw");
                        }

                        if (targetValue is string)
                        {
                            targetValue = methodOfConverting.Invoke(settings, new object[] { targetValue.ToString() });
                            prop.SetValue(this, targetValue);
                        }
                        else if (targetValue is System.Data.DataTable)
                        {
                            ((System.Data.DataTable)targetValue).AcceptChanges();
                            var trgDT = ((System.Data.DataTable)targetValue).Copy();
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
                                    var v = methodOfConverting.Invoke(settings, new object[] { trgDT.Rows[j][i].ToString() });
                                    trgDT.Rows[j][i] = v;
                                }
                            }
                            prop.SetValue(this, trgDT);
                        }
                    }
                }
            }
        }

    }
}
