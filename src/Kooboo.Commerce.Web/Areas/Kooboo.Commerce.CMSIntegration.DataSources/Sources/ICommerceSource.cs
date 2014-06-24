using Kooboo.CMS.Sites.DataSource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    public interface ICommerceSource
    {
        string Name { get; }

        IEnumerable<SourceFilterDefinition> Filters { get; }

        IEnumerable<string> SortableFields { get; }

        IEnumerable<string> IncludablePaths { get; }

        object Execute(CommerceSourceContext context);

        IDictionary<string, object> GetDefinitions();
    }
}