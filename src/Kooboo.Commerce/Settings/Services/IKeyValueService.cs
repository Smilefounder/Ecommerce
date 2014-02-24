using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Settings.Services {

    public interface IKeyValueService {

        void Set(string key, string value);

        void Set(string key, string value, string category);

        string Get(string key);

        string Get(string key, string category);

        IEnumerable<KeyValueSetting> GetByCategory(string category);
    }
}
