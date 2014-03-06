using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.DataSources
{
    public class BrandListDataSource : IListDataSource
    {
        private IRepository<Brand> _brandRepository;

        public IEnumerable<string> SupportedParameters
        {
            get
            {
                return new[] { "Brand" };
            }
        }

        public IEnumerable<IComparisonOperator> SupportedOperators
        {
            get
            {
                return new IComparisonOperator[] {
                    ComparisonOperators.Equals,
                    ComparisonOperators.NotEquals
                };
            }
        }

        public BrandListDataSource(IRepository<Brand> brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public IEnumerable<ListItem> GetItems(IParameter param)
        {
            return _brandRepository.Query()
                                   .Select(x => new ListItem
                                   {
                                       Text = x.Name,
                                       Value = x.Id.ToString()
                                   })
                                   .ToList();
        }
    }
}
