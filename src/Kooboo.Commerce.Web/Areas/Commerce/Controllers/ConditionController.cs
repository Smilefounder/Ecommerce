using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Expressions;
using Kooboo.Commerce.Rules.Parsing;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Rules;
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
        private IEnumerable<IConditionParameterProvider> _parameterProviders;

        public ConditionController(IEnumerable<IConditionParameterProvider> parameterProviders)
        {
            _parameterProviders = parameterProviders;
        }

        public ActionResult AvailableParameters(string contextModelTypeName)
        {
            var contextModelType = Type.GetType(contextModelTypeName, true);
            var models = new List<ConditionParameterModel>();

            foreach (var param in _parameterProviders.SelectMany(x => x.GetParameters(contextModelType)).DistinctBy(x => x.Name))
            {
                models.Add(new ConditionParameterModel(param));
            }

            return JsonNet(models).UsingClientConvention();
        }

        [HttpPost]
        public string GetExpression(ConditionsModel model, string contextModelType, bool prettify)
        {
            var expression = model.Conditions.GetExpression();
            if (prettify)
            {
                expression = PrettifyConditionsExpression(expression, contextModelType);
            }

            return expression;
        }

        public string PrettifyConditionsExpression(string expression, string contextModelType)
        {
            return new ConditionsExpressionPrettifier().Prettify(expression, System.Type.GetType(contextModelType, true));
        }

        public ActionResult GetConditionModels(string expression, string contextModelType)
        {
            try
            {
                IList<ConditionModel> models = new List<ConditionModel>();

                if (!String.IsNullOrEmpty(expression))
                {
                    var builder = new ConditionModelBuilder(_parameterProviders);
                    models = builder.BuildFrom(Server.UrlDecode(expression), Type.GetType(contextModelType, true));
                }

                return JsonNet(new
                {
                    Success = true,
                    Models = models
                })
                .UsingClientConvention();
            }
            catch (ParserException ex)
            {
                return JsonNet(new
                {
                    Success = false,
                    Errors = ex.Errors.Select(x => "Char " + (x.Location.CharIndex + 1) + ": " + x.Message)
                })
                .UsingClientConvention();
            }
        }
    }
}