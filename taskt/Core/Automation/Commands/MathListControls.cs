using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core.Automation.Commands
{
    internal class MathListControls
    {
        public static List<decimal> ConvertToDecimalList(string listName, bool ignoreNotNumeric, Engine.AutomationEngineInstance engine)
        {
            var list = listName.GetListVariable(engine);

            List<decimal> numList = new List<decimal>();
            foreach(var value in list)
            {
                decimal v;
                if (decimal.TryParse(value, out v))
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
