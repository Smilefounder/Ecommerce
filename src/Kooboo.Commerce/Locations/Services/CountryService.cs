using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Customers;

namespace Kooboo.Commerce.Locations.Services
{
    [Dependency(typeof(ICountryService))]
    public class CountryService : ICountryService
    {
        private readonly IRepository<Country> _countryRepository;

        public CountryService(IRepository<Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }
        public Country GetById(int id)
        {
            return _countryRepository.Query(o => o.Id == id).FirstOrDefault();
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
