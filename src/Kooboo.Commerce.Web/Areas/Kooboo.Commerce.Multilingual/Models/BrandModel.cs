using Kooboo.Commerce.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Models
{
    public class BrandModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public BrandModel() { }

        public BrandModel(Brand brand)
        {
            Id = brand.Id;
            Name = brand.Name;
            Description = brand.Description;
        }
    }
}