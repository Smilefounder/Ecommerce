using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Locations.Services
{
    public interface ICountryService
    {
        Country GetById(int id);

        IQueryable<Country> Query();

        void Create(Country country);

        void Update(Country country);

        void Save(Country country);

        void Delete(Country country);
    }
}
