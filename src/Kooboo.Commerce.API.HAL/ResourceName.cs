using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Kooboo.Commerce.API.HAL
{
    /// <summary>
    /// Represents the resource name.
    /// </summary>
    public class ResourceName : IEquatable<ResourceName>
    {
        static readonly Regex _pattern = new Regex(@"((?<category>[\w_]+):)?(?<name>[\w_]+)", RegexOptions.Compiled);

        public string Category { get; private set; }

        public string Name { get; private set; }

        public string FullName
        {
            get
            {
                if (String.IsNullOrEmpty(Category))
                {
                    return Name;
                }

                return Category + ":" + Name;
            }
        }

        public ResourceName(string fullName)
        {
            Require.NotNullOrEmpty(fullName, "fullName");

            var match = _pattern.Match(fullName);
            if (!match.Success)
                throw new ArgumentException("Invalid resource name " + fullName + ".");

            var categoryGroup = match.Groups["category"];
            if (categoryGroup != null)
            {
                Category = categoryGroup.Value;
            }

            Name = match.Groups["name"].Value;
        }

        public bool Equals(ResourceName other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (this == (obj as ResourceName))
            {
                return true;
            }

            var otherAsString = obj as string;

            if (String.IsNullOrEmpty(otherAsString))
            {
                return false;
            }

            return this == otherAsString;
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

        public override string ToString()
        {
            return FullName;
        }
        
        public static bool operator ==(ResourceName a, ResourceName b)
        {
            if (Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if ((object)a == null || (object)b == null)
            {
                return false;
            }

            return a.FullName == b.FullName;
        }

        public static bool operator !=(ResourceName a, ResourceName b)
        {
            return !(a == b);
        }

        public static implicit operator string(ResourceName name)
        {
            return name.FullName;
        }

        public static implicit operator ResourceName(string fullName)
        {
            if (String.IsNullOrEmpty(fullName))
            {
                return null;
            }

            return new ResourceName(fullName);
        }
    }
}