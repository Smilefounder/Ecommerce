using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API
{
    public interface IListResource<T> : IResource, IList<T>
        where T : IItemResource
    {
    }

    public class ListResource<T> : List<T>, IListResource<T>
        where T : IItemResource
    {
        private List<Link> _links = new List<Link>();

        public IList<Link> Links
        {
            get
            {
                return _links;
            }
        }

        public ListResource() { }

        public ListResource(IEnumerable<T> items)
            : base(items)
        {
        }
    }
}
