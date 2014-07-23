using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;
using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.TabQueries
{
    public class TabQueryModel
    {
        public string PageName { get; set; }

        public List<SavedTabQuery> SavedQueries { get; set; }

        public List<ITabQuery> AvailableQueries { get; set; }

        public SavedTabQuery CurrentQuery { get; set; }

        public IPagedList CurrentQueryResult { get; set; }
    }
}