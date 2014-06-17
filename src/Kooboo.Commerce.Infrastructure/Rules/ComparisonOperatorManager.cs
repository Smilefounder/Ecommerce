using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class ComparisonOperatorManager
    {
        private ConcurrentDictionary<string, IComparisonOperator> _operatorsByName 
            = new ConcurrentDictionary<string, IComparisonOperator>(StringComparer.OrdinalIgnoreCase);

        public IEnumerable<IComparisonOperator> Operators
        {
            get
            {
                return _operatorsByName.Values;
            }
        }

        public IComparisonOperator Find(string nameOrAlias)
        {
            Require.NotNullOrEmpty(nameOrAlias, "nameOrAlias");

            IComparisonOperator @operator;

            if (_operatorsByName.TryGetValue(nameOrAlias, out @operator))
            {
                return @operator;
            }

            return null;
        }

        public IEnumerable<string> AllOperatorNamesAndAlias()
        {
            var names = new List<string>();
            foreach (var op in Operators)
            {
                names.Add(op.Name);
                if (!String.IsNullOrWhiteSpace(op.Alias))
                {
                    names.Add(op.Alias);
                }
            }

            return names;
        }

        public void Register(IComparisonOperator @operator)
        {
            Require.NotNull(@operator, "operator");

            if (!_operatorsByName.TryAdd(@operator.Name, @operator))
            {
                throw new InvalidOperationException("A comparison operator with name '" + @operator.Name + "' already exists.");
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(@operator.Alias))
                {
                    if (!_operatorsByName.TryAdd(@operator.Alias, @operator))
                    {
                        IComparisonOperator temp;
                        _operatorsByName.TryRemove(@operator.Name, out temp);

                        throw new InvalidOperationException("Comparison operator alias '" + @operator.Alias + "' already exists.");
                    }
                }
            }
        }

        public bool Remove(string name)
        {
            Require.NotNullOrEmpty(name, "name");

            IComparisonOperator op;
            return _operatorsByName.TryRemove(name, out op);
        }

        public static readonly ComparisonOperatorManager Instance = new ComparisonOperatorManager();

        static ComparisonOperatorManager()
        {
            Instance.Register(ComparisonOperators.Equals);
            Instance.Register(ComparisonOperators.NotEquals);
            Instance.Register(ComparisonOperators.GreaterThan);
            Instance.Register(ComparisonOperators.GreaterThanOrEqual);
            Instance.Register(ComparisonOperators.LessThan);
            Instance.Register(ComparisonOperators.LessThanOrEqual);
            Instance.Register(ComparisonOperators.Contains);
            Instance.Register(ComparisonOperators.NotContains);
        }
    }
}
