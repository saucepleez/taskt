using System;

namespace taskt.Core.Script
{
    [Serializable]
    public class ScriptElement
    {
        public string ElementName { get; set; }
        public ScriptElementType ElementType { get; set; }
        public string ElementValue { get; set; }
    }
}
