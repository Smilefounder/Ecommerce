using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Runtime;

namespace Kooboo.Commerce.Search
{
    public class AutoRegisterAttributeDocumentType : IDependencyRegistrar
    {
        public int Order
        {
            get { return 200; }
        }

        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            var types = typeFinder.FindClassesOfType<object>();
            foreach (var type in types)
            {
                var docAttr = type.GetCustomAttribute<DocumentAttribute>();
                if (docAttr != null)
                {
                    if (string.IsNullOrEmpty(docAttr.TypeName))
                        docAttr.TypeName = type.Name;
                    if (docAttr.ObjectType == null)
                        docAttr.ObjectType = type;
                    if (docAttr.DocumentBuilder == null)
                        docAttr.DocumentBuilder = typeof(AttributeDocumentBuilder);
                    if (docAttr.IndexPathBuilder == null)
                        docAttr.IndexPathBuilder = typeof(LocaleDateTimeIndexPathBuilder);
                    if (docAttr.FacetFieldNameProvider == null)
                        docAttr.FacetFieldNameProvider = typeof(LocaleFacetFieldNameProvider);
                    AttributedTypes.SearchTypes.Add(type.Name, docAttr);
                }
            }
        }
    }
}
