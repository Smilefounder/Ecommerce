﻿using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.Models;
using Kooboo.Commerce.Api;
using Kooboo.Commerce.Api;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    public abstract class ApiCommerceSource : ICommerceSource
    {
        internal ApiQueryDescriptor Descriptor { get; private set; }

        protected List<SourceFilterDefinition> InternalFilters;
        protected HashSet<string> InternalSortableFields = new HashSet<string>();
        protected HashSet<string> InternalIncludablePaths;

        protected Type QueryType { get; private set; }

        protected Type ItemType { get; private set; }

        public string Name { get; protected set; }

        public IEnumerable<SourceFilterDefinition> Filters
        {
            get
            {
                return InternalFilters;
            }
        }

        public IEnumerable<string> SortableFields
        {
            get
            {
                return InternalSortableFields;
            }
        }

        public IEnumerable<string> IncludablePaths
        {
            get
            {
                return InternalIncludablePaths;
            }
        }

        protected ApiCommerceSource(string name, Type queryType, Type itemType)
        {
            Name = name;
            QueryType = queryType;
            ItemType = itemType;

            var descriptor = ApiQueryDescriptor.GetDescriptor(queryType);

            InternalFilters = descriptor.Filters.Select(f => f.ToFilterDefinition()).ToList();
            InternalIncludablePaths = new HashSet<string>(descriptor.IncludablePaths);

            Descriptor = descriptor;
        }

        protected abstract object GetQuery(ICommerceApi api);

        public virtual object Execute(CommerceSourceContext context)
        {
            var dataSourceContext = context.DataSourceContext;

            var apiContext = new ApiContext(context.Site.CommerceInstanceName(), CultureInfo.GetCultureInfo(context.Site.Culture), null);
            var api = ApiService.Get(context.Site.ApiType(), apiContext);

            var query = GetQuery(api);

            if (context.Filters != null && context.Filters.Count > 0)
            {
                ApplyFilters(query, context.Filters, context);
            }
            if (context.Includes != null && context.Includes.Count > 0)
            {
                ApplyIncludes(query, context.Includes, context);
            }

            // Executing result
            if (context.TakeOperation == Kooboo.Commerce.CMSIntegration.DataSources.TakeOperation.List)
            {
                if (context.EnablePaging)
                {
                    var pageSize = context.PageSize.GetValueOrDefault(10);
                    var pageNumber = context.PageNumber.GetValueOrDefault(1);

                    return CallMethod(query, "Pagination", pageNumber - 1, pageSize);
                }

                return CallMethod(query, "ToArray");
            }
            else
            {
                return CallMethod(query, "FirstOrDefault");
            }
        }

        public virtual IDictionary<string, object> GetDefinitions()
        {
            return DataSourceDefinitionHelper.GetDefinitions(ItemType);
        }

        protected virtual void ApplyFilters(object query, List<SourceFilter> filters, CommerceSourceContext context)
        {
            foreach (var filter in filters)
            {
                var definition = Descriptor.Filters.FirstOrDefault(f => f.Name == filter.Name);
                if (definition != null)
                {
                    var parameters = new object[definition.Parameters.Count];
                    for (var i = 0; i < definition.Parameters.Count; i++)
                    {
                        var param = definition.Parameters[i];
                        var paramValue = filter.ParameterValues[param.Name];
                        parameters[i] = filter.ParameterValues[param.Name];
                    }

                    CallMethod(query, definition.Method.Name, parameters);
                }
            }
        }

        protected object CallMethod(object obj, string method, params object[] parameters)
        {
            var methodInfo = obj.GetType().GetMethod(method, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (methodInfo == null)
                throw new InvalidOperationException("Method '" + method + "' was not found.");

            return methodInfo.Invoke(obj, parameters);
        }

        protected virtual void ApplyIncludes(object query, IEnumerable<string> includes, CommerceSourceContext context)
        {
            var methods = query.GetType()
                               .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                               .Where(m => m.Name == "Include");

            MethodInfo includeMethod = null;

            foreach (var method in methods)
            {
                var args = method.GetParameters();
                if (args.Length == 1 && args[0].ParameterType == typeof(string))
                {
                    includeMethod = method;
                    break;
                }
            }

            if (includeMethod == null)
                throw new InvalidOperationException("Cannot find Include(string path) method in query api.");

            foreach (var path in includes)
            {
                includeMethod.Invoke(query, new object[] { path });
            }
        }
    }
}