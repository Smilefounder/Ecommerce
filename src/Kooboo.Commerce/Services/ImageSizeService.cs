using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Models;
using Kooboo.Web.Mvc.Paging;

namespace Kooboo.Commerce.Services
{

    /// <summary>
    /// managing the imagesize options.
    /// </summary>
    public class ImageSizeService : ServiceBase<ImageSize>
    {
        #region .ctor
        public static IList<ImageSize> _defaultlist;
        public ImageSizeService()
        {
            _defaultlist = new List<ImageSize>();
        }
        #endregion

        /// <summary>
        /// The reserved List of Image Size name. 
        /// <Thumbnail>used in product search, or other purpose.</Thumbnail>
        /// <Detail>used in the product detail display page.</Detail>
        /// <list>Used in the product list page. </list>
        /// <Cart>Used in the shopping cart thumbnail display.</Cart>
        /// </summary>
        public virtual IEnumerable<ImageSize> GetSystemDefaultList()
        {
            if (_defaultlist == null || _defaultlist.Count == 0)
            {
                _defaultlist.Add(new ImageSize() { Name = "Thumbnail", IsSystemDefault = true });
                _defaultlist.Add(new ImageSize() { Name = "List", IsSystemDefault = true });
                _defaultlist.Add(new ImageSize() { Name = "Detail", IsSystemDefault = true });
                _defaultlist.Add(new ImageSize() { Name = "Cart", IsSystemDefault = true });
            }
            return _defaultlist;
        }

        public override IPagedList<ImageSize> GetList(string shopUUID, int pageIndex = 1, int pageSize = 50)
        {
            return new PagedList<ImageSize>(GetSystemDefaultList(), pageIndex, 50, 50);
            //return base.GetList(shopUUID, pageIndex, pageSize);
        }

        /// <summary>
        /// The list of available imagesizes and their defined value. 
        /// </summary>
        /// <returns></returns>
        /// <TODO>Where to get the Commerce context to define which database to query from?</TODO>
        //public IEnumerable<ImageSize> getListOfImageSizes()
        //{

        //    //TODO: This should merger defaultlist defined above and the items from database. 
        //    return (List<ImageSize>)Kooboo.Fake.MethodHelper.GetDummy();

        //}

    }
}
