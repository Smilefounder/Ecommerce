using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Mapping
{
    public interface IObjectMapper
    {
        object Map(object source, object target, Type sourceType, Type targetType, string prefix, MappingContext context);
    }
}
