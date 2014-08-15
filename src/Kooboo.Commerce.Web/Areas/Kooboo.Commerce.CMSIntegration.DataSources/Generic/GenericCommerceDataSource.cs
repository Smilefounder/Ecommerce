using Kooboo.CMS.Common.Formula;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.Commerce.Api.Metadata;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic
{
    [DataContract]
    [KnownType(typeof(GenericCommerceDataSource))]
    public abstract class GenericCommerceDataSource : ICommerceDataSource
    {
        [JsonProperty]
        public abstract string Name { get; }

        public string EditorVirtualPath
        {
            get
            {
                return "~/Areas/" + Strings.AreaName + "/Views/GenericDataSourceDesigner.cshtml";
            }
        }

        private GenericCommerceDataSourceSettings _settings;

        [DataMember, JsonProperty]
        public GenericCommerceDataSourceSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = new GenericCommerceDataSourceSettings();
                }
                return _settings;
            }
            set
            {
                _settings = value;
            }
        }

        [JsonProperty]
        public virtual IEnumerable<FilterDescription> Filters
        {
            get { return null; }
        }

        [JsonProperty]
        public virtual IEnumerable<string> SortFields
        {
            get { return null; }
        }

        [JsonProperty]
        public virtual IEnumerable<string> OptionalIncludeFields
        {
            get { return null; }
        }

        public virtual object Execute(CommerceDataSourceContext context)
        {
            ParsedGenericCommerceDataSourceSettings settings;
            if (TryParseSettings(context, out settings))
            {
                return ExecuteCore(context, settings);
            }

            return null;
        }

        protected abstract object ExecuteCore(CommerceDataSourceContext context, ParsedGenericCommerceDataSourceSettings settings);

        public virtual IDictionary<string, object> GetDefinitions(CommerceDataSourceContext context)
        {
            return new Dictionary<string, object>();
        }

        public virtual IEnumerable<string> GetParameters()
        {
            var parser = new FormulaParser();
            var parameters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (!String.IsNullOrWhiteSpace(Settings.PageSize))
            {
                parser.GetParameters(Settings.PageSize).ForEach((p, i) => parameters.Add(p));
            }
            if (!String.IsNullOrWhiteSpace(Settings.PageNumber))
            {
                parser.GetParameters(Settings.PageNumber).ForEach((p, i) => parameters.Add(p));
            }

            if (Settings.Filters != null)
            {
                foreach (var filter in Settings.Filters)
                {
                    foreach (var value in filter.ParameterValues)
                    {
                        if (!String.IsNullOrWhiteSpace(value.ParameterValue))
                        {
                            parser.GetParameters(value.ParameterValue).ForEach((p, i) => parameters.Add(p));
                        }
                    }
                }
            }

            return parameters;
        }

        public virtual bool IsEnumerable()
        {
            if (Settings.TakeOperation == TakeOperation.First)
            {
                return false;
            }

            return true;
        }

        protected bool TryParseSettings(CommerceDataSourceContext context, out ParsedGenericCommerceDataSourceSettings settings)
        {
            if (Settings == null)
            {
                settings = null;
                return false;
            }

            settings = new ParsedGenericCommerceDataSourceSettings();

            // Filters
            if (Settings.Filters != null && Settings.Filters.Count > 0)
            {
                foreach (var savedFilter in Settings.Filters)
                {
                    var filterDefinition = Filters.FirstOrDefault(f => f.Name == savedFilter.Name);
                    if (filterDefinition == null)
                    {
                        continue;
                    }

                    var parsedFilter = savedFilter.Parse(filterDefinition, context);
                    if (parsedFilter != null)
                    {
                        settings.Filters.Add(parsedFilter);
                    }
                    else if (savedFilter.Required)
                    {
                        // If filter is required but it's not presented in current context. Parse failed.
                        settings = null;
                        return false;
                    }
                }
            }

            settings.TakeOperation = Settings.TakeOperation;

            // Pagination
            settings.EnablePaging = Settings.EnablePaging;
            if (settings.EnablePaging)
            {
                settings.PageSize = context.ResolveFieldValue<int?>(Settings.PageSize, null);
                settings.PageNumber = context.ResolveFieldValue<int?>(Settings.PageNumber, null);
            }

            settings.Top = context.ResolveFieldValue<int?>(Settings.Top, null);

            // Includes
            if (Settings.Includes != null)
            {
                settings.Includes = Settings.Includes.ToList();
            }

            return true;
        }
    }
}