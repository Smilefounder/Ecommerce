using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.EAV
{
    /// <summary>
    /// custom file type
    /// </summary>
    public enum CustomFieldType
    {
        /// <summary>
        /// custom field
        /// </summary>
        Custom = 0,
        /// <summary>
        /// system field, which may not allow user to delete
        /// </summary>
        System = 1,
    }
}
