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
        private IConditionParameterFactory _parameterFactory;

        public ConditionController(IConditionParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public ActionResult AvailableParameters(string contextModelTypeName)
        {
            var contextModelType = Type.GetType(contextModelTypeName, true);
            var models = new List<ConditionParameterModel>();

            foreach (var param in _parameterFactory.GetConditionParameterInfos(contextModelType))
            {
                models.Add(new ConditionParameterModel(param.Parameter));
            }

            return JsonNet(models).Camelcased();
        }

        [HttpPost]
        public string GetExpression(ConditionsModel model)
        {
            return model.Conditions.GetExpression();
        }

        public ActionResult GetConditionModels(string expression, string contextModelType)
        {
            try
            {
                IList<ConditionModel> models = new List<ConditionModel>();

                if (!String.IsNullOrEmpty(expression))
                {
                    var builder = new ConditionModelBuilder(_parameterFactory);
                    models = builder.BuildFrom(Server.UrlDecode(expression), Type.GetType(contextModelType, true));
                }

                return JsonNet(new
                {
                    Success = true,
                    Models = models
                })
                .UseClientConvention();
            }
            catch (ParserException ex)
            {
                return JsonNet(new
                {
                    Success = false,
                    Errors = ex.Errors.Select(x => "Char " + (x.Location.CharIndex + 1) + ": " + x.Message)
                })
                .UseClientConvention();
            }
        }
    }
}