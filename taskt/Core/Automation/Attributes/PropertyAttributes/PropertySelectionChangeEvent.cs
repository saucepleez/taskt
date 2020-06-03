using System;

namespace taskt.Core.Automation.Attributes.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class PropertySelectionChangeEvent : Attribute
    {
        public string UIOption { get; private set; }
        public PropertySelectionChangeEvent(string option)
        {
            UIOption = option;
        }
    }
}
