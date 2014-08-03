using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Kooboo.Commerce.Search.CMSIntegration
{
    public class FacetsApi
    {
        private string _baseAddress;
        private string _instance;

        public FacetsApi(string host, string instance)
        {
            _baseAddress = host;
            _instance = instance;
        }

        public IEnumerable<FieldFacet> Facets(string culture)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = client.GetAsync("api/" + _instance + "/facets?culture=" + culture).Result;
                result.EnsureSuccessStatusCode();

                return result.Content.ReadAsAsync<List<FieldFacet>>().Result;
            }
        }
    }
}
