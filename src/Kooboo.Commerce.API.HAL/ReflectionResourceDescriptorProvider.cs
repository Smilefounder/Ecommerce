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

    }
}
