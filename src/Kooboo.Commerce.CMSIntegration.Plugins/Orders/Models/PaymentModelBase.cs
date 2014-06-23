using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Orders.Models
{
    public class PaymentModelBase
    {
        public int OrderId { get; set; }

        public int PaymentMethodId { get; set; }

        public string ReturnUrl { get; set; }

        protected Dictionary<string, string> PaymentParameters { get; private set; }

        public PaymentModelBase()
        {
            PaymentParameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public IDictionary<string, string> GetPaymentParameters()
        {
            return PaymentParameters;
        }

        public string GetPaymentParameterValue(string key)
        {
            string value;

            if (PaymentParameters.TryGetValue(key, out value))
            {
                return value;
            }

            return null;
        }

        public T GetPaymentParameterValue<T>(string key, T defaultValue = default(T))
        {
            var value = GetPaymentParameterValue(key);
            if (value == null)
            {
                return defaultValue;
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)value;
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

        public void SetPaymentParameterValue(string key, object value)
        {
            string strValue = null;

            if (value != null)
            {
                if (value is string)
                {
                    strValue = value as string;
                }
                else
                {
                    strValue = value.ToString();
                }
            }

            if (PaymentParameters.ContainsKey(key))
            {
                PaymentParameters[key] = strValue;
            }
            else
            {
                PaymentParameters.Add(key, strValue);
            }
        }
    }
}
