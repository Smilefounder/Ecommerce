using Kooboo.Commerce.Data;
using Lucene.Net.Documents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Rebuild
{
    /// <summary>
    /// Represents the source of data which is enumerated and indexed.
    /// </summary>
    public interface IIndexSource
    {
        int Count(CommerceInstance instance, CultureInfo culture);

        IEnumerable Enumerate(CommerceInstance instance, CultureInfo culture);
    }
}