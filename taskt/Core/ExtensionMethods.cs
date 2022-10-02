using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using taskt.Core.Automation;
namespace taskt.Core
{
    public static class ExtensionMethods
    {
        private static List<char> autoCalucationSkipChars = new List<char>()
        {
            '+', '-', '*', '/', '=',
            '\n', '\r', '\t'
        };

        public static bool ConvertToUserVariableAsBool(this string str, string parameterName, object sender)
        {
            string convertedText = str.ConvertToUserVariable(sender);
            bool v;
            if (bool.TryParse(convertedText, out v))
            {
                return v;
            }
            else
            {
                throw new Exception(parameterName + " '" + str + "' is not a boolean.");
            }
        }

        public static DateTime ConvertToUserVariableAsDateTime(this string str, string parameterName, object sender)
        {
            string convertedText = str.ConvertToUserVariable(sender);
            DateTime v;
            if (DateTime.TryParse(convertedText, out v))
            {
                return v;
            }
            else
            {
                throw new Exception(parameterName + " '" + str + "' is not a DateTime.");
            }
        }

        /// <summary>
        /// Replaces variable placeholders ([variable]) with variable text.
        /// </summary>
        /// <param name="sender">The script engine instance (frmScriptEngine) which contains session variables.</param>
        public static string ConvertToUserVariable(this String str, object sender)
        {
            if (((Core.Automation.Engine.AutomationEngineInstance)sender).engineSettings.UseNewParser)
            {
                return str.ConvertToUserVariable_Unofficial(sender);
            }
            else
            {
                return str.ConvertToUserVariable_Official(sender);
            }
        }
        public static string ConvertToUserVariable_Unofficial(this String str, object sender)
        {
            if (str == null)
            {
                return string.Empty;
            }
                
            if (sender == null)
            {
                return str;
            }

            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var variableList = engine.VariableList;
            var systemVariables = Core.Common.GenerateSystemVariables();

            var searchList = new List<Core.Script.ScriptVariable>();
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

            //if ((searchVariableCount == 0) && (convertedStr == "") && (str != convertedStr))
            //{
            //    // not converted variable, result is empty string, but 'str' is not equals result
            //}

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
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="variables"></param>
        /// <param name="startMarker"></param>
        /// <param name="endMarker"></param>
        /// <returns>ret[0]: expands variable value, ret[1]: left string</returns>
        private static string[] SearchVariable(string str, List<Core.Script.ScriptVariable> variables, Core.Automation.Engine.AutomationEngineInstance engine)
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

        private static bool ExpandVariable(string variableName, List<Core.Script.ScriptVariable> variables, Core.Automation.Engine.AutomationEngineInstance engine, out string result)
        {
            variableName = variableName.Trim();
            result = null;
            if (isExpandJSON(variableName, engine)) // =>
            {
                bool ret = ExpandVariableJSON(variableName, variables, engine, out result);
                if (ret)
                {
                    return true;
                }
            }
            if (isExpandListIndex(variableName, engine))    // var[index]
            {
                bool ret = ExpandVariableListIndex(variableName, variables, engine, out result);
                if (ret)
                {
                    return true;
                }
            }
            if (isExpandDotProperty(variableName, engine))  // var.prop
            {
                bool ret = ExpandVariableDotProperty(variableName, variables, engine, out result);
                if (ret)
                {
                    return true;
                }
            }
            return ExpandVariableNormal(variableName, variables, out result);
        }
        private static bool ExpandVariableNormal(string variableName, List<Core.Script.ScriptVariable> variables, out string result)
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
        private static bool ExpandVariableScriptVariable(string variableName, List<Core.Script.ScriptVariable> variables, out Core.Script.ScriptVariable result)
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

        private static bool isExpandJSON(string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            // TODO: '=>' is engine settings
            return (variableName.Split(new string[] { "=>" }, StringSplitOptions.None).Length >= 2);
        }

        public static bool ExpandVariableJSON(string variableName, List<Core.Script.ScriptVariable> variables, Core.Automation.Engine.AutomationEngineInstance engine, out string result)
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

        private static bool isExpandListIndex(string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            // TODO: [ ] is engine settings
            return (variableName.Contains('[') && variableName.EndsWith("]"));
        }

        private static bool ExpandVariableListIndex(string variableName, List<Core.Script.ScriptVariable> variables, Core.Automation.Engine.AutomationEngineInstance engine, out string result)
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

            Core.Script.ScriptVariable targetVariable;
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

        private static bool isExpandDotProperty(string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            // TODO: . is engine settings
            return (variableName.Split('.').Length == 2);
        }

        private static bool ExpandVariableDotProperty(string variableName, List<Core.Script.ScriptVariable> variables, Core.Automation.Engine.AutomationEngineInstance engine, out string result)
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
                    break;

                case "File.CurrentScriptFile":
                    result = engine.FileName;
                    return true;
                    break;

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

            Core.Script.ScriptVariable targetVariable;
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

        public static string ConvertToUserVariable_Intermediate(this String str, Core.Automation.Engine.AutomationEngineInstance engine)
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
            var systemVariables = Core.Common.GenerateSystemVariables();

            var searchList = new List<Core.Script.ScriptVariable>();
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

        private static string[] SearchVariable_Intermediate(string str, List<Core.Script.ScriptVariable> variables, Core.Automation.Engine.AutomationEngineInstance engine)
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
                                    ret = engine.engineSettings.wrapIntermediateVariableMaker(variableName);
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
                                ret = engine.engineSettings.wrapIntermediateVariableMaker(variableName);
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
        public static string ConvertToUserVariable_Official(this String str, object sender)
        {
            if (str == null)
                return string.Empty;

            if (sender == null)
                return str;

            if (str.Length < 2)
            {
                return str;
            }


            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var variableList = engine.VariableList;
            var systemVariables = Core.Common.GenerateSystemVariables();

            var searchList = new List<Core.Script.ScriptVariable>();
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

                    case "File.CurrentScriptFile":
                        varCheck.VariableValue = engine.FileName;
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
                var mathChars = new List<Char>();
                mathChars.Add('*');
                mathChars.Add('+');
                mathChars.Add('-');
                mathChars.Add('=');
                mathChars.Add('/');
                mathChars.Add('\r');
                mathChars.Add('\n');
                mathChars.Add('\t');

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
        public static void StoreInUserVariable(this String str, object sender, string targetVariable)
        {
            //Core.Automation.Commands.VariableCommand newVariableCommand = new Core.Automation.Commands.VariableCommand
            //{
            //    v_userVariableName = targetVariable,
            //    v_Input = str
            //};
            //newVariableCommand.RunCommand(sender);
            StoreInUserVariable(targetVariable, str, (Core.Automation.Engine.AutomationEngineInstance)sender, true);
        }

        /// <summary>
        /// Stores value of the string to a user-defined variable without attempting to convert variables
        /// </summary>
        /// <param name="sender">The script engine instance (frmScriptEngine) which contains session variables.</param>
        /// <param name="targetVariable">the name of the user-defined variable to override with new value</param>
        public static void StoreRawDataInUserVariable(this String str, object sender, string targetVariable)
        {
            //Core.Automation.Commands.VariableCommand newVariableCommand = new Core.Automation.Commands.VariableCommand
            //{
            //    v_userVariableName = targetVariable,
            //    v_Input = str,
            //    v_ReplaceInputVariables = "No"            
            //};
            //newVariableCommand.RunCommand(sender);
            StoreInUserVariable(targetVariable, str, (Core.Automation.Engine.AutomationEngineInstance)sender, false);
        }

        public static void StoreInUserVariable(this bool value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            StoreInUserVariable(targetVariable, value ? "TRUE" : "FALSE", sender, false);
        }

        public static void StoreInUserVariable<Type>(this List<Type> value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            StoreInUserVariable(targetVariable, value, sender, false);
        }

        public static void StoreInUserVariable(this Dictionary<string, string> value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            StoreInUserVariable(targetVariable, value, sender, false);
        }

        public static void StoreInUserVariable(this DataTable value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            StoreInUserVariable(targetVariable, value, sender, false);
        }

        public static void StoreInUserVariable(this DataRow value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            StoreInUserVariable(targetVariable, value, sender, false);
        }
        public static void StoreInUserVariable(this DateTime value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            StoreInUserVariable(targetVariable, value, sender, false);
        }
        public static void StoreInUserVariable(this System.Drawing.Color value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            StoreInUserVariable(targetVariable, value, sender, false);
        }

        public static void StoreInUserVariable(this System.Windows.Automation.AutomationElement value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            StoreInUserVariable(targetVariable, value, sender, false);
        }

        public static void StoreInUserVariable(this List<MimeKit.MimeMessage> value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            StoreInUserVariable(targetVariable, value, sender, false);
        }

        public static void StoreInUserVariable(this MimeKit.MimeMessage value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            StoreInUserVariable(targetVariable, value, sender, false);
        }

        /// <summary>
        /// Formats item as a variable (enclosing brackets)s
        /// </summary>
        /// <param name="str">The string to be wrapped as a variable</param>
        public static string ApplyVariableFormatting(this String str, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            //var settings = new ApplicationSettings().GetOrCreateApplicationSettings();

            //return str.Insert(0, settings.EngineSettings.VariableStartMarker).Insert(str.Length + 1, settings.EngineSettings.VariableEndMarker);
            return engine.engineSettings.wrapVariableMarker(str);
        }

        private static void StoreInUserVariable(string userVariable, object variableValue, Core.Automation.Engine.AutomationEngineInstance engine, bool parseValue = true)
        {
            userVariable = parseVariableName(userVariable, engine);

            var requiredVariable = lookupVariable(userVariable, engine);

            //if still not found and user has elected option, create variable at runtime
            if ((requiredVariable == null) && (engine.engineSettings.CreateMissingVariablesDuringExecution))
            {
                engine.VariableList.Add(new Script.ScriptVariable() { VariableName = userVariable });
                requiredVariable = lookupVariable(userVariable, engine);
            }

            if (requiredVariable == null)
            {
                throw new Exception("Variable Name " + userVariable + " does not exits.");
            }
            else
            {
                if (parseValue && (variableValue is string))
                {
                    requiredVariable.VariableValue = ((string)variableValue).ConvertToUserVariable(engine);
                }
                else
                {
                    requiredVariable.VariableValue = null;
                    requiredVariable.VariableValue = variableValue;
                }
            }
        }

        public static Script.ScriptVariable GetRawVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            string newName = parseVariableName(variableName, engine);
            newName = newName.ConvertToUserVariable(engine);
            Script.ScriptVariable searchedVaiable = lookupVariable(newName, engine);
            if (searchedVaiable == null)
            {
                throw new Exception("Variable " + variableName + " does not exists.");
            }
            else
            {
                return searchedVaiable;
            }
        }

        public static List<string> GetListVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is List<string>)
            {
                return (List<string>)v.VariableValue;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not supported List");
            }
        }

