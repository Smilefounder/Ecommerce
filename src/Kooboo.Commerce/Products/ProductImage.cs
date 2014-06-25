using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Commerce.Data.Events;
using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Products
{
    /// <summary>
    /// The real image for each product, different size of image can ge rendered from imagesizeservice.
    /// </summary>
    public class ProductImage
    {
        public int Id { get; set; }

        public string Size { get; set; }

        public string ImageUrl { get; set; }

        public Product Product { get; set; }

        #region Delete Orphan Product Images Handler

        class DeleteOrphanProductImageHandler : IHandle<SavingDbChanges>
        {
            public void Handle(SavingDbChanges @event)
            {
                var images = @event.DbContext.Set<ProductImage>();

                images.Where(i => i.Product == null)
                      .ToList()
                      .ForEach(i => images.Remove(i));
            }
        }

        #endregion
    }
}
