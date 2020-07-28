using System;

namespace taskt.Core.Attributes.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InputSpecification : Attribute
    {
        public string Specification { get; private set; }
        public InputSpecification(string specification)
        {
            Specification = specification;
        }
    }
}
