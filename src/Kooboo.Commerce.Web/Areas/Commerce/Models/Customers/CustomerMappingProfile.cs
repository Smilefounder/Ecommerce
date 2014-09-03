using AutoMapper;
using Kooboo.Commerce.Countries;
using Kooboo.Commerce.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Customers
{
    class CustomerMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<CustomerCustomField, NameValue>();
            Mapper.CreateMap<Address, AddressModel>();
            Mapper.CreateMap<Customer, CustomerEditorModel>()
                  .AfterMap((customer, model) =>
                  {
                      if (customer.DefaultShippingAddressId != null)
                      {
                          var address = model.Addresses.FirstOrDefault(it => it.Id == customer.DefaultShippingAddressId.Value);
                          if (address != null)
                          {
                              address.IsDefaultShippingAddress = true;
                          }
                      }
                      if (customer.DefaultBillingAddressId != null)
                      {
                          var address = model.Addresses.FirstOrDefault(it => it.Id == customer.DefaultBillingAddressId.Value);
                          if (address != null)
                          {
                              address.IsDefaultBillingAddress = true;
                          }
                      }
                  });
        }
    }
}