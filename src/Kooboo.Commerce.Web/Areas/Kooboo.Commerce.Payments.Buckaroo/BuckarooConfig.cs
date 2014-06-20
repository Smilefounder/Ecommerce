using Kooboo.Commerce.Settings.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.Buckaroo
{
    public class BuckarooConfig
    {
        [Required]
        [Display(Name = "Website key")]
        public string WebsiteKey { get; set; }

        [Required]
        [Display(Name = "Secret key")]
        public string SecretKey { get; set; }

        [Display(Name = "Credit debit mandate reference")]
        public string CreditDebitMandateReference { get; set; }

        [Display(Name = "Credit debit mandate date")]
        public string CreditDebitMandateDate { get; set; }

        [Display(Name = "Test mode")]
        public bool TestMode { get; set; }
    }
}