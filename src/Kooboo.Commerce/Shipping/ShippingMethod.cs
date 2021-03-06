﻿using Kooboo.Commerce.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping
{
    public class ShippingMethod
    {
        [Param]
        public int Id { get; set; }

        [Required, Param, StringLength(50)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool IsEnabled { get; set; }

        [Required, StringLength(100)]
        public string ShippingRateProviderName { get; set; }

        private string ShippingRateProviderConfig { get; set; }

        public T LoadShippingRateProviderConfig<T>()
            where T : class
        {
            return LoadShippingRateProviderConfig(typeof(T)) as T;
        }

        public object LoadShippingRateProviderConfig(Type configModelType)
        {
            if (String.IsNullOrWhiteSpace(ShippingRateProviderConfig))
            {
                return null;
            }

            return JsonConvert.DeserializeObject(ShippingRateProviderConfig, configModelType);
        }

        public void UpdateShippingRateProviderConfig(object configModel)
        {
            if (configModel == null)
            {
                ShippingRateProviderConfig = null;
            }
            else
            {
                ShippingRateProviderConfig = JsonConvert.SerializeObject(configModel);
            }
        }

        #region Entity Mapping

        class ShippingMethodMap : EntityTypeConfiguration<ShippingMethod>
        {
            public ShippingMethodMap()
            {
                Property(c => c.ShippingRateProviderConfig);
            }
        }

        #endregion
    }
}
