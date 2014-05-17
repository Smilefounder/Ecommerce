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

        public IList<string> OptionalIncludablePaths { get; set; }

        public QueryDescriptorModel()
        {
            Filters = new List<QueryFilterModel>();
            OptionalIncludablePaths = new List<string>();
        }

        public QueryDescriptorModel(QueryDescriptor descriptor)
        {
            Name = descriptor.Name;
            Filters = descriptor.Filters.Select(f => new QueryFilterModel(f)).ToList();
            OptionalIncludablePaths = descriptor.OptionalIncludablePaths.ToList();
        }
    }
}