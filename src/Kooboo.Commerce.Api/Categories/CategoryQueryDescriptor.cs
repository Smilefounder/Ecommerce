﻿using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Categories
{
    public class CategoryQueryDescriptor : IQueryDescriptor
    {
        public IEnumerable<FilterDescription> Filters
        {
            get
            {
                return new[] { CategoryFilters.ById, CategoryFilters.ByName, CategoryFilters.ByParent, CategoryFilters.ByCustomField };
            }
        }

        public IEnumerable<string> OptionalIncludeFields
        {
            get
            {
                var fields = OptionalIncludeAttribute.GetOptionalIncludeFields(typeof(Category)).ToList();
                fields.Add("Subtrees");
                return fields;
            }
        }

        public IEnumerable<string> DefaultIncludedFields
        {
            get { return null; }
        }

        public IEnumerable<string> SortFields
        {
            get { return new[] { "Id", "Name" }; }
        }
    }
}
