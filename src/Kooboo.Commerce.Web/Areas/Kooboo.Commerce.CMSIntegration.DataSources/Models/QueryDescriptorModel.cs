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

    public class QueryFilterModel
    {
        public string Name { get; set; }

        public IList<QueryFilterParameterModel> Parameters { get; set; }

        public QueryFilterModel()
        {
            Parameters = new List<QueryFilterParameterModel>();
        }

        public QueryFilterModel(QueryFilter filter)
        {
            Name = filter.Name;
            Parameters = filter.Parameters.Select(p => new QueryFilterParameterModel
            {
                Name = p.Name
            })
            .ToList();
        }
    }

    public class QueryFilterParameterModel
    {
        public string Name { get; set; }
    }
}