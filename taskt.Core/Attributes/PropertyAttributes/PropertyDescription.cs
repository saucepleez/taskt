using System;

namespace taskt.Core.Attributes.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PropertyDescription : Attribute
    {
        public string Description { get; private set; }
        public PropertyDescription(string description)
        {
            Description = description;
        }
    }
}
