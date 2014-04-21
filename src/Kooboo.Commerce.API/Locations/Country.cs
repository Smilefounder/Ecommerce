using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Locations
{
    /// <summary>
    /// country
    /// </summary>
    public class Country : ItemResource
    {
        /// <summary>
        /// country id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// country name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// three letter iso code
        /// </summary>
        public string ThreeLetterISOCode { get; set; }
        /// <summary>
        /// two letter iso code
        /// </summary>
        public string TwoLetterISOCode { get; set; }
        /// <summary>
        /// numeric iso code
        /// </summary>
        public string NumericISOCode { get; set; }
    }
}
