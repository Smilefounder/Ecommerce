using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Conditions.Operators
{
    public class ComparisonOperatorCollection : IEnumerable<IComparisonOperator>
    {
        private List<IComparisonOperator> _operators = new List<IComparisonOperator>();
        private Dictionary<string, IComparisonOperator> _operatorsByNameOrAlias = new Dictionary<string, IComparisonOperator>();

        public int Count
        {
            get
            {
                return _operators.Count;
            }
        }

        public IEnumerable<string> NamesAndAlias
        {
            get
            {
                return _operatorsByNameOrAlias.Keys;
            }
        }

        public ComparisonOperatorCollection() { }

        public IComparisonOperator Find(string nameOrAlias)
        {
            Require.NotNullOrEmpty(nameOrAlias, "nameOrAlias");

            IComparisonOperator @operator;

            if (_operatorsByNameOrAlias.TryGetValue(nameOrAlias, out @operator))
            {
                return @operator;
            }

            return null;
        }

        public void Add(IComparisonOperator @operator)
        {
            Require.NotNull(@operator, "operator");

            if (_operatorsByNameOrAlias.ContainsKey(@operator.Name))
                throw new InvalidOperationException("A comparison operator with a same name '" + @operator.Name + "' already exists.");

            if (!String.IsNullOrEmpty(@operator.Alias) && _operatorsByNameOrAlias.ContainsKey(@operator.Alias))
                throw new InvalidOperationException("A comparison operator with a same name/alias '" + @operator.Alias + "' already exists.");

            _operators.Add(@operator);
            _operatorsByNameOrAlias.Add(@operator.Name, @operator);

            if (!String.IsNullOrEmpty(@operator.Alias))
            {
                _operatorsByNameOrAlias.Add(@operator.Alias, @operator);
            }
        }

        public bool Remove(string nameOrAlias)
        {
            Require.NotNullOrEmpty(nameOrAlias, "nameOrAlias");

            var @operator = Find(nameOrAlias);
            if (@operator != null)
            {
                _operators.Remove(@operator);
                _operatorsByNameOrAlias.Remove(@operator.Name);

                if (!String.IsNullOrEmpty(@operator.Alias))
                {
                    _operatorsByNameOrAlias.Remove(@operator.Alias);
                }

                return true;
            }

            return false;
        }

        public IEnumerator<IComparisonOperator> GetEnumerator()
        {
            return _operators.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
