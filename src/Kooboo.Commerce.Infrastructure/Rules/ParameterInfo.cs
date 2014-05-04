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
    public class ParameterInfo
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
        /// So the container path of CustomerId parameter is: Order proprety -> Customer property
        /// </remark>
        public IEnumerable<MemberInfo> ContainerPath { get; private set; }

        public ParameterInfo(IConditionParameter parameter, IEnumerable<MemberInfo> containerPath)
        {
            Require.NotNull(parameter, "parameter");
            Require.NotNull(containerPath, "containerPath");

            Parameter = parameter;
            ContainerPath = containerPath.ToList();
        }

        /// <summary>
        /// Get the containing model value of the condition parameter from the root contextual model.
        /// </summary>
        public object GetValue(object rootModel)
        {
            var model = ResolveParameterContainer(rootModel);
            return Parameter.GetValue(model);
        }

        private object ResolveParameterContainer(object rootModel)
        {
            if (ContainerPath.Count() == 0)
            {
                return rootModel;
            }

            var container = rootModel;

            foreach (var path in ContainerPath)
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
