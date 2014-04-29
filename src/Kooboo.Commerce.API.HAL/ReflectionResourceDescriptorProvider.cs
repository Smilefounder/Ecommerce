using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Kooboo.CMS.Common.Runtime;
using System.Web.Http;
using System.Xml.Linq;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.Commerce.API.HAL
{
    [Dependency(typeof(IResourceDescriptorProvider), ComponentLifeStyle.Singleton)]
    public class ReflectionResourceDescriptorProvider : IResourceDescriptorProvider
    {
        private ITypeFinder _typeFinder;
        private IEnumerable<ResourceDescriptor> _resources;
        private string[] _specialActionNames = new string[] { "get", "put", "post", "delete" };

        public ReflectionResourceDescriptorProvider()
        {
            _typeFinder = new AppDomainTypeFinder();
        }

        public IEnumerable<ResourceDescriptor> GetAllDescriptors()
        {
            ScanResources();
            return _resources;
        }

        public ResourceDescriptor GetDescriptor(string resourceName)
        {
            ScanResources();
            resourceName = resourceName.ToLower();
            return _resources.FirstOrDefault(o => o.ResourceName == resourceName);
        }

        private void ScanResources()
        {
            // avoid always scan the resouces, that is expensive.
            if (_resources != null)
                return;

            var resources = new List<ResourceDescriptor>();
            var types = _typeFinder.FindClassesOfType<ApiController>().Where(o => !o.IsAbstract);
            foreach (var type in types)
            {
                var typeResources = GetResources(type);
                if (typeResources != null)
                {
                    resources.AddRange(typeResources);
                }
            }
            _resources = resources;
        }

        /// <summary>
        /// get resources from type
        /// </summary>
        /// <param name="type">resource type</param>
        /// <returns>resources of this type</returns>
        private IEnumerable<ResourceDescriptor> GetResources(Type type)
        {
            var resources = new List<ResourceDescriptor>();
            // get api actions
            var actions = type.GetMethods()
                    .Where(o => o.IsPublic && !o.IsSpecialName && !o.IsSecurityTransparent && o.GetMethodImplementationFlags() == MethodImplAttributes.IL)
                    .ToArray();
            foreach (var action in actions)
            {
                var resAttrs = action.GetCustomAttributes(typeof(ResourceAttribute), true);
                if (resAttrs != null && resAttrs.Length > 0)
                {
                    var resAttr = resAttrs[0] as ResourceAttribute;
                    if (resAttr != null)
                    {
                        var resource = new ResourceDescriptor();
                        
                        var controllerName = type.Name.Replace("Controller", "");
                        resource.ResourceName = NormalizeResourceName(resAttr.Name, controllerName, action.Name);


                        if (string.IsNullOrEmpty(resAttr.Uri))
                        {
                            string actionName = action.Name;
                            if (_specialActionNames.Contains(action.Name.ToLower()))
                                actionName = string.Empty;
                            resource.ResourceUri = string.Format("/{{instance}}/{0}/{1}", controllerName.ToLower(), actionName.ToLower()).TrimEnd('/');
                        }
                        else
                            resource.ResourceUri = resAttr.Uri.Replace("{controller}", controllerName.ToLower()).Replace("{action}", action.Name.ToLower());
                        resource.IsListResource = resAttr.IsList;
                        if (!string.IsNullOrEmpty(resAttr.ItemName))
                        {
                            resource.IsListResource = true;
                            resource.ItemResourceName = NormalizeResourceName(resAttr.ItemName, controllerName, null);
                        }
                        
                        if (resAttr.ImplicitLinksProvider != null && typeof(IImplicitLinkProvider).IsAssignableFrom(resAttr.ImplicitLinksProvider))
                        {
                            var linksProvider = Activator.CreateInstance(resAttr.ImplicitLinksProvider) as IImplicitLinkProvider;
                            resource.ImplicitLinkProvider = linksProvider;
                        }

                        var paras = action.GetParameters();
                        if (paras != null && paras.Count() > 0)
                        {
                            List<HalParameter> inputParameters = new List<HalParameter>();
                            var halParas = paras.Where(o => o.GetCustomAttributes(true).OfType<HalParameterAttribute>().Count() > 0);
                            if (halParas.Count() > 0)
                            {
                                foreach(var para in halParas)
                                {
                                    var pa = para.GetCustomAttributes(true).OfType<HalParameterAttribute>().First();
                                    if (string.IsNullOrEmpty(pa.Name))
                                        pa.Name = para.Name;
                                    if (pa.ParameterType == null)
                                        pa.ParameterType = para.ParameterType;
                                    if (!pa.Required.HasValue)
                                        pa.Required = !para.IsOptional && IsTypeRequired(pa.ParameterType);

                                    inputParameters.Add(new HalParameter(string.Format("{0}.{1}", controllerName, pa.Name), pa.ParameterType, pa.Required.Value));
                                }
                            }
                            else
                            {
                                foreach(var para in paras)
                                {
                                    if(IsSimpleType(para.ParameterType))
                                    {
                                        bool required = !para.IsOptional && IsTypeRequired(para.ParameterType);
                                        inputParameters.Add(new HalParameter(string.Format("{0}.{1}", controllerName, para.Name), para.ParameterType, required));
                                    }
                                }
                            }

                            List<HalParameter> outputParameters = new List<HalParameter>();
                            if(action.ReturnType != typeof(void))
                            { 
                                var returnPara = action.ReturnParameter;
                                if (IsSimpleType(returnPara.ParameterType))
                                {
                                    outputParameters.Add(new HalParameter(returnPara.Name, returnPara.ParameterType, IsTypeRequired(returnPara.ParameterType)));
                                }
                                else
                                {
                                    var ptype = returnPara.ParameterType;
                                    if (ptype.IsGenericType && ptype.GetGenericTypeDefinition().Equals(typeof(IListResource<>)))
                                        ptype = ptype.GetGenericTypeDefinition().GetGenericArguments()[0];
                                    if (typeof(IItemResource).IsAssignableFrom(ptype))
                                    {

                                        var returnParas = ptype.GetProperties();
                                        foreach(var para in returnParas)
                                        {
                                            if (para.Name == "Links")
                                                continue;
                                            if (IsSimpleType(para.PropertyType))
                                            {
                                                outputParameters.Add(new HalParameter(string.Format("{0}.{1}", ptype.Name, para.Name), para.PropertyType, IsTypeRequired(para.PropertyType)));
                                            }
                                        }
                                    }
                                }
                            }

                            resource.InputPramameters = inputParameters.ToArray();
                            resource.OutputParameters = outputParameters.ToArray();
                        }

                        resources.Add(resource);
                    }
                }
            }

            return resources;
        }


        private string NormalizeResourceName(string resName, string controller, string action)
        {
            if (string.IsNullOrEmpty(resName))
                resName = string.Format("{0}:{1}", controller, action);
            else if (resName.IndexOf(':') < 0)
                resName = string.Format("{0}:{1}", controller, resName);
            return resName.ToLower();
        }

        private bool IsTypeRequired(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        private bool IsSimpleType(Type type)
        {
            if (type.Equals(typeof(string)) || type.IsValueType)
                return true;
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                return type.GetGenericArguments()[0].IsValueType;
            return false;
        }
    }
}
