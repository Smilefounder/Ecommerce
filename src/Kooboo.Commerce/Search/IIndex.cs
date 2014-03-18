using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Search
{
    public interface IIndex
    {
        string TypeName { get; }
        string Directory { get; }
        IDocumentBuilder DocumentBuilder { get; }
        IIndexPathBuilder IndexPathBuilder { get; }

        void Initialize();
        void AddDocument(IDictionary<string, object> values, float boost = 1.0f);
        void UpdateDocument(IDictionary<string, object> values);
        void RemoveDocument(IDictionary<string, object> values);

        void Save();
        void Optimize();
        void Flush();
        void Close();
    }
}
