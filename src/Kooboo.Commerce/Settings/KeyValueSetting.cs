using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Settings
{
    // TODO: Better not use 'Setting' suffix, so rename to SettingEntry ?
    public class KeyValueSetting
    {
        [Key]
        public string Category { get; protected set; }

        [Key]
        public  string Key { get; protected set; }

        public  string Value { get; set; }

        protected KeyValueSetting() { }

        public KeyValueSetting(string key, string category)
        {
            Key = key;
            Category = category;
        }
    }
}
