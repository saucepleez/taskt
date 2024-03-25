﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using taskt.Core.Automation.Commands;

namespace taskt.Core
{
    public class PropertyConvertTag
    {
        public string Name
        {
            get;
        }
        public string Value
        {
            get;
            private set;
        }
        public string Description
        {
            get;
            private set;
        }
        public bool HasName
        {
            get;
        }

        public PropertyConvertTag(string value, string description)
        {
            this.Value = value;
            this.Description = description;
            this.Name = "";
            this.HasName = false;
        }
        public PropertyConvertTag(string value, string name, string description)
        {
            this.Value = value;
            this.Name = name;
            this.Description = description;

            this.HasName = !(string.IsNullOrEmpty(this.Name));
        }
        public void SetNewValue(string value)
        {
            this.Value = value;
        }

        public void SetNewDescription(string desc)
        {
            this.Description = desc;
        }
    }

    public static class ExtensionMethods
    {
        private static List<char> autoCalucationSkipChars = new List<char>()
        {
            '+', '-', '*', '/', '=',
            '\n', '\r', '\t'
        };

        /// <summary>
        /// get raw property value as string
        /// </summary>
        /// <param name="command"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyDescription"></param>
        /// <returns></returns>
        /// <exception cref="Exception">value is not string</exception>
        public static string GetRawPropertyValueAsString(this ScriptCommand command, string propertyName, string propertyDescription)
        {
            var propInfo = command.GetType().GetProperty(propertyName) ?? throw new Exception(propertyDescription + " (name: '" + propertyName + "') does not exists.");
            object propValue = propInfo.GetValue(command);

            if (propValue is DataTable)
            {
                throw new Exception(propertyName + " is DataTable.");
            }
            else
            {
                return propValue?.ToString() ?? "";
            }
        }

        /// <summary>
        /// expand value or user variable as string
        /// </summary>
        /// <param name="command"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyDescription"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string ExpandValueOrUserVariable(this ScriptCommand command, string propertyName, string propertyDescription, Automation.Engine.AutomationEngineInstance engine)
        {
            return GetRawPropertyValueAsString(command, propertyName, propertyDescription).ExpandValueOrUserVariable(engine);
        }

        /// <summary>
        /// expand value or user variable as string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static string ExpandValueOrUserVariable(this string str, Automation.Engine.AutomationEngineInstance engine)
        {
            if (engine.engineSettings.UseNewParser)
            {
                return str.ExpandValueOrUserVariable_new2022(engine);
            }
            else
            {
                return str.ExpandValueOrUserVariable_Official(engine);
            }
        }

        /// <summary>
        /// new expand method 2022
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static string ExpandValueOrUserVariable_new2022(this string str, Automation.Engine.AutomationEngineInstance engine)
        {
            if (str == null)
            {
                return string.Empty;
            }
                
            if (engine == null)
            {
                return str;
            }

            var variableList = engine.VariableList;
            //var systemVariables = Core.Common.GenerateSystemVariables();
            var systemVariables = Automation.Engine.SystemVariables.GetSystemVariables(engine);

            var searchList = new List<Script.ScriptVariable>();
            searchList.AddRange(variableList);
            searchList.AddRange(systemVariables);

            //custom variable markers
            var startVariableMarker = engine.engineSettings.VariableStartMarker;

            int searchVariableCount = 0;
            string convertedStr = "";
            while (str.Length > 0)
            {
                int startIndex = str.IndexOf(startVariableMarker);
                if (startIndex >= 0)
                {
                    convertedStr += str.Substring(0, startIndex);
                    str = str.Substring(startIndex + startVariableMarker.Length);
                    string[] varResult = SearchVariable(str, searchList, engine);
                    convertedStr += varResult[0];
                    str = varResult[1];

                    searchVariableCount++;
                }
                else
                {
                    convertedStr += str;
                    str = "";
                }
            }

            if (!engine.AutoCalculateVariables)
            {
                return convertedStr;
            }
            else
            {
                return AutoCalucationVariable(convertedStr);
            }
        }

