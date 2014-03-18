using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Search
{
    public interface IDocumentBuilder
    {
        Field[] GetFields(string typeName);
        string GetAnalyzer(string typeName);
        KeyValuePair<string, string> GetKey(string typeName, IDictionary<string, object> values);
    }

    public static class IDocumentBuilderExtensions
    {
        public static Field GetField(this IDocumentBuilder docBuilder, string typeName, string fieldName)
        {
            var fields = docBuilder.GetFields(typeName);
            return fields.FirstOrDefault(o => o.FieldName == fieldName);
        }
    }
}
