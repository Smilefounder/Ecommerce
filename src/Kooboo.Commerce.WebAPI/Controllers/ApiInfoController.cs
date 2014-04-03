using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.WebAPI.ViewModels;
using System.IO;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class ApiInfoController : Controller
    {
        private ITypeFinder _typeFinder;
        private ICommerceInstanceManager _instanceMgr;

        public ApiInfoController(ICommerceInstanceManager instanceMgr)
        {
            _instanceMgr = instanceMgr;
            _typeFinder = new AppDomainTypeFinder();
        }

        public ActionResult Index()
        {
            var instances = _instanceMgr.GetAllInstanceMetadatas();

            return View(instances);
        }

        public ActionResult AllAPI()
        {
            var types = GetAllApiInfo();
            return View(types);
        }

        public ActionResult APIDetail(string controllerName, string actionName)
        {
            var types = GetAllApiInfo();
            ApiActionInfo ai = null;
            var ci = types.FirstOrDefault(o => o.ControllerName == controllerName);
            if (ci != null)
            {
                ai = ci.Actions.FirstOrDefault(o => o.ActionName == actionName);
            }
            ViewBag.Controller = ci;
            return View(ai);
        }

        private IEnumerable<ApiControllerInfo> GetAllApiInfo()
        {
            var types = _typeFinder.FindClassesOfType<CommerceAPIControllerBase>()
                .Select(o => GetAPIInfo(o));
            return types;
        }

        private XElement LoadComments(Assembly asm)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", asm.GetName().Name + ".xml");
            if (System.IO.File.Exists(path))
            {
                var xdoc = XDocument.Load(path);
                if (xdoc != null)
                {
                    return xdoc.Root.Element("members");
                }
            }
            return null;
        }

        private ApiControllerInfo GetAPIInfo(Type type)
        {
            var comments = LoadComments(type.Assembly);
            var ci = new ApiControllerInfo();
            ci.ControllerName = type.Name.Replace("Controller", "");
            if (comments != null)
            {
                var ename = string.Format("T:{0}.{1}", type.Namespace, type.Name);
                var cele = comments.Elements("member").FirstOrDefault(o => o.Attribute("name").Value == ename);
                if (cele != null)
                    ci.Comments = cele.Element("summary").Value;
            }
            var actions = type.GetMethods()
                    .Where(o => o.IsPublic && !o.IsSpecialName && !o.IsSecurityTransparent && o.GetMethodImplementationFlags() == MethodImplAttributes.IL)
                    .ToArray();
            var ais = new List<ApiActionInfo>();
            foreach (var a in actions)
            {
                var ai = new ApiActionInfo();
                ai.ActionName = a.Name;
                ai.ApiRoute = string.Format("/{{instance}}/{0}/{1}", ci.ControllerName, ai.ActionName);

                var paras = a.GetParameters();
                var eTypeName = string.Format("M:{0}.{1}.{2}", type.Namespace, type.Name, a.Name);
                var eParaNames = "";
                if(paras != null && paras.Count() > 0)
                {
                    eParaNames = string.Format("({0})", string.Join(",", paras.Select(o => o.ParameterType.FullName)));
                }
                var aele = comments.Elements("member").FirstOrDefault(o => o.Attribute("name").Value == eTypeName + eParaNames);
                if (aele == null)
                {
                    var btype = type.BaseType;
                    while (btype != null)
                    {
                        eTypeName = string.Format("M:{0}.{1}.{2}", btype.Namespace, btype.Name, a.Name);
                        aele = comments.Elements("member").FirstOrDefault(o => o.Attribute("name").Value == eTypeName + eParaNames);
                        if (aele != null)
                            break;
                        btype = btype.BaseType;
                    }
                }
                if(aele != null)
                    ai.Comments = aele.Element("summary").Value;

                var pis = new List<ApiParameterInfo>();
                foreach (var p in paras)
                {
                    var pi = new ApiParameterInfo();
                    pi.ParameterName = p.Name;
                    pi.TypeName = p.ParameterType.Name;

                    if(aele != null)
                    {
                        var pele = aele.Elements("param").FirstOrDefault(o => o.Attribute("name").Value == p.Name);
                        if(pele != null)
                        {
                            pi.Comments = pele.Value;
                        }
                    }

                    pis.Add(pi);
                }
                ai.Parameters = pis;
                ais.Add(ai);
            }
            ci.Actions = ais;

            return ci;
        }
    }
}
