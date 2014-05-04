using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    /// <summary>
    /// Adapter to adapt a context model into the model that can be understood by the parameter.
    /// </summary>
    public interface IContextModelAdapter
    {
        /// <summary>
        /// Adapte the root context model into the model that can be understood by the parameter.
        /// </summary>
        /// <remarks>
        /// For example, we have CustomerCreated event:
        /// <code>
        ///     public class CustomerCreated {
        ///         public int CustomerId { get; set; }
        ///     }
        /// </code>
        /// We want to use CustomerGender parameter for this event, but CustomerGender parameter only understands Customer.
        /// So we can create an adapter to adapt CustomerCreated into Customer, so we can reuse CustomerGender parameter for this event context.
        /// </remarks>
        object AdaptModel(object rootContextModel);
    }

    public class NestedModelAdapter : IContextModelAdapter
    {
        private List<MemberInfo> _modelPathFromRoot;

        public NestedModelAdapter(IEnumerable<MemberInfo> modelPathFromRoot)
        {
            _modelPathFromRoot = modelPathFromRoot.ToList();
        }

        public object AdaptModel(object rootContextModel)
        {
            if (_modelPathFromRoot.Count == 0)
            {
                return rootContextModel;
            }

            var container = rootContextModel;

            foreach (var path in _modelPathFromRoot)
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
