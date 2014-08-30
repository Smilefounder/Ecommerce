using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Lucene.Net.Documents;
using Kooboo.Commerce.Products;
using System.Globalization;
using Kooboo.Commerce.Web.Framework.UI.Form;
using System.Text;

namespace Kooboo.Commerce.Search.Models
{
    public class ProductModel
    {
        [Key]
        [Field(Field.Index.NOT_ANALYZED)]
        public int Id { get; set; }

        [Field(Field.Index.ANALYZED)]
        public string Name { get; set; }

        [Field(Field.Index.NOT_ANALYZED)]
        public string Brand { get; set; }

        [Field(Field.Index.NOT_ANALYZED)]
        public IList<string> Categories { get; set; }

        [Field(Field.Index.NOT_ANALYZED, Numeric = true)]
        public decimal LowestPrice { get; set; }

        [Field(Field.Index.NOT_ANALYZED, Numeric = true)]
        public decimal HighestPrice { get; set; }

        [Field(Field.Index.NOT_ANALYZED, Numeric = true)]
        public IList<decimal> Prices { get; set; }

        [Field(Field.Index.NOT_ANALYZED)]
        public IDictionary<string, HashSet<string>> VariantFieldValues { get; set; }

        [Field(Field.Index.ANALYZED, Field.Store.NO)]
        public string SearchText { get; set; }

        public ProductModel()
        {
            Categories = new List<string>();
            Prices = new List<decimal>();
            VariantFieldValues = new Dictionary<string, HashSet<string>>();
        }

        public static ProductModel Create(Product product, CultureInfo culture)
        {
            var doc = new ProductModel
            {
                Id = product.Id,
                Name = product.GetText("Name", culture) ?? product.Name
            };

            if (product.Brand != null)
            {
                doc.Brand = product.Brand.GetText("Name", culture) ?? product.Brand.Name;
            }

            foreach (var category in product.Categories)
            {
                doc.Categories.Add(category.GetText("Name", culture) ?? category.Name);
            }
            foreach (var variant in product.Variants)
            {
                doc.Prices.Add(variant.Price);
            }

            if (doc.Prices.Count > 0)
            {
                doc.LowestPrice = doc.Prices.Min();
                doc.HighestPrice = doc.Prices.Max();
            }

            var controls = FormControls.Controls().ToList();

            foreach (var variant in product.Variants)
            {
                foreach (var fieldDef in product.ProductType.VariantFieldDefinitions)
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
                                fieldValue = product.ProductType.GetText("VariantFieldDefinitions[" + fieldDef.Name + "].SelectionItems[" + item.Value + "]", culture) ?? item.Text;
                            }
                        }
                        else if (control.IsValuesPredefined)
                        {
                            fieldValue = product.ProductType.GetText("VariantFieldDefinitions[" + fieldDef.Name + "].DefaultValue", culture) ?? fieldDef.DefaultValue;
                        }
                        else
                        {
                            fieldValue = variant.GetText("VariantFields[" + fieldDef.Name + "]", culture) ?? variantField.FieldValue;
                        }

                        if (!String.IsNullOrEmpty(fieldValue))
                        {
                            if (doc.VariantFieldValues.ContainsKey(fieldDef.Name))
                            {
                                doc.VariantFieldValues[fieldDef.Name].Add(fieldValue);
                            }
                            else
                            {
                                doc.VariantFieldValues.Add(fieldDef.Name, new HashSet<string> { fieldValue });
                            }
                        }
                    }
                }
            }

            // Build search text
            var searchText = new StringBuilder();
            searchText.Append(doc.Name);
            searchText.Append(" ").Append(doc.Brand);
            searchText.Append(" ").Append(String.Join(" ", doc.Categories));

            foreach (var each in doc.VariantFieldValues)
            {
                searchText.Append(" ").Append(String.Join(" ", each.Value));
            }

            doc.SearchText = searchText.ToString();

            return doc;
        }
    }
}