using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Kooboo.Commerce.Payments.iDeal
{
    public class IDealConfig
    {
        [Display(Name = "Partner ID")]
        public string PartnerId { get; set; }

        [Display(Name = "Test mode")]
        public bool TestMode { get; set; }
    }
}