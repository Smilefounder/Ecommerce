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
    public abstract class ServiceBase<T>
        where T : IShopObject, new()
    {
        #region GetList
        public virtual IPagedList<T> GetList(string shopUUID, int pageIndex = 1, int pageSize = 50)
        {
            return new Kooboo.Web.Mvc.Paging.PagedList<T>(new[] { new T() }, 1, 20);
        }
        //private static List<T> GetDummy()
        //{
        //    return (List<T>)Kooboo.Fake.MethodHelper.GetDummy();
        //}
        #endregion

        #region GetObjectByUUID
        public virtual T GetObjectByUUID(string uuid)
        {
            return default(T);
            //return (T)Kooboo.Fake.MethodHelper.GetDummy();
        }
        #endregion

        #region Add
        public virtual void Add(T item)
        {
            Kooboo.Fake.MethodHelper.SetDummy(item);
        }
        #endregion

        #region Update
        public virtual void Update(T item)
        {
            Kooboo.Fake.MethodHelper.SetDummy(item);
        }
        #endregion

        #region Delete
        public virtual void Delete(T item)
        {

        }
        #endregion
    }
}
