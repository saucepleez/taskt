using System.Collections.Generic;

namespace taskt.Core.Script
{
    public class ScriptObject
    {
        public List<ScriptVariable> ScriptVariables { get; set; }
        public List<ScriptElement> ScriptElements { get; set; }

        public ScriptObject()
        {

        }

        public ScriptObject(List<ScriptVariable> scriptVariables, List<ScriptElement> scriptElements)
        {
            ScriptVariables = scriptVariables;
            ScriptElements = scriptElements;
        }
    }
}
