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

namespace Kooboo.Commerce.HAL
{
    [Dependency(typeof(IResourceDescriptorProvider), ComponentLifeStyle.Singleton)]
    public class ReflectionResourceDescriptorProvider : IResourceDescriptorProvider
    {
        private ITypeFinder _typeFinder;
        private IEnumerable<ResourceDescriptor> _resources;

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
                        if (string.IsNullOrEmpty(resAttr.Name))
                            resource.ResourceName = string.Format("{0}:{1}", controllerName, action.Name);
                        else if (resAttr.Name.IndexOf(':') < 0)
                            resource.ResourceName = string.Format("{0}:{1}", controllerName, resAttr.Name);
                        else
                            resource.ResourceName = resAttr.Name;

                        if (string.IsNullOrEmpty(resAttr.Uri))
                            resource.ResourceUri = string.Format("/{{instance}}/{0}/{1}", controllerName, resource.ResourceName);
                        else
                            resource.ResourceUri = resAttr.Uri;
                        resource.IsListResource = resAttr.IsList;
                        resource.ItemResourceName = resAttr.ItemName;
                        resources.Add(resource);
                    }
                }
            }

            return resources;
        }

    }
}
