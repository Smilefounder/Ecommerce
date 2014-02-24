using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Customers;
using Kooboo.Web.Mvc.Paging;

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

        public IEnumerable<Country> GetAllCountries()
        {
            return _countryRepository.Query().ToArray();
        }

        public IPagedList<Country> GetAllCountries(int? page, int? pageSize)
        {
            var query = _countryRepository.Query().OrderBy(o => o.Id);
            return PageLinqExtensions.ToPagedList(query, page ?? 1, pageSize ?? 50);
        }

        public void Create(Country country)
        {
            _countryRepository.Insert(country);
        }

        public void Update(Country country)
        {
            _countryRepository.Update(country, k => new object[] { k.Id });
        }

        public void Save(Country country)
        {
            if (country.Id > 0)
            {
                bool exists = _countryRepository.Query(o => o.Id == country.Id).Any();
                if (exists)
                    Update(country);
                else
                    Create(country);
            }
            else
            {
                Create(country);
            }
        }

        public void Delete(Country country)
        {
            _countryRepository.Delete(country);
        }
    }
}
