using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Stores;
using Kooboo.Commerce.Web.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Stores
{
    [Grid(IdProperty = "Id", Checkable = true)]
    public class StoreRowModel
    {
        public int Id { get; set; }

        [GridColumn(GridItemColumnType = typeof (EditGridActionItemColumn))]
        public string Name { get; set; }

        public StoreRowModel()
        {
        }

        public StoreRowModel(Store store)
        {
            Id = store.Id;
            Name = store.Name;
        }
    }
}