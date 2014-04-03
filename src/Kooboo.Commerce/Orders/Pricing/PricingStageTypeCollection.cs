using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing
{
    public class PricingStageTypeCollection : IEnumerable<Type>
    {
        private List<Type> _types = new List<Type>();

        public Type this[int index]
        {
            get
            {
                lock (_types)
                {
                    return _types[index];
                }
            }
        }

        public PricingStageTypeCollection() { }

        public PricingStageTypeCollection(IEnumerable<Type> stageTypes)
        {
            _types = new List<Type>(stageTypes);
        }

        public void Add<TStage>()
            where TStage : IPricingStage
        {
            lock (_types)
            {
                _types.Add(typeof(TStage));
            }
        }

        public void Insert<TStage>(int index)
        {
            lock (_types)
            {
                _types.Insert(index, typeof(TStage));
            }
        }

        public void Remove<TStage>()
        {
            lock (_types)
            {
                var type = _types.FirstOrDefault(x => x == typeof(TStage));
                if (type != null)
                {
                    _types.Remove(type);
                }
            }
        }

        public IEnumerator<Type> GetEnumerator()
        {
            lock (_types)
            {
                var types = _types.ToList();
                foreach (var type in types)
                {
                    yield return type;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
