using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Expressions;
using Kooboo.Commerce.Rules.Expressions.Formatting;
using Kooboo.Commerce.Rules.Parsing;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Conditions;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class ConditionController : CommerceControllerBase
    {
        public ActionResult AvailableParameters(string dataContextType)
        {
            var contextType = Type.GetType(dataContextType, true);
            var models = new List<ConditionParameterModel>();
            var parameters = ParameterProviderManager.Instance.Providers.SelectMany(x => x.GetParameters(contextType)).DistinctBy(x => x.Name);

            foreach (var param in parameters)
            {
                models.Add(new ConditionParameterModel(param));
            }

            return JsonNet(models).UsingClientConvention();
        }

        [HttpPost]
        public ActionResult BuildConditionModels(BuildConditionModelsRequest request)
        {
            var type = Type.GetType(request.DataContextType, true);
            var builder = new ConditionModelBuilder();
            var models = request.Conditions.Select(c => builder.Build(c.Expression, type, c.Type)).ToList();
            return JsonNet(models).UsingClientConvention();
        }
    }
}