using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Rebuild
{
    public interface IRebuildInfoManager
    {
        RebuildInfo Load(IndexKey indexKey);

        void Save(IndexKey indexKey, RebuildInfo info);
    }
}