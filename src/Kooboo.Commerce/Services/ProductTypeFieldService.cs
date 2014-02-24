using Kooboo.Commerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Services
{
  public  class ProductTypeFieldService
    {

      private static IList<ProductTypeField> _systemDefaultFields;


      public ProductTypeFieldService()
      {
          _systemDefaultFields = new List<ProductTypeField>();
      }

      /// <TODO>This list value should comes from configuration xml file, instead of hard code here.</TODO>
      private static IEnumerable<ProductTypeField> getSystemDefaultList()
      {
          if (_systemDefaultFields == null || _systemDefaultFields.Count == 0)
          {
              _systemDefaultFields.Add(new ProductTypeField() { Name = "Summary", WebControl="TextBox" });
              _systemDefaultFields.Add(new ProductTypeField() { Name = "Description", WebControl="TextArea" });
              _systemDefaultFields.Add(new ProductTypeField() { Name = "Weight" });
              _systemDefaultFields.Add(new ProductTypeField() { Name = "PackageSize" });
          }
          return _systemDefaultFields;
      }


      public IEnumerable<ProductTypeField> getFields(ProductType CurrentType)
      {
          //TODO: This should merger defaultlist defined above and the items from database. 
          return (List<ProductTypeField>)Kooboo.Fake.MethodHelper.GetDummy();
      }

    }
}
