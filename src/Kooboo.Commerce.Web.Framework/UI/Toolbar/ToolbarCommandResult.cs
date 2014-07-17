using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.UI.Toolbar
{
    public class ToolbarCommandResult
    {
        public bool Success { get; set; }

        public IList<string> Messages { get; set; }

        public ToolbarCommandResult()
        {
            Success = true;
            Messages = new List<string>();
        }

        public static ToolbarCommandResult Succeeded()
        {
            return new ToolbarCommandResult
            {
                Success = true
            };
        }

        public static ToolbarCommandResult Failed()
        {
            return new ToolbarCommandResult
            {
                Success = false
            };
        }

        public ToolbarCommandResult WithMessage(string message)
        {
            Messages.Add(message);
            return this;
        }
    }
}
