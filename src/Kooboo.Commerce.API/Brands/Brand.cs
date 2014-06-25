using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Brands
{
    /// <summary>
    /// the brand api object
    /// </summary>
    public class Brand
    {
        /// <summary>
        /// brand id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// brand name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// brand description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The logo image file path of this brand.
        /// </summary>
        public string Logo { get; set; }
    }
}
