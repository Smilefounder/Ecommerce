using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Products;

namespace Kooboo.Commerce.EAV.Services {

    public interface ICustomFieldService {

        CustomField Load(int id);

        IQueryable<CustomField> Query();

        void Create(CustomField field);

        void Update(CustomField field);

        void Delete(CustomField field);

        IEnumerable<CustomField> GetSystemFields();
        void SetSystemFields(IEnumerable<CustomField> fields);
    }
}
