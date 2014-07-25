using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Mapping
{
    public class ObjectReference : IEquatable<ObjectReference>
    {
        public object Object { get; private set; }

        public ObjectReference(object obj)
        {
            Require.NotNull(obj, "obj");

            Object = obj;
        }

        public bool Equals(ObjectReference other)
        {
            return other != null && Object.ReferenceEquals(other.Object, Object);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ObjectReference);
        }

        public override int GetHashCode()
        {
            return RuntimeHelpers.GetHashCode(Object);
        }
    }
}
