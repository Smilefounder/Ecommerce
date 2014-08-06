using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search
{
    public class IndexKey
    {
        public string Instance { get; private set; }

        public Type DocumentType { get; private set; }

        public CultureInfo Culture { get; private set; }

        public IndexKey(string instance, Type documentType, CultureInfo culture)
        {
            Instance = instance;
            Culture = culture;
            DocumentType = documentType;
        }

        public override bool Equals(object obj)
        {
            var other = obj as IndexKey;
            return other != null
                && other.Instance.Equals(Instance, StringComparison.OrdinalIgnoreCase)
                && other.Culture.Equals(Culture)
                && other.DocumentType.Equals(DocumentType);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = Instance.ToLowerInvariant().GetHashCode();
                hash = hash * 397 ^ Culture.GetHashCode();
                hash = hash * 397 ^ DocumentType.GetHashCode();
                return hash;
            }
        }
    }
}