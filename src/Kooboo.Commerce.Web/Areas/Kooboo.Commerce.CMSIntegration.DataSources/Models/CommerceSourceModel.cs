using Kooboo.Commerce.CMSIntegration.DataSources.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Models
{
    public class CommerceSourceModel
    {
        public string Name { get; set; }

        public IList<SourceFilterModel> Filters { get; set; }

        public IList<string> IncludablePaths { get; set; }

        public CommerceSourceModel()
        {
            Filters = new List<SourceFilterModel>();
            IncludablePaths = new List<string>();
        }

        public CommerceSourceModel(ICommerceSource source) : this()
        {
            Name = source.Name;

            if (source.Filters != null)
            {
                Filters = source.Filters.Select(f => new SourceFilterModel(f)).ToList();
            }

            if (source.IncludablePaths != null)
            {
                IncludablePaths = source.IncludablePaths.ToList();
            }
        }
    }
}