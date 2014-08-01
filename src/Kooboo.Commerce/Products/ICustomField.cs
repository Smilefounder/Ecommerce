using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Products
{
    public interface ICustomField
    {
        string FieldName { get; }

        string FieldValue { get; }
    }
}
