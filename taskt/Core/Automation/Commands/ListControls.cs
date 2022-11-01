using System;
using System.Collections.Generic;

namespace taskt.Core.Automation.Commands
{
    internal static class ListControls
    {
        public static List<decimal> ConvertToDecimalList(string listName, bool ignoreNotNumeric, Engine.AutomationEngineInstance engine)
        {
            var list = listName.GetListVariable(engine);

            List<decimal> numList = new List<decimal>();
            foreach(var value in list)
            {
                if (decimal.TryParse(value, out decimal v))
                {
                    numList.Add(v);
                }
                else if (!ignoreNotNumeric)
                {
                    throw new Exception(listName + " has not numeric value.");
                }
            }

            return numList;
        }
    }
}
