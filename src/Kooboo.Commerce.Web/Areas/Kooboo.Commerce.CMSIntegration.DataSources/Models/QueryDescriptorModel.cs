using Kooboo.Commerce.API.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Models
{
    public class QueryDescriptorModel
    {
        public string Name { get; set; }

        public IList<QueryFilterModel> Filters { get; set; }

        public QueryDescriptorModel()
        {
            Filters = new List<QueryFilterModel>();
        }

        public QueryDescriptorModel(QueryDescriptor descriptor)
        {
            Name = descriptor.Name;
            Filters = descriptor.Filters.Select(f => new QueryFilterModel(f)).ToList();
        }
    }
}