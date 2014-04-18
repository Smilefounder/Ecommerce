using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API
{
    public interface IResource
    {
        [JsonProperty("_links")]
        IList<Link> Links { get; }
    }

    public interface IItemResource : IResource
    {
    }

    public abstract class ItemResource : IItemResource
    {
        public IList<Link> Links { get; set; }

        protected ItemResource()
        {
            Links = new List<Link>();
        }
    }
}
