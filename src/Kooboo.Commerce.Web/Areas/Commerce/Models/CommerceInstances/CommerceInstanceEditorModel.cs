using Kooboo.Commerce.ComponentModel.DataAnnotations;
using Kooboo.Commerce.Data;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.CommerceInstances
{
    public class CommerceInstanceEditorModel
    {
        public bool IsNew { get; set; }

        [RequiredIf("IsNew", "true"), RegularExpression(@"^[a-zA-Z][\w_]*$", ErrorMessage = "Invalid name format")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Display name")]
        public string DisplayName { get; set; }

        [Display(Name = "Database Schema")]
        [RequiredIf("IsNew", "true"), RegularExpression(@"^[a-zA-Z][\w_]*$", ErrorMessage = "Invalid schema format")]
        public string DbSchema { get; set; }

        [Display(Name = "Database Provider")]
        public string DbProviderDisplayName { get; set; }

        [RequiredIf("IsNew", "true"), Display(Name = "Db Provider")]
        public string DbProviderKey { get; set; }

        [Display(Name = "Advanced mode")]
        public bool AdvancedMode { get; set; }

        [RequiredIf("AdvancedMode", "true")]
        public string ConnectionString { get; set; }

        public IList<SelectListItem> ConnectionStringParameters { get; set; }

        public IList<ConnectionStringParameterNames> ConnectionStringParameterNames { get; set; }

        public IList<SelectListItem> DbProviders { get; set; }

        public CommerceInstanceEditorModel()
        {
            IsNew = true;
            DbProviders = new List<SelectListItem>();
            ConnectionStringParameters = new List<SelectListItem>();
            ConnectionStringParameterNames = new List<ConnectionStringParameterNames>();
        }

        public void AddDbProvider(ICommerceDbProvider provider)
        {
            DbProviders.Add(new SelectListItem
            {
                Text = provider.DisplayName,
                Value = provider.InvariantName + "|" + provider.ManifestToken
            });

            ConnectionStringParameterNames.Add(new ConnectionStringParameterNames
            {
                DbProviderKey = provider.InvariantName + "|" + provider.ManifestToken,
                ParameterNames = provider.ConnectionStringBuilder.ParameterNames.ToList()
            });
        }
    }

    public class ConnectionStringParameterNames
    {
        public string DbProviderKey { get; set; }

        public IList<string> ParameterNames { get; set; }
    }
}