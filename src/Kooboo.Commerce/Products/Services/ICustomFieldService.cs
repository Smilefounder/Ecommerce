using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Products;

namespace Kooboo.Commerce.EAV.Services {

    public interface ICustomFieldService {

        CustomField GetById(int id);

        IQueryable<CustomField> Query();

        IQueryable<CustomField> PredefinedFields();

        void Create(CustomField field);

        void Update(CustomField field);

        void Delete(CustomField field);
    }
}
