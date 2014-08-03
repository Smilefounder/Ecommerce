using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Framework.UI.Form;
using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Search.Builders
{
    public static class ProductDocumentBuilder
    {
        public static Document Build(Product product, ProductType productType, CultureInfo culture)
        {
            var doc = new Document();
            doc.Add(new Field("Id", product.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

            var searchText = new StringBuilder();

            var productName = product.GetText("Name", culture) ?? product.Name;
            doc.Add(new Field("Name", productName, Field.Store.YES, Field.Index.ANALYZED));
            searchText.Append(productName);

            var brandName = product.Brand == null ? null : product.Brand.GetText("Name", culture) ?? product.Brand.Name;
            if (brandName != null)
            {
                doc.Add(new Field("Brand", brandName, Field.Store.YES, Field.Index.NOT_ANALYZED));
                searchText.Append(" ").Append(brandName);
            }

            foreach (var category in product.Categories)
            {
                var categoryName = category.Category.GetText("Name", culture) ?? category.Category.Name;
                doc.Add(new Field("Category", categoryName, Field.Store.YES, Field.Index.NOT_ANALYZED));
                searchText.Append(" ").Append(categoryName);
            }

            var controls = FormControls.Controls().ToList(); // TODO: Duplicate IsSelectionList in the ProductType?
            var variantFieldValues = new Dictionary<string, HashSet<string>>();

            foreach (var variant in product.Variants)
            {
                foreach (var fieldDef in productType.VariantFieldDefinitions)
                {
                    var variantField = variant.VariantFields.FirstOrDefault(f => f.FieldName == fieldDef.Name);
                    if (variantField != null)
                    {
                        string fieldValue = null;
                        var control = controls.Find(c => c.Name == fieldDef.ControlType);
                        if (control.IsSelectionList)
                        {
                            var item = fieldDef.SelectionItems.FirstOrDefault(i => i.Value == variantField.FieldValue);
                            if (item != null)
                            {
                                fieldValue = productType.GetText("VariantFieldDefinitions[" + fieldDef.Name + "].SelectionItems[" + item.Value + "]", culture) ?? item.Text;
                            }
                        }
                        else if (control.IsValuesPredefined)
                        {
                            fieldValue = productType.GetText("VariantFieldDefinitions[" + fieldDef.Name + "].DefaultValue", culture) ?? fieldDef.DefaultValue;
                        }
                        else
                        {
                            fieldValue = variant.GetText("VariantFields[" + fieldDef.Name + "]", culture) ?? variantField.FieldValue;
                        }

                        if (!String.IsNullOrEmpty(fieldValue))
                        {
                            if (variantFieldValues.ContainsKey(fieldDef.Name))
                            {
                                variantFieldValues[fieldDef.Name].Add(fieldValue);
                            }
                            else
                            {
                                variantFieldValues.Add(fieldDef.Name, new HashSet<string> { fieldValue });
                            }
                        }
                    }
                }
            }

            foreach (var each in variantFieldValues)
            {
                foreach (var value in each.Value)
                {
                    doc.Add(new Field(each.Key, value, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    searchText.Append(" ").Append(value);
                }
            }

            doc.Add(new Field("SearchText", searchText.ToString(), Field.Store.NO, Field.Index.ANALYZED));

            return doc;
        }
    }
}