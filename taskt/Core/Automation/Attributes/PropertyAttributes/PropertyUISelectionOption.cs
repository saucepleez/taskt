using System;

namespace taskt.Core.Automation.Attributes.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class PropertyUISelectionOption : Attribute
    {
        public string UIOption { get; private set; }
        public PropertyUISelectionOption(string option)
        {
            UIOption = option;
        }
    }
}
