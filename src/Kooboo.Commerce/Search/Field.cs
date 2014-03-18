using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Search
{
    public class Field
    {
        public Field()
        {
            IsKey = false;
            Boost = 1.0f;
            FieldType = FieldType.String;
            StoreMode = FieldStoreMode.YES;
            IndexMode = FieldIndexMode.ANALYZED;
        }

        public string FieldName { get; set; }
        public FieldType FieldType { get; set; }
        public bool IsKey { get; set; }
        public FieldStoreMode StoreMode { get; set; }
        public FieldIndexMode IndexMode { get; set; }
        public float Boost { get; set; }
        public object Value { get; set; }
        public string Analyzer { get; set; }
    }
}
