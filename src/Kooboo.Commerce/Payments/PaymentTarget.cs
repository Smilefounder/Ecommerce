using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    /// <summary>
    /// Represents a reference to the target object a payment is applied to.
    /// </summary>
    [ComplexType]
    public class PaymentTarget
    {
        /// <summary>
        /// The identity key of the target object. For example, it could be the id value of an order.
        /// </summary>
        [StringLength(100)]
        public string Id { get; protected set; }

        /// <summary>
        /// The type of the target object. For example, use "Order" to indicates a payment applied to an order.
        /// </summary>
        [StringLength(100)]
        public string Type { get; protected set; }

        protected PaymentTarget()
        {
        }

        public PaymentTarget(string id, string type)
        {
            Require.NotNullOrEmpty(id, "id");
            Require.NotNullOrEmpty(type, "type");

            Id = id;
            Type = type;
        }

        public override bool Equals(object obj)
        {
            var other = obj as PaymentTarget;
            return other != null && Id.Equals(other.Id) && Type.Equals(other.Type);
        }

        public override int GetHashCode()
        {
            return (Id.GetHashCode() * 397) ^ Type.GetHashCode();
        }
    }
}
