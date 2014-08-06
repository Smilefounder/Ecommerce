using Kooboo.Commerce.CMSIntegration.DataSources.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Models
{
    public class GenericDataSourceModel
    {
        public string Name { get; set; }

        public IList<FilterModel> Filters { get; set; }

        public IList<string> SortableFields { get; set; }

        public IList<string> IncludablePaths { get; set; }

        public GenericDataSourceModel()
        {
            Filters = new List<FilterModel>();
            SortableFields = new List<string>();
            IncludablePaths = new List<string>();
        }

        public GenericDataSourceModel(GenericCommerceDataSource source)
            : this()
        {
            Name = source.Name;

            if (source.Filters != null)
            {
                Filters = source.Filters.Select(f => new FilterModel(f)).ToList();
            }

            if (source.SortableFields != null)
            {
                SortableFields = source.SortableFields.ToList();
            }

            if (source.IncludablePaths != null)
            {
                IncludablePaths = source.IncludablePaths.ToList();
            }
        }
    }
}