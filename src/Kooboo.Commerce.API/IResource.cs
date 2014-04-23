using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API
{
    public interface IResource
    {
        [JsonIgnore]
        IList<Link> Links { get; }
    }

    public interface IItemResource : IResource
    {
    }

    public abstract class ItemResource : IItemResource
    {
        public IList<Link> Links { get; set; }

        public ItemResource()
        {
            Links = new List<Link>();
        }
    }
}
