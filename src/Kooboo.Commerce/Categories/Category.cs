using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Kooboo.Commerce.Categories
{
    public class Category
    { 
        public int Id { get; set; } 

        public string Name { get; set; } 

        public string Description { get; set; }

        /// <summary>
        /// Photo of this category, should be saved in the disk with the file path + name here. 
        /// For different size of photos,  use resize and cache. 
        /// </summary>
        public string Photo { get; set; }

        public bool Published { get; set; }

        [ScriptIgnore]
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; }
    }
}
