using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search
{
    public class IndexKey : IEquatable<IndexKey>
    {
        public string Instance { get; private set; }

        public Type ModelType { get; private set; }

        public CultureInfo Culture { get; private set; }

        public IndexKey(string instance, Type modelType, CultureInfo culture)
        {
            Require.NotNullOrEmpty(instance, "instance");
            Require.NotNull(modelType, "modelType");
            Require.NotNull(culture, "culture");

            Instance = instance;
            Culture = culture;
            ModelType = modelType;
        }
        public bool Equals(IndexKey other)
        {
            return other != null
                && other.Instance.Equals(Instance, StringComparison.OrdinalIgnoreCase)
                && other.Culture.Equals(Culture)
                && other.ModelType.Equals(ModelType);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IndexKey);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = Instance.ToLowerInvariant().GetHashCode();
                hash = hash * 397 ^ Culture.GetHashCode();
                hash = hash * 397 ^ ModelType.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return Instance + "/" + ModelType.Name + "/" + (String.IsNullOrEmpty(Culture.Name) ? "Original" : Culture.Name);
        }
    }
}