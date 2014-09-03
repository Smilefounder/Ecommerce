using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Customers;

namespace Kooboo.Commerce.Countries
{
    [Dependency(typeof(CountryService))]
    public class CountryService
    {
        private readonly IRepository<Country> _countryRepository;

        public CountryService(ICommerceDatabase database)
        {
            _countryRepository = database.GetRepository<Country>();
        }

        public Country GetById(int id)
        {
            return _countryRepository.Find(id);
        }

        public IQueryable<Country> Query()
        {
            return _countryRepository.Query();
        }

        public void Create(Country country)
        {
            _countryRepository.Insert(country);
        }

        public void Update(Country country)
        {
            _countryRepository.Update(country);
        }

        public void Delete(Country country)
        {
            _countryRepository.Delete(country);
        }
    }
}
