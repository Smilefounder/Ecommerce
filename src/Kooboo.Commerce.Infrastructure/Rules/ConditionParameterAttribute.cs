using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    /// <summary>
    /// Declare a model property to be used as a condition expression parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ConditionParameterAttribute : Attribute
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }
    }
}
