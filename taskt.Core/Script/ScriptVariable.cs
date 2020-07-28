using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace taskt.Core.Script
{
    [Serializable]
    public class ScriptVariable
    {
        /// <summary>
        /// name that will be used to identify the variable
        /// </summary>
        public string VariableName { get; set; }

        /// <summary>
        /// index/position tracking for complex variables (list)
        /// </summary>
        [JsonIgnore]
        public int CurrentPosition = 0;

        /// <summary>
        /// value of the variable or current index
        /// </summary>
        public object VariableValue { get; set; }

        /// <summary>
        /// To check if the value is a secure string
        /// </summary>
        public bool IsSecureString { get; set; }

        /// <summary>
        /// retrieve value of the variable
        /// </summary>
        public string GetDisplayValue(string requiredProperty = "")
        {

            if (VariableValue is string)
            {
                switch (requiredProperty)
                {
                    case "type":
                    case "Type":
                    case "TYPE":
                        return "BASIC";
                    default:
                        return (string)VariableValue;
                }

            }
            else if (VariableValue is DataTable)
            {
                DataTable dataTable = (DataTable)VariableValue;
                var dataRow = dataTable.Rows[CurrentPosition];
                return JsonConvert.SerializeObject(dataRow.ItemArray);
            }
            else if(VariableValue.GetType() != typeof(string))
            {
                return VariableValue.ToString();
            }
            else
            {
                List<string> requiredValue = (List<string>)VariableValue;
                switch (requiredProperty)
                {
                    case "count":
                    case "Count":
                    case "COUNT":
                        return requiredValue.Count.ToString();
                    case "index":
                    case "Index":
                    case "INDEX":
                        return CurrentPosition.ToString();
                    case "tojson":
                    case "ToJson":
                    case "toJson":
                    case "TOJSON":
                        return JsonConvert.SerializeObject(requiredValue);
                    case "topipe":
                    case "ToPipe":
                    case "toPipe":
                    case "TOPIPE":
                        return String.Join("|", requiredValue);
                    case "first":
                    case "First":
                    case "FIRST":
                        return requiredValue.FirstOrDefault();
                    case "last":
                    case "Last":
                    case "LAST":
                        return requiredValue.Last();
                    case "type":
                    case "Type":
                    case "TYPE":
                        return "LIST";
                    default:
                        return requiredValue[CurrentPosition];
                }
            }

        }
    }
}
