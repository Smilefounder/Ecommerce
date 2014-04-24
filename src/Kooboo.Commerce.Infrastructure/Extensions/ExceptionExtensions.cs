using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    static class ExceptionExtensions
    {
        public static string Print(this Exception ex)
        {
            var result = new StringBuilder();

            Exception current = ex;

            while (current != null)
            {
                if (result.Length > 0)
                {
                    result.AppendLine("=================================");
                }

                result.AppendLine(current.Message);
                result.AppendLine(current.StackTrace);

                current = current.InnerException;
            }

            return result.ToString();
        }
    }
}
