using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Commerce.Stores;
using Kooboo.Web.Mvc.Grid2.Design;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Stores
{
    [Grid]
    public class StoreEditorModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        public StoreEditorModel()
        {
        }

        public StoreEditorModel(Store store)
        {
            Id = store.Id;
            Name = store.Name;
        }

        public void UpdateTo(Store store)
        {
            store.Name = Name.Trim();
        }
    }
}