using Kooboo.CMS.Common.Formula;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.DataSource;
using Kooboo.Commerce.CMSIntegration.DataSources.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    [DataContract(Name = "CommerceDataSource")]
    [KnownType(typeof(CommerceDataSource))]
    public class CommerceDataSource : IDataSource
    {
        /// <summary>
        /// Name of the commerce source.
        /// </summary>
        [DataMember]
        public string SourceName { get; set; }

        [DataMember]
        public TakeOperation TakeOperation { get; set; }

        [DataMember]
        public List<DataSourceFilter> Filters { get; set; }

        [DataMember]
        public List<string> Includes { get; set; }

        [DataMember]
        public string Top { get; set; }

        [DataMember]
        public string SortField { get; set; }

        [DataMember]
        public SortDirection SortDirection { get; set; }

        [DataMember]
        public bool EnablePaging { get; set; }

        [DataMember]
        public string PageSize { get; set; }

        [DataMember]
        public string PageNumber { get; set; }

        public CommerceDataSource()
        {
            Filters = new List<DataSourceFilter>();
            Includes = new List<string>();
        }

        public object Execute(DataSourceContext dataSourceContext)
        {
            var source = ResolveCommerceSource();
            var context = new CommerceSourceContext(dataSourceContext)
            {
                TakeOperation = TakeOperation,
                SortField = SortField,
                SortDirection = SortDirection
            };

            // Filters
            if (Filters != null && Filters.Count > 0)
            {
                foreach (var each in Filters)
                {
                    var filterDefinition = source.Filters.FirstOrDefault(f => f.Name == each.Name);
                    if (filterDefinition != null)
                    {
                        var filter = new Sources.SourceFilter(each.Name);

                        foreach (var param in each.ParameterValues)
                        {
                            var paramDefinition = filterDefinition.Parameters.Find(p => p.Name == param.ParameterName);
                            if (paramDefinition != null)
                            {
                                var strParamValue = ParameterizedFieldValue.GetFieldValue(param.ParameterValue, dataSourceContext.ValueProvider);
                                var paramValue = ResolveParameterValue(strParamValue, paramDefinition.Type);
                                filter.ParameterValues.Add(param.ParameterName, paramValue);
                            }
                        }

                        context.Filters.Add(filter);
                    }
                }
            }

            // Pagination
            context.EnablePaging = EnablePaging;
            if (EnablePaging)
            {
                var pageSize = ParameterizedFieldValue.GetFieldValue(PageSize, dataSourceContext.ValueProvider);
                if (!String.IsNullOrEmpty(pageSize))
                {
                    context.PageSize = Convert.ToInt32(pageSize);
                }

                var pageNumber = ParameterizedFieldValue.GetFieldValue(PageNumber, dataSourceContext.ValueProvider);
                if (!String.IsNullOrEmpty(pageNumber))
                {
                    context.PageNumber = Convert.ToInt32(pageNumber);
                }
            }

            // Top
            if (!String.IsNullOrWhiteSpace(Top))
            {
                var top = ParameterizedFieldValue.GetFieldValue(Top, dataSourceContext.ValueProvider);
                if (!String.IsNullOrWhiteSpace(top))
                {
                    context.Top = Convert.ToInt32(top);
                }
            }

            // Includes
            if (Includes != null)
            {
                context.Includes = Includes.ToList();
            }

            return source.Execute(context);
        }

        private ICommerceSource ResolveCommerceSource()
        {
            return EngineContext.Current
                                .ResolveAll<ICommerceSource>()
                                .FirstOrDefault(s => s.Name == SourceName);
        }

        private object ResolveParameterValue(string strValue, Type type)
        {
            if (String.IsNullOrWhiteSpace(strValue))
            {
                return null;
            }

            Type targetType = type;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                targetType = Nullable.GetUnderlyingType(type);
            }

            return Convert.ChangeType(strValue, targetType);
        }

        public IDictionary<string, object> GetDefinitions(DataSourceContext dataSourceContext)
        {
            var source = ResolveCommerceSource();
            return source.GetDefinitions();
        }

        public IEnumerable<string> GetParameters()
        {
            var parser = new FormulaParser();
            var parameters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (!String.IsNullOrWhiteSpace(PageSize))
            {
                AddParameters(parameters, parser.GetParameters(PageSize));
            }
            if (!String.IsNullOrWhiteSpace(PageNumber))
            {
                AddParameters(parameters, parser.GetParameters(PageNumber));
            }

            if (Filters != null)
            {
                foreach (var filter in Filters)
                {
                    foreach (var value in filter.ParameterValues)
                    {
                        if (!String.IsNullOrWhiteSpace(value.ParameterValue))
                        {
                            AddParameters(parameters, parser.GetParameters(value.ParameterValue));
                        }
                    }
                }
            }

            return parameters;
        }

        private void AddParameters(HashSet<string> set, IEnumerable<string> parameters)
        {
            foreach (var param in parameters)
            {
                set.Add(param);
            }
        }

        public bool IsEnumerable()
        {
            if (TakeOperation == TakeOperation.First)
            {
                return false;
            }

            return true;
        }
    }
}