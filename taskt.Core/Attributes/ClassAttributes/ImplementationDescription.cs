using System;

namespace taskt.Core.Attributes.ClassAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ImplementationDescription : Attribute
    {
        public string CommandImplementationDescription { get; private set; }
        public ImplementationDescription(string description)
        {
            CommandImplementationDescription = description;
        }
    }
}
