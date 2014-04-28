using Kooboo.Commerce.API.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.HAL
{
    public class ImplicitLinkProviderModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ImplicitLinkProviderModel() { }

        public ImplicitLinkProviderModel(IImplicitLinkProvider provider)
        {
            Name = provider.Name;
            Description = provider.Description;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}