        private static string AutoCalucationVariable(string targetString)
        {
            //if the string matches the char then return
            //as the user does not want to do math
            if (autoCalucationSkipChars.Any(f => f.ToString() == targetString) || (autoCalucationSkipChars.Any(f => targetString.StartsWith(f.ToString()))))
            {
                return targetString;
            }

            //test if math is required
            try
            {
                DataTable dt = new DataTable();
                var v = dt.Compute(targetString, "");
                return v.ToString();
            }
            catch (Exception)
            {
                return targetString;
            }
        }

        /// <summary>
        /// search user variable
        /// </summary>
        /// <param name="str"></param>
        /// <param name="variables"></param>
        /// <param name="startMarker"></param>
        /// <param name="endMarker"></param>
        /// <returns>ret[0]: expands variable value, ret[1]: left string</returns>
        private static string[] SearchVariable(string str, List<Script.ScriptVariable> variables, Automation.Engine.AutomationEngineInstance engine)
        {
            int state = 1;
            string variableName = "";
            string ret = "";

            string startMarker = engine.engineSettings.VariableStartMarker;
            string endMarker = engine.engineSettings.VariableEndMarker;

            while(str.Length > 0)
            {
                switch (state)
                {
                    case 1: // search end marker, nest start marker
                        int nestStartIndex = str.IndexOf(startMarker);
                        int endIndex = str.IndexOf(endMarker);
                        if (nestStartIndex >= 0 && endIndex >= 0)
                        {
                            // both found
                            if (nestStartIndex < endIndex)
                            {
                                // nest search
                                variableName += str.Substring(0, nestStartIndex);
                                str = str.Substring(nestStartIndex + startMarker.Length);
                                string[] nestResult = SearchVariable(str, variables, engine);
                                variableName += nestResult[0];
                                str = nestResult[1];
                                state = 1;
                            }
                            else
                            {
                                // end marker found
                                variableName += str.Substring(0, endIndex);
                                str = str.Substring(endIndex + endMarker.Length);
                                // expands
                                string variableValue;
                                if (ExpandVariable(variableName, variables, engine, out variableValue))
                                {
                                    ret = variableValue;
                                    state = 2;
                                }
                                else
                                {
                                    ret = startMarker + variableName + endMarker;
                                    state = 3;
                                }
                            }
                        }
                        else if (nestStartIndex < 0 && endIndex >= 0)
                        {
                            // end marker found
                            variableName += str.Substring(0, endIndex);
                            str = str.Substring(endIndex + endMarker.Length);
                            // expands
                            string variableValue;
                            if (ExpandVariable(variableName, variables, engine, out variableValue))
                            {
                                ret = variableValue;
                                state = 2;
                            }
                            else
                            {
                                ret = startMarker + variableName + endMarker;
                                state = 3;
                            }
                        }
                        else
                        {
                            // not variable
                            variableName += str;
                            str = "";
                            ret = startMarker + variableName + endMarker;
                            state = 3;
                        }
                        break;

                    case 2: // end marker found
                        break;

                    case 3: // not variable
                        break;

                    default:
                        break;
                }
                if ((state == 2) || (state == 3))
                {
                    break;
                }
            }
            return new string[] { ret, str };
        }

        /// <summary>
        /// expand user variable
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="variables"></param>
        /// <param name="engine"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static bool ExpandVariable(string variableName, List<Script.ScriptVariable> variables, Automation.Engine.AutomationEngineInstance engine, out string result)
        {
            variableName = variableName.Trim();
            result = null;
            if (IsExpandJSON(variableName, engine)) // =>
            {
                bool ret = ExpandVariableJSON(variableName, variables, engine, out result);
                if (ret)
                {
                    return true;
                }
            }
            if (IsExpandListIndex(variableName, engine))    // var[index]
            {
                bool ret = ExpandVariableListIndex(variableName, variables, engine, out result);
                if (ret)
                {
                    return true;
                }
            }
            if (IsExpandDotProperty(variableName, engine))  // var.prop
            {
                bool ret = ExpandVariableDotProperty(variableName, variables, engine, out result);
                if (ret)
                {
                    return true;
                }
            }
            return ExpandVariableNormal(variableName, variables, out result);
        }

