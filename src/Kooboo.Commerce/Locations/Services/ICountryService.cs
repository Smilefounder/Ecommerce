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

        bool Create(Country country);

        bool Update(Country country);

        bool Save(Country country);

        bool Delete(Country country);
    }
}
