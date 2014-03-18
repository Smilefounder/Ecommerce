using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Search
{
    public enum FieldStoreMode
    {
        // 摘要:
        //     Store the original field value in the index. This is useful for short texts
        //     like a document's title which should be displayed with the results. The value
        //     is stored in its original form, i.e. no analyzer is used before it is stored.
        YES = 0,
        //
        // 摘要:
        //     Do not store the field value in the index.
        NO = 1,
    }
}
