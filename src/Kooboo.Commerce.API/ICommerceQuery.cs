﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API
{
    public interface ICommerceQuery<T>
    {
        T[] Pagination(int pageIndex, int pageSize);
        T FirstOrDefault();
        T[] ToArray();
        int Count();
    }
}