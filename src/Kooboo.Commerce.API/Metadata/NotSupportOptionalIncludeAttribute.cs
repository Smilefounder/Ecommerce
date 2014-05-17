using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Metadata
{
    /// <summary>
    /// Marks a property not supporting optional include.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NotSupportOptionalIncludeAttribute : Attribute
    {
    }
}
