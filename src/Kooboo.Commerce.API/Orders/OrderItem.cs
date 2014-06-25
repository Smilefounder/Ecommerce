using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.Products;

namespace Kooboo.Commerce.API.Orders
{
    /// <summary>
    /// order item
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// order item id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// order id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// The item that user really buy.
        /// A product can be sell in various prices according to it's variants, such as color, memory size, screen size etc.
        /// </summary>
        public int ProductPriceId { get; set; }

        /// <summary>
        /// redundant for query.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// sku no.
        /// </summary>
        public string SKU { get; set; }
        /// <summary>
        /// quantity of the item
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// price per unit
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// subtotal price
        /// </summary>
        public decimal SubTotal { get; set; }
        /// <summary>
        /// discount, usually from promotion
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// tax of this product
        /// </summary>
        public decimal TaxCost { get; set; }
        /// <summary>
        /// total price=subtotal - discount + tax
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// order
        /// </summary>
        public Order Order { get; set; }
        /// <summary>
        /// corresponding product price, which is various by the product variants.
        /// </summary>
        public ProductPrice ProductPrice { get; set; }
    }
}
