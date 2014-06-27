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

        public bool Create(Country country)
        {
            return _countryRepository.Insert(country);
        }

        public bool Update(Country country)
        {
            return _countryRepository.Update(country, country);
        }

        public bool Save(Country country)
        {
            if (country.Id > 0)
            {
                bool exists = _countryRepository.Query(o => o.Id == country.Id).Any();
                if (exists)
                    return Update(country);
                else
                    return Create(country);
            }
            else
            {
                return Create(country);
            }
        }

        public bool Delete(Country country)
        {
            return _countryRepository.Delete(country);
        }
    }
}
