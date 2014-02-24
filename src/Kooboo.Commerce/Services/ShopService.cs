#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Commerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Services
{
    public class ShopService
    {
        #region GetList
        public virtual IPagedList<Shop> GetList(int pageIndex = 1, int pageSize = 50)
        {
            return new Kooboo.Web.Mvc.Paging.PagedList<Shop>(GetDummy(), 1, 20);
        }
        private static List<Shop> GetDummy()
        {
            return (List<Shop>)Kooboo.Fake.MethodHelper.GetDummy();
        }
        #endregion

        #region GetObjectByUUID
        public virtual Shop GetObjectByUUID(string uuid)
        {
            return (Shop)Kooboo.Fake.MethodHelper.GetDummy();
        }
        #endregion

        #region Add
        public virtual void Add(Shop item)
        {
            Kooboo.Fake.MethodHelper.SetDummy(item);
        }
        #endregion

        #region Update
        public virtual void Update(Shop item)
        {
            Kooboo.Fake.MethodHelper.SetDummy(item);
        }
        #endregion

        #region Delete
        public virtual void Delete(Shop item)
        {

        }
        #endregion
    }
}
