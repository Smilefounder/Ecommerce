using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Products;

namespace Kooboo.Commerce.Products.Services {

    public interface IPredefinedCustomFieldService {

        CustomField GetById(int id);

        IQueryable<CustomField> Query();

        void Create(CustomField field);

        void Update(CustomField field);

        void UpdateWith(IEnumerable<CustomField> newFields);

        void Delete(CustomField field);
    }
}
