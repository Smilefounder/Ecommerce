using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce
{
    public static class Require
    {
        public static void NotNull(object obj, string paramName)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName);
        }

        public static void NotNull(object obj, string paramName, string message)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName, message);
        }

        public static void NotNullOrEmpty(string str, string paramName)
        {
            if (String.IsNullOrEmpty(str))
                throw new ArgumentException("Value is required.", paramName);
        }

        public static void NotNullOrEmpty(string str, string paramName, string message)
        {
            if (String.IsNullOrEmpty(str))
                throw new ArgumentException(message, paramName);
        }

        public static void That(bool condition, string paramName)
        {
            if (!condition)
                throw new ArgumentException("Precondition is not met.", paramName);
        }

        public static void That(bool condition, string paramName, string message)
        {
            if (!condition)
                throw new ArgumentException(message, paramName);
        }
    }
}
