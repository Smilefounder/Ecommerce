using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Products;

namespace Kooboo.Commerce.Products.Services {

    public interface IPredefinedCustomFieldService {

        CustomFieldDefinition GetById(int id);

        IQueryable<CustomFieldDefinition> Query();

        void Create(CustomFieldDefinition field);

        void Update(CustomFieldDefinition field);

        void UpdateWith(IEnumerable<CustomFieldDefinition> newFields);

        void Delete(CustomFieldDefinition field);
    }
}
