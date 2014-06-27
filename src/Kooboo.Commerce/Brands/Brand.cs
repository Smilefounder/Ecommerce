using Kooboo.Commerce.Rules;
using System.Collections.Generic;

namespace Kooboo.Commerce.Brands
{
    public class Brand
    {
        [Param]
        public int Id { get; set; }

        [Param]
        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// The logo file path.
        /// </summary>
        public string Logo { get; set; }

        public virtual ICollection<BrandCustomField> CustomFields { get; set; }

        public Brand()
        {
            CustomFields = new List<BrandCustomField>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}