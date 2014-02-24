#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Settings
{
    public class KeyValueSetting
    {
        [Key]
        public string Category { get; set; }
        [Key]
        public  string Key { get; set; }

        public  string Value { get; set; }
    }
}
