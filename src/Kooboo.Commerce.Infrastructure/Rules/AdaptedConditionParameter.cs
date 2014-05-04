using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    /// <summary>
    /// An adapted version of a parameter to be used for a different context model.
    /// </remarks>
    public class AdaptedConditionParameter : IConditionParameter
    {
        private IConditionParameter _innerParameter;
        private IContextModelAdapter _modelAdapter;

        public string Name { get; set; }

        public string DisplayName { get; set; }

        // TODO: Change to class level attribute ?
        public Type ContextModelType { get; set; }

        public Type ValueType
        {
            get
            {
                return _innerParameter.ValueType;
            }
        }

        public IEnumerable<IComparisonOperator> SupportedOperators
        {
            get
            {
                return _innerParameter.SupportedOperators;
            }
        }

        public AdaptedConditionParameter(Type newModelType, IConditionParameter underlyingParameter, IContextModelAdapter modelAdapter)
        {
            _innerParameter = underlyingParameter;
            _modelAdapter = modelAdapter;
            Name = underlyingParameter.Name;
            DisplayName = underlyingParameter.DisplayName;
            ContextModelType = newModelType;
        }

        public object GetValue(object model)
        {
            Require.NotNull(model, "model");
            var underlyingModel = _modelAdapter.AdaptModel(model);
            return _innerParameter.GetValue(model);
        }

        public object ParseValue(string value)
        {
            return _innerParameter.ParseValue(value);
        }
    }
}
