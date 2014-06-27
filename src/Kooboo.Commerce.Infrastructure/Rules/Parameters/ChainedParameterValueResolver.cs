using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parameters
{
    /// <summary>
    /// 表示支持链状求值的参数值求解器，链中的每个求解器依次执行，并将求得结果作为下一个求解器的输出，最终返回最后一个求解器的结果。
    /// </summary>
    public class ChainedParameterValueResolver : ParameterValueResolver
    {
        private List<ParameterValueResolver> _resolvers;

        public ChainedParameterValueResolver()
        {
            _resolvers = new List<ParameterValueResolver>();
        }

        public ChainedParameterValueResolver(IEnumerable<ParameterValueResolver> resolvers)
        {
            _resolvers = resolvers.ToList();
        }

        /// <summary>
        /// Add an instance of <see cref="Kooboo.Commerce.Rules.ParameterValueResolver"/> to the resolver chain.
        /// </summary>
        public ChainedParameterValueResolver Add(ParameterValueResolver resolver)
        {
            _resolvers.Add(resolver);
            return this;
        }

        public override object ResolveValue(ConditionParameter param, object dataContext)
        {
            var value = dataContext;
            foreach (var resolver in _resolvers)
            {
                value = resolver.ResolveValue(param, value);
                if (value == null)
                {
                    break;
                }
            }

            return value;
        }
    }
}
