using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Mapping
{
    public interface IObjectMapper
    {
        object Map(object source, object target, Type sourceType, Type targetType, MappingContext context);
    }
}
