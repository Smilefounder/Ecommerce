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
        [DataMember]
        public string QueryName { get; set; }

        [DataMember]
        public QueryType QueryType { get; set; }

        [DataMember]
        public List<DataSourceFilter> Filters { get; set; }

        [DataMember]
        public List<string> Includes { get; set; }

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
            var source = EngineContext.Current.Resolve<ICommerceSource>(QueryName);
            var context = new CommerceSourceContext(dataSourceContext)
            {
                QueryType = QueryType
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
                                var paramValue = strParamValue == null ? null : Convert.ChangeType(strParamValue, paramDefinition.Type);
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

            // Includes
            if (Includes != null)
            {
                context.Includes = Includes.ToList();
            }

            return source.Execute(context);
        }

        public bool HasAnyParameters()
        {
            foreach (var filter in Filters)
            {
                if (filter.ParameterValues.Any(v => ParameterizedFieldValue.IsParameterizedField(v.ParameterValue)))
                {
                    return true;
                }
            }

            if (ParameterizedFieldValue.IsParameterizedField(PageSize))
            {
                return true;
            }

            if (ParameterizedFieldValue.IsParameterizedField(PageNumber))
            {
                return true;
            }

            return false;
        }
    }
}