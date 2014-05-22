using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class ChainedParameterValueResolver : IParameterValueResolver
    {
        private List<IParameterValueResolver> _resolvers;

        public ChainedParameterValueResolver()
        {
            _resolvers = new List<IParameterValueResolver>();
        }

        public ChainedParameterValueResolver(IEnumerable<IParameterValueResolver> resolvers)
        {
            _resolvers = resolvers.ToList();
        }

        public ChainedParameterValueResolver Chain(IParameterValueResolver resolver)
        {
            _resolvers.Add(resolver);
            return this;
        }

        public ChainedParameterValueResolver Clone()
        {
            return new ChainedParameterValueResolver(_resolvers);
        }

        public object GetValue(ConditionParameter param, object dataContext)
        {
            var value = dataContext;
            foreach (var resolver in _resolvers)
            {
                value = resolver.GetValue(param, value);
                if (value == null)
                {
                    break;
                }
            }

            return value;
        }
    }
}
