using Kooboo.Commerce.Globalization;
using Kooboo.Commerce.Rules;
using System.Collections.Generic;

namespace Kooboo.Commerce.Brands
{
    public class Brand : ILocalizable
    {
        [Param]
        public int Id { get; set; }

        [Param]
        [Localizable]
        public string Name { get; set; }

        [Localizable]
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