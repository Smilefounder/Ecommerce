using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Kooboo.Commerce.Data
{
    [DataContract]
    [Flags]
    public enum DataOperation
    {
        [EnumMember]
        None = 1,
        [EnumMember]
        Query = 2,
        [EnumMember]
        Insert = 4,
        [EnumMember]
        Update = 8,
        [EnumMember]
        Delete = 16
    }
}
