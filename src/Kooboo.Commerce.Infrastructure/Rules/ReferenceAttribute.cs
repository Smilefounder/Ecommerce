using System;

namespace Kooboo.Commerce.Rules
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ReferenceAttribute : Attribute
    {
        public string Prefix { get; set; }

        public Type ReferencingType { get; set; }

        public Type ReferenceResolver { get; set; }

        public ReferenceAttribute()
        {
        }

        public ReferenceAttribute(Type referencingType)
            : this(referencingType, typeof(IndirectReferenceResolver))
        {
        }

        public ReferenceAttribute(Type referencingType, Type referenceResolver)
        {
            ReferencingType = referencingType;
            ReferenceResolver = referenceResolver;
        }
    }
}
