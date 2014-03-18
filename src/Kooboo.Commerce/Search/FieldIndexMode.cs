using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Search
{
    public enum FieldIndexMode
    {
        // 摘要:
        //     Do not index the field value. This field can thus not be searched, but one
        //     can still access its contents provided it is Lucene.Net.Documents.Field.Store.
        NO = 0,
        //
        // 摘要:
        //     Index the tokens produced by running the field's value through an Analyzer.
        //     This is useful for common text.
        ANALYZED = 1,
        //
        // 摘要:
        //     Index the field's value without using an Analyzer, so it can be searched.
        //      As no analyzer is used the value will be stored as a single term. This is
        //     useful for unique Ids like product numbers.
        NOT_ANALYZED = 2,
        //
        // 摘要:
        //     Expert: Index the field's value without an Analyzer, and also disable the
        //     storing of norms. Note that you can also separately enable/disable norms
        //     by setting Lucene.Net.Documents.AbstractField.OmitNorms. No norms means that
        //     index-time field and document boosting and field length normalization are
        //     disabled. The benefit is less memory usage as norms take up one byte of RAM
        //     per indexed field for every document in the index, during searching. Note
        //     that once you index a given field with norms enabled, disabling norms will
        //     have no effect. In other words, for this to have the above described effect
        //     on a field, all instances of that field must be indexed with NOT_ANALYZED_NO_NORMS
        //     from the beginning.
        NOT_ANALYZED_NO_NORMS = 3,
        //
        // 摘要:
        //     Expert: Index the tokens produced by running the field's value through an
        //     Analyzer, and also separately disable the storing of norms. See Lucene.Net.Documents.Field.Index.NOT_ANALYZED_NO_NORMS
        //     for what norms are and why you may want to disable them.
        ANALYZED_NO_NORMS = 4,
    }
}
