using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    public class PaginationImplictLinksProvider : IImplicitLinkProvider
    {
        public IEnumerable<Link> GetImplicitLinks(IUriResolver uriResolver, ResourceDescriptor descriptor, IDictionary<string, object> parameters)
        {
            int pageIndex = Convert.ToInt32(parameters["pageIndex"]);
            int pageSize = Convert.ToInt32(parameters["pageSize"]);
            int totalItemCount = Convert.ToInt32(parameters["totalItemCount"]);
            int totalPageCount = Convert.ToInt32(Math.Ceiling((double)totalItemCount / pageSize));

            var links = new List<Link>();
            parameters["pageIndex"] = 0;
            links.Add(new Link
            {
                Rel = "first",
                Href = uriResolver.Resovle(descriptor.ResourceUri, parameters)
            });
            parameters["pageIndex"] = pageIndex - 1 <= 0 ? 0 : pageIndex - 1;
            links.Add(new Link
            {
                Rel = "prev",
                Href = uriResolver.Resovle(descriptor.ResourceUri, parameters)
            });
            parameters["pageIndex"] = pageIndex + 1 <= totalPageCount - 1 ? pageIndex + 1 : totalPageCount - 1;
            links.Add(new Link
            {
                Rel = "next",
                Href = uriResolver.Resovle(descriptor.ResourceUri, parameters)
            });
            parameters["pageIndex"] = totalPageCount - 1;
            links.Add(new Link
            {
                Rel = "last",
                Href = uriResolver.Resovle(descriptor.ResourceUri, parameters)
            });
            parameters["pageIndex"] = "{goto}";
            links.Add(new Link
            {
                Rel = "goto",
                Href = uriResolver.Resovle(descriptor.ResourceUri, parameters)
            });
            parameters["pageIndex"] = pageIndex;
            return links;
        }
    }
}
