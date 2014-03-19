using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.EAV.Services {

    public interface IFieldValidationRuleService {

        FieldValidationRule Load(int id);

        IQueryable<FieldValidationRule> Query();

        bool Create(FieldValidationRule rule);

        bool Update(FieldValidationRule rule);

        bool Delete(FieldValidationRule rule);
    }
}
