using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Kooboo.Commerce.API.HAL;

namespace Kooboo.Commerce.API.CmsSite
{
    /// <summary>
    /// this resolver is temporary for providing cms friendly url to sample commerce vitaminstore. 
    /// this should be removed and replaced with kooboo cms friendly url solution
    /// TODO: to be removed.
    /// </summary>
    public class CmsSiteUrlResolver
    {
        public static string MapUrl(IItemResource resource, string linkRelName, string url = null)
        {
            if (resource.Links == null)
                return url;
            var link = resource.Links.FirstOrDefault(o => o.Rel.ToLower() == linkRelName.ToLower());
            if (link != null)
            {
                Uri linkUrl = MockUri(link.Href);
                // we ignore the 1,2 segements, which stand for "/" and "{commerce instance}"
                if (linkUrl.Segments.Length > 2)
                {
                    var qs = HttpUtility.ParseQueryString(linkUrl.Query);
                    List<string> path = new List<string>();
                    switch(linkUrl.Segments[2].TrimEnd('/').ToLower())
                    {
                        case "product":
                            if(linkUrl.Segments.Length > 3)
                            switch(linkUrl.Segments[3].TrimEnd('/').ToLower())
                            {
                                case "list":
                                    if(qs.AllKeys.Contains("categoryId"))
                                    {
                                        path.Add("category");
                                        path.Add(qs["categoryId"]);
                                        qs.Remove("categoryId");
                                    }
                                    break;
                            }
                            break;

                    }
                    if(qs.AllKeys.Contains("pageIndex"))
                    {
                        int pidx = 0;
                        int.TryParse(qs["pageIndex"], out pidx);
                        if(pidx <= 0)
                        {
                            qs.Remove("pageIndex");
                            if (qs.AllKeys.Contains("pageSize"))
                                qs.Remove("pageSize");
                        }
                    }
                    var nurl = string.Join("/", path.ToArray());
                    if(qs.Keys.Count > 0)
                    {
                        nurl += "?" + string.Join("&", qs.AllKeys.Select(o => string.Format("{0}={1}", o, qs[o])).ToArray());
                    }
                    return nurl;
                }
            }
            return url;
        }

        private static Uri MockUri(string href)
        {
            if (!href.StartsWith("/"))
                href = "/" + href;
            return new Uri(string.Format("http://localhost{0}", href), UriKind.Absolute);
        }
    }
}
