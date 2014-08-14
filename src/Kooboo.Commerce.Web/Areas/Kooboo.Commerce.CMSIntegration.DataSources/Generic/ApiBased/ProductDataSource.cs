﻿using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    [DataContract]
    [KnownType(typeof(ProductDataSource))]
    public class ProductDataSource : ApiBasedDataSource<Product>
    {
        public override string Name
        {
            get { return "Products"; }
        }

        protected override Api.Query<Product> Query(Api.ICommerceApi api)
        {
            return api.Products.Query();
        }
    }
}