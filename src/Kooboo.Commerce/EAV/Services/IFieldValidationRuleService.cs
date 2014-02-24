using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.EAV.Services {

    public interface IFieldValidationRuleService {

        FieldValidationRule Load(int id);

        IQueryable<FieldValidationRule> Query();

        void Create(FieldValidationRule rule);

        void Update(FieldValidationRule rule);

        void Delete(FieldValidationRule rule);
    }
}
