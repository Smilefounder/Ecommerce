using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search
{
    public class FieldAttribute : Attribute
    {
        public Field.Store Store { get; set; }

        public Field.Index Index { get; set; }

        public bool Numeric { get; set; }

        public FieldAttribute(Field.Index index)
            : this(index, Field.Store.YES)
        {
        }

        public FieldAttribute(Field.Index index, Field.Store store)
        {
            Store = store;
            Index = index;
        }

        public IFieldable CreateLuceneField(string fieldName, object fieldValue)
        {
            if (!Numeric)
            {
                return new Field(fieldName, IndexUtil.ToFieldStringValue(fieldValue), Store, Index);
            }

            var field = new NumericField(fieldName, Store, Index != Field.Index.NO);

            if (fieldValue is Int32)
            {
                field.SetIntValue((int)fieldValue);
            }
            else if (fieldValue is Int64)
            {
                field.SetLongValue((long)fieldValue);
            }
            else if (fieldValue is Single)
            {
                field.SetFloatValue((float)fieldValue);
            }
            else if (fieldValue is Double)
            {
                field.SetDoubleValue((double)fieldValue);
            }
            else if (fieldValue is Decimal)
            {
                field.SetDoubleValue((double)(decimal)fieldValue);
            }
            else
            {
                throw new NotSupportedException();
            }

            return field;
        }
    }
}