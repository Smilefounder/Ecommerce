using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Search
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class FieldAttribute : System.Attribute
    {
        public FieldAttribute(int order, string fieldName = null, FieldStoreMode storeMode = FieldStoreMode.YES, FieldIndexMode indexMode = FieldIndexMode.ANALYZED, float boost = 1.0f, string analyzer = "standard", bool isKey = false)
        {
            FieldName = fieldName;
            Order = order;
            StoreMode = storeMode;
            IndexMode = indexMode;
            Boost = boost;
            IsKey = isKey;
        }

        public string FieldName { get; set; }
        public bool IsKey { get; set; }
        public FieldStoreMode StoreMode { get; set; }
        public FieldIndexMode IndexMode { get; set; }
        public float Boost { get; set; }
        public string Analyzer { get; set; }
        public int Order { get; set; }
    }
}
