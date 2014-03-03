using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Payments.Buckaroo
{
    public static class BuckarooUtil
    {
        public static string GetGatewayUrl(bool testMode)
        {
            return String.Format("https://{0}checkout.buckaroo.nl/html/", testMode ? "test" : String.Empty);
        }


        public static string GetSignature(NameValueCollection parameters, string secretKey)
        {
            var builder = new StringBuilder();

            foreach (var key in parameters.AllKeys.OrderBy(it => it))
            {
                if (key.ToLower() != "brq_signature")//except brq_signature
                    builder.AppendFormat("{0}={1}", key, parameters[key]);
            }

            builder.Append(secretKey);

            return SHA1(builder.ToString());
        }

        public static string SHA1(string parameters)
        {
            var sha1Provider = new SHA1CryptoServiceProvider();
            //convert input string to a byte array
            byte[] messageArray = Encoding.UTF8.GetBytes(parameters);
            //calculate hash over the byte array
            byte[] hash1 = sha1Provider.ComputeHash(messageArray);

            var builder = new StringBuilder();

            //convert each byte in the hash to hexadecimal format
            foreach (byte b in hash1)
            {
                builder.Append(b.ToString("x2"));
            }

            //retrieve the result from the stringbuilder
            return builder.ToString();
        }
    }
}