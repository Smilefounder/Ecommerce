using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public interface IObjectPersistence
    {
        IEnumerable<T> GetAllObjects<T>();
        T GetObject<T>(string name);
        void SaveObject<T>(string name, T obj);
    }
}
