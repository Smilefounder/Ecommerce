using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Mapping
{
    public class ObjectProperty : IEquatable<ObjectProperty>
    {
        public object Container { get; private set; }

        public PropertyInfo Property { get; private set; }

        public string PropertyName
        {
            get
            {
                return Property.Name;
            }
        }

        public Type PropertyType
        {
            get
            {
                return Property.PropertyType;
            }
        }

        public ObjectProperty(object container, PropertyInfo property)
        {
            Require.NotNull(container, "container");
            Require.NotNull(property, "property");

            Container = container;
            Property = property;
        }

        public object GetValue()
        {
            return Property.GetValue(Container, null);
        }

        public void SetValue(object value)
        {
            Property.SetValue(Container, value, null);
        }

        public bool Equals(ObjectProperty other)
        {
            return other != null && Object.ReferenceEquals(other.Container, Container) && other.Property == Property;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ObjectProperty);
        }

        public override int GetHashCode()
        {
            return (RuntimeHelpers.GetHashCode(Container) * 397) ^ Property.GetHashCode();
        }
    }
}