        public static Dictionary<string, string> GetDictionaryVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is Dictionary<string, string>)
            {
                return (Dictionary<string, string>)v.VariableValue;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not Dictionary");
            }
        }

        public static DataTable GetDataTableVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is DataTable)
            {
                return (DataTable)v.VariableValue;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not DataTable");
            }
        }
        public static DateTime GetDateTimeVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is DateTime)
            {
                return (DateTime)v.VariableValue;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not DateTime");
            }
        }

        public static System.Drawing.Color GetColorVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is System.Drawing.Color)
            {
                return (System.Drawing.Color)v.VariableValue;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not Color");
            }
        }

        public static System.Windows.Automation.AutomationElement GetAutomationElementVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is System.Windows.Automation.AutomationElement)
            {
                return (System.Windows.Automation.AutomationElement)v.VariableValue;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not AutomationElement");
            }
        }

        public static List<MimeKit.MimeMessage> GetMailKitMailListVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is List<MimeKit.MimeMessage>)
            {
                return (List<MimeKit.MimeMessage>)v.VariableValue;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not MailKit MailList");
            }
        }

        public static MimeKit.MimeMessage GetMailKitMailVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is MimeKit.MimeMessage)
            {
                return (MimeKit.MimeMessage)v.VariableValue;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not MailKit Mail");
            }
        }

        public static Script.ScriptVariable GetInnerVariable(int index, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            return GetInnerVariableName(index, engine).GetRawVariable(engine);
        }

        public static void SetInnerVariable(object value, int index, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = GetInnerVariableName(index, engine).GetRawVariable(engine);
            v.VariableValue = value;
        }

        public static string GetInnerVariableName(int index, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            return engine.engineSettings.wrapVariableMarker("__INNER_" + index.ToString());
        }

        private static Script.ScriptVariable lookupVariable(string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            //search for the variable
            var requiredVariable = engine.VariableList.Where(var => var.VariableName == variableName).FirstOrDefault();

            return requiredVariable;
        }

        public static string parseVariableName(string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            var settings = engine.engineSettings;
            if (variableName.StartsWith(settings.VariableStartMarker) && variableName.EndsWith(settings.VariableEndMarker))
            {
                if (engine.engineSettings.IgnoreFirstVariableMarkerInOutputParameter)
                {
                    variableName = variableName.Substring(settings.VariableStartMarker.Length, variableName.Length - settings.VariableStartMarker.Length - settings.VariableEndMarker.Length);
                }
            }
            if (variableName.Contains(settings.VariableStartMarker) && variableName.Contains(settings.VariableEndMarker))
            {
                variableName = variableName.ConvertToUserVariable(engine);
            }

            return variableName;
        }

        public static string ToBase64(this string text)
        {
            return ToBase64(text, Encoding.UTF8);
        }

        public static string ToBase64(this string text, Encoding encoding)
        {
            byte[] textAsBytes = encoding.GetBytes(text);
            return Convert.ToBase64String(textAsBytes);
        }

        public static bool TryParseBase64(this string text, out string decodedText)
        {
            return TryParseBase64(text, Encoding.UTF8, out decodedText);
        }

        public static bool TryParseBase64(this string text, Encoding encoding, out string decodedText)
        {
            if (string.IsNullOrEmpty(text))
            {
                decodedText = text;
                return false;
            }

            try
            {
                byte[] textAsBytes = Convert.FromBase64String(text);
                decodedText = encoding.GetString(textAsBytes);
                return true;
            }
            catch (Exception)
            {
                decodedText = null;
                return false;
            }
        }

        public static string GetUISelectionValue(this string text, string propertyName, Core.Automation.Commands.ScriptCommand command, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            var prop = command.GetType().GetProperty(propertyName);
            var propIsOpt = (Core.Automation.Attributes.PropertyAttributes.PropertyIsOptional)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyIsOptional));

            string convText = (propIsOpt == null) ? "" : propIsOpt.setBlankToValue;
            if (!String.IsNullOrEmpty(text))
            {
                convText = text.ConvertToUserVariable(engine);
            }

            var options = (Core.Automation.Attributes.PropertyAttributes.PropertyUISelectionOption[])prop.GetCustomAttributes(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyUISelectionOption));
            if (options.Length > 0)
            {
                var propCaseSensitive = (Core.Automation.Attributes.PropertyAttributes.PropertyValueSensitive)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyValueSensitive));
                bool isCaseSensitive = (propCaseSensitive == null) ? false : propCaseSensitive.caseSensitive;

                if (isCaseSensitive)
                {
                    foreach (var opt in options)
                    {
                        if (convText == opt.uiOption)
                        {
                            return convText;
                        }
                    }
                }
                else
                {
                    convText = convText.ToLower();
                    foreach(var opt in options)
                    {
                        if (convText == opt.uiOption.ToLower())
                        {
                            return convText;
                        }
                    }
                }

                var desc = (Core.Automation.Attributes.PropertyAttributes.PropertyDescription)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyDescription));
                throw new Exception("Parameter '" + desc.propertyDescription + "' has strange value '" + text + "'");
            }
            else
            {
                return convText;
            }
        }
    }
}

