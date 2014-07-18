using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Conditions.Expressions;
using Kooboo.Commerce.Rules.Conditions.Expressions.Formatting;
using Kooboo.Commerce.Rules.Parameters;
using Kooboo.Commerce.Rules.Conditions.Parsing;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Commerce.Web.Framework.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class ConditionController : CommerceController
    {
        public ActionResult AvailableParameters(string dataContextType)
        {
            var contextType = Type.GetType(dataContextType, true);
            var models = new List<RuleParameterModel>();
            var parameters = RuleParameterProviders.Providers.SelectMany(x => x.GetParameters(contextType)).ToList();

            foreach (var param in parameters.OrderBy(p => p.Name))
            {
                models.Add(new RuleParameterModel(param));
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