        private static bool ExpandVariableNormal(string variableName, List<Script.ScriptVariable> variables, out string result)
        {
            foreach(var trg in variables)
            {
                if (trg.VariableName == variableName)
                {
                    result = trg.GetDisplayValue();
                    return true;
                }
            }
            result = null;
            return false;
        }

        private static bool ExpandVariableScriptVariable(string variableName, List<Script.ScriptVariable> variables, out Script.ScriptVariable result)
        {
            foreach (var trg in variables)
            {
                if (trg.VariableName == variableName)
                {
                    result = trg;
                    return true;
                }
            }
            result = null;
            return false;
        }

        private static bool IsExpandJSON(string variableName, Automation.Engine.AutomationEngineInstance engine)
        {
            // TODO: '=>' is engine settings
            return (variableName.Split(new string[] { "=>" }, StringSplitOptions.None).Length >= 2);
        }

        public static bool ExpandVariableJSON(string variableName, List<Script.ScriptVariable> variables, Automation.Engine.AutomationEngineInstance engine, out string result)
        {
            var elements = variableName.Split(new string[] { "=>" }, StringSplitOptions.None);
            var newVariableName = elements[0].Trim();
            var jsonPattern = elements[1].Trim();

            string expandsValue;
            if (ExpandVariable(newVariableName, variables, engine, out expandsValue))
            {
                //check json pattern starts with
                if (jsonPattern.StartsWith("$."))
                {
                    JToken match;
                    if (expandsValue.StartsWith("[") && expandsValue.EndsWith("]"))
                    {
                        //attempt to match array based on user defined pattern
                        JArray parsedObject = JArray.Parse(expandsValue);
                        match = parsedObject.SelectToken(jsonPattern);
                    }
                    else
                    {
                        //attempt to match object based on user defined pattern
                        JObject parsedObject = JObject.Parse(expandsValue);
                        match = parsedObject.SelectToken(jsonPattern);
                    }
                    result = match.ToString();
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }
            else
            {
                result = null;
                return false;
            }
        }

        private static bool IsExpandListIndex(string variableName, Automation.Engine.AutomationEngineInstance engine)
        {
            // TODO: [ ] is engine settings
            return (variableName.Contains('[') && variableName.EndsWith("]"));
        }

        private static bool ExpandVariableListIndex(string variableName, List<Script.ScriptVariable> variables, Automation.Engine.AutomationEngineInstance engine, out string result)
        {
            int startPos = variableName.IndexOf('[');
            string newVariableName = variableName.Substring(0, startPos).Trim();
            string variableIndex = variableName.Substring(startPos + 1, variableName.Length - startPos - 2).Trim();

            int listIndex;
            if (!int.TryParse(variableIndex, out listIndex))
            {
                result = null;
                return false;
            }

            Script.ScriptVariable targetVariable;
            if (ExpandVariableScriptVariable(newVariableName, variables, out targetVariable))
            {
                try
                {
                    int saveIndex = targetVariable.CurrentPosition;
                    targetVariable.CurrentPosition = listIndex;
                    result = targetVariable.GetDisplayValue();
                    targetVariable.CurrentPosition = saveIndex;
                    return true;
                }
                catch
                {
                    // not list
                    result = null;
                    return false;
                }
            }
            else
            {
                result = null;
                return false;
            }
        }

        private static bool IsExpandDotProperty(string variableName, Automation.Engine.AutomationEngineInstance engine)
        {
            // TODO: . is engine settings
            return (variableName.Split('.').Length == 2);
        }

        private static bool ExpandVariableDotProperty(string variableName, List<Script.ScriptVariable> variables, Automation.Engine.AutomationEngineInstance engine, out string result)
        {
            //if (variableName == "taskt.EngineContext")
            //{
            //    result = engine.GetEngineContext();
            //    return true;
            //}

            switch (variableName)
            {
                case "taskt.EngineContext":
                    result = engine.GetEngineContext();
                    return true;

                //    case "File.CurrentScriptFile":
                //        result = engine.FileName;
                //        return true;

                default:
                    break;
            }

            if (ExpandVariableNormal(variableName, variables, out result))
            {
                // System Variables
                return true;
            }

            string[] sptVar = variableName.Split('.');
            string newVariableName = sptVar[0].Trim();
            string variableProperty = sptVar[1].Trim();

            Script.ScriptVariable targetVariable;
            if (ExpandVariableScriptVariable(newVariableName, variables, out targetVariable))
            {
                try
                {
                    result = targetVariable.GetDisplayValue(variableProperty);
                    return true;
                }
                catch
                {
                    // not list
                    result = null;
                    return false;
                }
            }
            else
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// convert user variable to Intermediate Notation (change wrap marker)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string ConvertUserVariableToIntermediateNotation(this string str, Automation.Engine.AutomationEngineInstance engine)
        {
            if (str == null)
            {
                return string.Empty;
            }

            if (engine == null)
            {
                return str;
            }

            var variableList = engine.VariableList;
            //var systemVariables = Core.Common.GenerateSystemVariables();
            var systemVariables = Automation.Engine.SystemVariables.GetSystemVariables(engine);

            var searchList = new List<Script.ScriptVariable>();
            searchList.AddRange(variableList);
            searchList.AddRange(systemVariables);

            //custom variable markers
            var startVariableMarker = engine.engineSettings.VariableStartMarker;

            string convertedStr = "";
            while (str.Length > 0)
            {
                int startIndex = str.IndexOf(startVariableMarker);
                if (startIndex >= 0)
                {
                    convertedStr += str.Substring(0, startIndex);
                    str = str.Substring(startIndex + startVariableMarker.Length);
                    string[] varResult = SearchVariable_Intermediate(str, searchList, engine);
                    convertedStr += varResult[0];
                    str = varResult[1];
                }
                else
                {
                    convertedStr += str;
                    str = "";
                }
            }
            return convertedStr;
        }

        private static string[] SearchVariable_Intermediate(string str, List<Script.ScriptVariable> variables, Automation.Engine.AutomationEngineInstance engine)
        {
            int state = 1;
            string variableName = "";
            string ret = "";

            string startMarker = engine.engineSettings.VariableStartMarker;
            string endMarker = engine.engineSettings.VariableEndMarker;

            while (str.Length > 0)
            {
                switch (state)
                {
                    case 1: // search end marker, nest start marker
                        int nestStartIndex = str.IndexOf(startMarker);
                        int endIndex = str.IndexOf(endMarker);
                        if (nestStartIndex >= 0 && endIndex >= 0)
                        {
                            // both found
                            if (nestStartIndex < endIndex)
                            {
                                // nest search
                                variableName += str.Substring(0, nestStartIndex);
                                str = str.Substring(nestStartIndex + startMarker.Length);
                                string[] nestResult = SearchVariable_Intermediate(str, variables, engine);
                                variableName += nestResult[0];
                                str = nestResult[1];
                                state = 1;
                            }
                            else
                            {
                                // end marker found
                                variableName += str.Substring(0, endIndex);
                                str = str.Substring(endIndex + endMarker.Length);
                                // expands
                                string variableValue;
                                if (ExpandVariable(variableName, variables, engine, out variableValue))
                                {
                                    //ret = engine.engineSettings.wrapIntermediateVariableMaker(variableName);
                                    ret = IntermediateControls.GetWrappedIntermediateVariable(variableName);
                                    state = 2;
                                }
                                else
                                {
                                    ret = startMarker + variableName + endMarker;
                                    state = 3;
                                }
                            }
                        }
                        else if (nestStartIndex < 0 && endIndex >= 0)
                        {
                            // end marker found
                            variableName += str.Substring(0, endIndex);
                            str = str.Substring(endIndex + endMarker.Length);
                            // expands
                            string variableValue;
                            if (ExpandVariable(variableName, variables, engine, out variableValue))
                            {
                                //ret = engine.engineSettings.wrapIntermediateVariableMaker(variableName);
                                ret = IntermediateControls.GetWrappedIntermediateVariable(variableName);
                                state = 2;
                            }
                            else
                            {
                                ret = startMarker + variableName + endMarker;
                                state = 3;
                            }
                        }
                        else
                        {
                            // not variable
                            variableName += str;
                            str = "";
                            ret = startMarker + variableName + endMarker;
                            state = 3;
                        }
                        break;

                    case 2: // end marker found
                        break;

                    case 3: // not variable
                        break;

                    default:
                        break;
                }
                if ((state == 2) || (state == 3))
                {
                    break;
                }
            }
            return new string[] { ret, str };
        }

        // official parser
        public static string ExpandValueOrUserVariable_Official(this string str, Automation.Engine.AutomationEngineInstance engine)
        {
            if (str == null)
                return string.Empty;

            if (engine == null)
                return str;

            if (str.Length < 2)
            {
                return str;
            }

            var variableList = engine.VariableList;
            //var systemVariables = Core.Common.GenerateSystemVariables();
            var systemVariables = Automation.Engine.SystemVariables.GetSystemVariables(engine);

            var searchList = new List<Script.ScriptVariable>();
            searchList.AddRange(variableList);
            searchList.AddRange(systemVariables);

            //custom variable markers
            var startVariableMarker = engine.engineSettings.VariableStartMarker;
            var endVariableMarker = engine.engineSettings.VariableEndMarker;

            //split by custom markers
            string[] potentialVariables = str.Split(new string[] { startVariableMarker, endVariableMarker }, StringSplitOptions.None);

            foreach (var potentialVariable in potentialVariables)
            {

                //complex variable handling
                if (potentialVariable.Contains("=>"))
                {
                    var complexJsonVariable = potentialVariable;

                    // ^ で囲まれいたら置き換える (そんな機能知らん)
                    //detect potential variables and replace
                    string[] potentialSubVariables = complexJsonVariable.Split(new string[] { "^" }, StringSplitOptions.None);

                    foreach (var potentialSubVariable in potentialSubVariables)
                    {
                        var matchingVar = (from vars in searchList
                                           where vars.VariableName == potentialSubVariable
                                           select vars).FirstOrDefault();

                        if (matchingVar != null)
                        {
                            //get the value from the list

                            complexJsonVariable = complexJsonVariable.Replace("^" + potentialSubVariable + "^", matchingVar.GetDisplayValue());
                            continue;
                        }
                    }

                    // => がついていたらそこで JSON のセレクターを何かする
                    //split by json select token pointer
                    var element = complexJsonVariable.Split(new string[] { "=>" }, StringSplitOptions.None);

                    //verify length
                    if (element.Length >= 2)
                    {
                        //get variable name
                        var variableName = element[0].Trim();

                        //get json pattern
                        var jsonPattern = element[1].Trim();

                        //check json pattern starts with
                        if (jsonPattern.StartsWith("$."))
                        {
                            //find variable
                            var matchingVar = (from vars in searchList
                                               where vars.VariableName == variableName
                                               select vars).FirstOrDefault();
                            //if variable is found
                            if (matchingVar != null)
                            {
                                //get the value from the list
                                var complexJson = matchingVar.GetDisplayValue();

                                JToken match;
                                if (complexJson.StartsWith("[") && complexJson.EndsWith("]"))
                                {
                                    //attempt to match array based on user defined pattern
                                    JArray parsedObject = JArray.Parse(complexJson);
                                    match = parsedObject.SelectToken(jsonPattern);
                                }
                                else
                                {
                                    //attempt to match object based on user defined pattern
                                    JObject parsedObject = JObject.Parse(complexJson);
                                    match = parsedObject.SelectToken(jsonPattern);
                                }

                                //check match
                                if (match != null)
                                {
                                    //replace with value
                                    str = str.Replace(startVariableMarker + potentialVariable + endVariableMarker, match.ToString());
                                    continue;
                                }
                            }
                        }
                    }
                }

                //
                string varcheckname = potentialVariable;
                // SystemVar かチェック
                bool isSystemVar = systemVariables.Any(vars => vars.VariableName == varcheckname);
                // [ ] で分ける
                string[] aPotentialVariable = potentialVariable.Split(new string[] { "[", "]" }, StringSplitOptions.None);
                int directElementIndex = 0;
                bool useDirectElementIndex = false;
                // 3 つに分かれて、インデックスが数字だったら……
                if (aPotentialVariable.Length == 3 && int.TryParse(aPotentialVariable[1], out directElementIndex))
                {
                    varcheckname = aPotentialVariable[0];
                    useDirectElementIndex = true;   // [ ] フラグON
                }
                // . で分けたら 2 つに分かれて、SystemVar でなければ
                else if (potentialVariable.Split('.').Length == 2 && !isSystemVar)
                {
                    varcheckname = potentialVariable.Split('.')[0];
                }

                // varcheckname の変数を取得
                var varCheck = (from vars in searchList
                                where vars.VariableName == varcheckname
                                select vars).FirstOrDefault();


                if (potentialVariable.Length == 0)
                    continue;


                //if (potentialVariable == "taskt.EngineContext")
                //{
                //    varCheck.VariableValue = engine.GetEngineContext();
                //}
                switch (potentialVariable)
                {
                    case "taskt.EngineContext":
                        varCheck.VariableValue = engine.GetEngineContext();
                        break;

                    //case "File.CurrentScriptFile":
                    //    varCheck.VariableValue = engine.FileName;
                    //    break;
                    default:
                        break;
                }

                if (varCheck != null)
                {
                    var searchVariable = startVariableMarker + potentialVariable + endVariableMarker;

                    // str に検索対象の searchVariable がある
                    if (str.Contains(searchVariable))
                    {
                        if (useDirectElementIndex)
                        {
                            // [ ] を指定したリスト取得
                            int savePosition = varCheck.CurrentPosition;
                            varCheck.CurrentPosition = directElementIndex;
                            str = str.Replace(searchVariable, (string)varCheck.GetDisplayValue());
                            varCheck.CurrentPosition = savePosition;
                        }
                        else if (varCheck.VariableValue is DataTable && potentialVariable.Split('.').Length == 2)
                        {
                            // DataTable で . を含む
                            //user is trying to get data from column name or index
                            string columnName = potentialVariable.Split('.')[1];
                            var dt = varCheck.VariableValue as DataTable;

                            string cellItem;
                            if (int.TryParse(columnName, out var columnIndex))
                            {
                                cellItem = dt.Rows[varCheck.CurrentPosition].Field<object>(columnIndex).ToString();
                            }
                            else
                            {
                                cellItem = dt.Rows[varCheck.CurrentPosition].Field<object>(columnName).ToString();
                            }

                            str = str.Replace(searchVariable, cellItem);
                        }
                        else if (potentialVariable.Split('.').Length == 2) // This handles vVariable.count 
                        {
                            string propertyName = potentialVariable.Split('.')[1];
                            str = str.Replace(searchVariable, (string)varCheck.GetDisplayValue(propertyName));
                        }
                        else
                        {
                            str = str.Replace(searchVariable, (string)varCheck.GetDisplayValue());
                        }
                    }
                    else if (str.Contains(potentialVariable))
                    {
                        try
                        {
                            str = str.Replace(potentialVariable, (string)varCheck.GetDisplayValue());
                        }
                        catch (Exception)
                        {

                        }
                    }
                }

                else if ((potentialVariable.Contains("ds") && (potentialVariable.Contains("."))))
                {
                    //peform dataset check
                    var splitVariable = potentialVariable.Split('.');

                    if (splitVariable.Length == 3)
                    {
                        string dsleading = splitVariable[0];
                        string datasetName = splitVariable[1];
                        string columnRequired = splitVariable[2];

                        var datasetVariable = variableList.Where(f => f.VariableName == datasetName).FirstOrDefault();

                        if (datasetVariable == null)
                            continue;

                        DataTable dataTable = (DataTable)datasetVariable.VariableValue;

                        if (datasetVariable == null)
                            continue;

                        if ((dsleading == "ds") && (int.TryParse(columnRequired, out int columnNumber)))
                        {
                            //get by column index
                            str = (string)dataTable.Rows[datasetVariable.CurrentPosition][columnNumber];
                        }
                        else if (dsleading == "ds")
                        {
                            //get by column index
                            str = (string)dataTable.Rows[datasetVariable.CurrentPosition][columnRequired];
                        }
                    }
                }
            }

            if (!engine.AutoCalculateVariables)
            {
                return str;
            }
            else
            {
                //track math chars
                var mathChars = new List<Char>
                {
                    '*',
                    '+',
                    '-',
                    '=',
                    '/',
                    '\r',
                    '\n',
                    '\t'
                };

                //if the string matches the char then return
                //as the user does not want to do math
                if (mathChars.Any(f => f.ToString() == str) || (mathChars.Any(f => str.StartsWith(f.ToString()))))
                {
                    return str;
                }

                //bypass math for types that are dates
                DateTime dateTest;
                if ((DateTime.TryParse(str, out dateTest) && (str.Length > 6)))
                {
                    return str;
                }

                //test if math is required
                try
                {
                    DataTable dt = new DataTable();
                    var v = dt.Compute(str, "");
                    return v.ToString();
                }
                catch (Exception)
                {
                    return str;
                }
            }
        }

        /// <summary>
        /// Stores value of the string to a user-defined variable.
        /// </summary>
        /// <param name="sender">The script engine instance (frmScriptEngine) which contains session variables.</param>
        /// <param name="targetVariable">the name of the user-defined variable to override with new value</param>
        public static void StoreInUserVariable(this string str, Automation.Engine.AutomationEngineInstance engine, string targetVariable)
        {
            StoreInUserVariable(targetVariable, str, engine, true);
        }

        /// <summary>
        /// Stores value of the string to a user-defined variable without attempting to convert variables
        /// </summary>
        /// <param name="sender">The script engine instance (frmScriptEngine) which contains session variables.</param>
        /// <param name="targetVariable">the name of the user-defined variable to override with new value</param>
        public static void StoreRawDataInUserVariable(this string str, Automation.Engine.AutomationEngineInstance engine, string targetVariable)
        {
            StoreInUserVariable(targetVariable, str, engine, false);
        }

        //public static void StoreInUserVariable(this DataRow value, Automation.Engine.AutomationEngineInstance engine, string targetVariable)
        //{
        //    StoreInUserVariable(targetVariable, value, engine, false);
        //}

        /// <summary>
        /// Formats item as a variable (enclosing brackets)s
        /// </summary>
        /// <param name="str">The string to be wrapped as a variable</param>
        public static string ApplyVariableFormatting(this string str, Automation.Engine.AutomationEngineInstance engine)
        {
            //var settings = new ApplicationSettings().GetOrCreateApplicationSettings();

            //return str.Insert(0, settings.EngineSettings.VariableStartMarker).Insert(str.Length + 1, settings.EngineSettings.VariableEndMarker);
            //return engine.engineSettings.wrapVariableMarker(str);

            return VariableNameControls.GetWrappedVariableName(str, engine);
        }

        public static void StoreInUserVariable(string userVariable, object variableValue, Automation.Engine.AutomationEngineInstance engine, bool parseValue = true)
        {
            //userVariable = parseVariableName(userVariable, engine);
            userVariable = VariableNameControls.GetVariableName(userVariable, engine);

            var requiredVariable = LookupVariable(userVariable, engine);

            //if still not found and user has elected option, create variable at runtime
            if ((requiredVariable == null) && (engine.engineSettings.CreateMissingVariablesDuringExecution))
            {
                engine.VariableList.Add(new Script.ScriptVariable() { VariableName = userVariable });
                requiredVariable = LookupVariable(userVariable, engine);
            }

            if (requiredVariable == null)
            {
                throw new Exception("Variable Name " + userVariable + " does not exits.");
            }
            else
            {
                if (parseValue && (variableValue is string str))
                {
                    requiredVariable.VariableValue = str.ExpandValueOrUserVariable(engine);
                }
                else
                {
                    requiredVariable.VariableValue = null;
                    requiredVariable.VariableValue = variableValue;
                }
            }
        }

        /// <summary>
        /// get raw user variable value
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Script.ScriptVariable GetRawVariable(this string variableName, Automation.Engine.AutomationEngineInstance engine)
        {
            //string newName = parseVariableName(variableName, engine);
            var newName = VariableNameControls.GetVariableName(variableName, engine);
            if (newName != variableName)
            {
                newName = newName.ExpandValueOrUserVariable(engine);
            }
            Script.ScriptVariable searchedVaiable = LookupVariable(newName, engine);
            if (searchedVaiable == null)
            {
                throw new Exception("Variable " + variableName + " does not exists.");
            }
            else
            {
                return searchedVaiable;
            }
        }

        /// <summary>
        /// search and return variable that specified name
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        private static Script.ScriptVariable LookupVariable(string variableName, Automation.Engine.AutomationEngineInstance engine)
        {
            //search for the variable
            var requiredVariable = engine.VariableList.Where(var => var.VariableName == variableName).FirstOrDefault();

            return requiredVariable;
        }

        //public static string parseVariableName(string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        //{
        //    var settings = engine.engineSettings;
        //    if (variableName.StartsWith(settings.VariableStartMarker) && variableName.EndsWith(settings.VariableEndMarker))
        //    {
        //        if (engine.engineSettings.IgnoreFirstVariableMarkerInOutputParameter)
        //        {
        //            variableName = variableName.Substring(settings.VariableStartMarker.Length, variableName.Length - settings.VariableStartMarker.Length - settings.VariableEndMarker.Length);
        //        }
        //    }
        //    if (variableName.Contains(settings.VariableStartMarker) && variableName.Contains(settings.VariableEndMarker))
        //    {
        //        variableName = variableName.ExpandValueOrUserVariable(engine);
        //    }

        //    return variableName;
        //}

        //public static string ToBase64(this string text)
        //{
        //    return ToBase64(text, Encoding.UTF8);
        //}

        //public static string ToBase64(this string text, Encoding encoding)
        //{
        //    byte[] textAsBytes = encoding.GetBytes(text);
        //    return Convert.ToBase64String(textAsBytes);
        //}

        //public static bool TryParseBase64(this string text, out string decodedText)
        //{
        //    return TryParseBase64(text, Encoding.UTF8, out decodedText);
        //}

        //public static bool TryParseBase64(this string text, Encoding encoding, out string decodedText)
        //{
        //    if (string.IsNullOrEmpty(text))
        //    {
        //        decodedText = text;
        //        return false;
        //    }

        //    try
        //    {
        //        byte[] textAsBytes = Convert.FromBase64String(text);
        //        decodedText = encoding.GetString(textAsBytes);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        decodedText = null;
        //        return false;
        //    }
        //}
    }
}

