using Kooboo.Commerce.Settings.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.AuthorizeNet
{
    public class AuthorizeNetConfig
    {
        [Required]
        [Display(Name = "Login ID")]
        public string LoginId { get; set; }

        [Required]
        [Display(Name = "Transaction key")]
        public string TransactionKey { get; set; }

        [Display(Name = "Sandbox")]
        public bool SandboxMode { get; set; }
    }
}