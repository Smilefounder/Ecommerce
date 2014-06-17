using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Products;

namespace Kooboo.Commerce.EAV.Services {

    public interface ICustomFieldService {

        CustomField GetById(int id);

        IQueryable<CustomField> Query();

        bool Create(CustomField field);

        bool Update(CustomField field);

        bool Delete(CustomField field);

        IEnumerable<CustomField> GetSystemFields();
    }
}
