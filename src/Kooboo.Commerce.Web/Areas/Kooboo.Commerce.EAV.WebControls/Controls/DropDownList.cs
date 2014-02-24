using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.EAV.WebControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.EAV.WebControls
{
    [Dependency(typeof(IWebControl), Key = "Kooboo.Commerce.EAV.WebControls.DropDownList")]
    public class DropDownList : WebControlBase
    {
        public override string Name
        {
            get
            {
                return "DropDownList";
            }
        }
    }

    public class DropDownListData
    {
        public string Caption { get; set; }

        public IList<SelectListItem> Items { get; set; }

        public DropDownListData()
        {
            Items = new List<SelectListItem>();
        }

        public static DropDownListData Deserialize(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return new DropDownListData();
            }

            return JsonConvert.DeserializeObject<DropDownListData>(data);
        }
    }
}