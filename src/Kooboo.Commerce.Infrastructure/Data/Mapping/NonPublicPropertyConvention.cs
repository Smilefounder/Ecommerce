using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Data.Mapping
{
    public class NonPublicPropertyConvention : Convention
    {
        public NonPublicPropertyConvention()
        {
            Types().Having(NonPublicProperties)
                   .Configure((config, props) =>
                   {
                       foreach (var prop in props)
                       {
                           config.Property(prop);
                       }
                   });

            Types().Having(NonPublicKeys)
                   .Configure((config, props) =>
                   {
                       config.HasKey(props.Select(p => p.Name));
                   });
        }

        private IEnumerable<PropertyInfo> NonPublicKeys(Type type)
        {
            return GetProperties(type, typeof(KeyAttribute));
        }

        private IEnumerable<PropertyInfo> NonPublicProperties(Type type)
        {
            return GetProperties(type, typeof(ColumnAttribute));
        }

        private IEnumerable<PropertyInfo> GetProperties(Type type, Type attributeType)
        {
            var matchingProperties = type.GetProperties(BindingFlags.SetProperty | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance)
                                         .Where(propInfo => propInfo.GetCustomAttributes(attributeType, true).Length > 0)
                                         .ToArray();
            return matchingProperties.Length == 0 ? null : matchingProperties;
        }
    }
}
