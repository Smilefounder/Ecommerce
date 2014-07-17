using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web.Framework.UI.Toolbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Common.Toolbar
{
    static class ToolbarCommandHelper
    {
        public static IEnumerable<ToolbarCommandResult> SafeExecute(IToolbarCommand command, object config, IEnumerable<object> dataItems, CommerceInstance instance)
        {
            var results = new List<ToolbarCommandResult>();
            foreach (var item in dataItems)
            {
                try
                {
                    var result = command.Execute(item, config, instance);
                    if (result != null)
                    {
                        results.Add(result);
                    }
                }
                catch (Exception ex)
                {
                    var result = new ToolbarCommandResult();
                    result.Success = false;
                    result.Messages.Add(ex.Message);
                    results.Add(result);
                }
            }

            return results;
        }
    }
}