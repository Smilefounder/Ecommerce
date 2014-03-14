using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    /// <summary>
    /// Represents the condition parameter and its containing model info.
    /// </summary>
    public class ConditionParameterInfo
    {
        public IConditionParameter Parameter { get; private set; }

        /// <summary>
        /// The path of the parameter's model from root context model.
        /// </summary>
        /// <remarks>
        /// For example, if the root context model is:
        /// <code>
        ///     public class OrderCreated {
        ///         public Order Order { get; set; }
        ///     }
        ///     
        ///     public class Order {
        ///         public Customer Customer { get; set; }
        ///     }
        /// </code>
        /// The the model of CustomerId parameter is Customer class.
        /// The container of CustomerId parameter is the Customer property of Order class.
        /// So the model path of CustomerId parameter is: Order proprety -> Customer property
        /// </remark>
        public IEnumerable<MemberInfo> ModelPath { get; private set; }

        public ConditionParameterInfo(IConditionParameter parameter, IEnumerable<MemberInfo> modelPath)
        {
            Require.NotNull(parameter, "parameter");
            Require.NotNull(modelPath, "modelPath");

            Parameter = parameter;
            ModelPath = modelPath.ToList();
        }

        /// <summary>
        /// Get the containing model value of the condition parameter from the root contextual model.
        /// </summary>
        public object GetValue(object rootModel)
        {
            var model = ResolveModel(rootModel);
            return Parameter.GetValue(model);
        }

        private object ResolveModel(object rootModel)
        {
            if (ModelPath.Count() == 0)
            {
                return rootModel;
            }

            var container = rootModel;

            foreach (var path in ModelPath)
            {
                if (path is FieldInfo)
                {
                    container = ((FieldInfo)path).GetValue(container);
                }
                else if (path is PropertyInfo)
                {
                    container = ((PropertyInfo)path).GetValue(container, null);
                }
            }

            return container;
        }
    }
}
