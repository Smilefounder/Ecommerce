using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    public interface ISerializationKnownTypesProvider
    {
        IEnumerable<Type> GetKnownTypes();
    }
}