using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Search
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class DocumentAttribute : System.Attribute
    {
        public DocumentAttribute(Type docBuilder = null, Type pathBuilder = null, string analyzer = null, string typeName = null, Type objType = null)
        {
            TypeName = typeName;
            Analyzer = analyzer;
            ObjectType = objType;
            DocumentBuilder = docBuilder;
            IndexPathBuilder = pathBuilder;
        }

        public string TypeName { get; set; }
        public Type ObjectType { get; set; }
        public string Analyzer { get; set; }
        public Type DocumentBuilder { get; set; }
        public Type IndexPathBuilder { get; set; }
        public Type FacetFieldNameProvider { get; set; }
    }
}
