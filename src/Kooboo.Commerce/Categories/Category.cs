using Kooboo.Commerce.Globalization;
using Kooboo.Commerce.Rules;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Kooboo.Commerce.Categories
{
    public class Category : ILocalizable
    { 
        [Param]
        public int Id { get; set; } 

        [Param]
        [Localizable]
        public string Name { get; set; } 

        [Localizable]
        public string Description { get; set; }

        /// <summary>
        /// Photo of this category, should be saved in the disk with the file path + name here. 
        /// For different size of photos,  use resize and cache. 
        /// </summary>
        public string Photo { get; set; }

        public int? ParentId { get; set; }

        [ScriptIgnore]
        public virtual Category Parent { get; set; }

        public virtual ICollection<Category> Children { get; set; }

        public virtual ICollection<CategoryCustomField> CustomFields { get; set; }
    }
}
