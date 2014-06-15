using Kooboo.Commerce.CMSIntegration.DataSources.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Models
{
    public class SourceFilterModel
    {
        public string Name { get; set; }

        public IList<FilterParameterModel> Parameters { get; set; }

        public SourceFilterModel()
        {
            Parameters = new List<FilterParameterModel>();
        }

        public SourceFilterModel(SourceFilterDefinition filter)
        {
            Name = filter.Name;
            Parameters = filter.Parameters.Select(p => new FilterParameterModel
            {
                Name = p.Name
            })
            .ToList();
        }
    }
    public class FilterParameterModel
    {
        public string Name { get; set; }
    }
}