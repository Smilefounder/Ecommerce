using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Commerce.ComponentModel;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Brands;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Brands
{
    public class Brand : INotifyObjectCreated
    {
        [Parameter(Name = "BrandId", DisplayName = "Brand ID")]
        public int Id { get; set; }

        [Parameter(Name = "BrandName", DisplayName = "Brand Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// The logo file path.
        /// </summary>
        public string Logo { get; set; }

        public virtual ICollection<BrandCustomField> CustomFields { get; set; }

        public override string ToString()
        {
            return Name;
        }

        void INotifyObjectCreated.NotifyCreated()
        {
            Event.Raise(new BrandCreated(this));
        }
    }
}