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
    public interface IIndexSource
    {
        int Count(CommerceInstance instance, Type documentType, CultureInfo culture);

        IEnumerable Enumerate(CommerceInstance instance, Type documentType, CultureInfo culture);

        Document CreateDocument(object data, CommerceInstance instance, Type documentType, CultureInfo culture);
    }
}