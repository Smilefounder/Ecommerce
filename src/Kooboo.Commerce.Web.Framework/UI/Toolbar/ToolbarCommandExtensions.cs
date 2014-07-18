using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Folders;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.UI.Toolbar
{
    public static class ToolbarCommandExtensions
    {
        static DataFile GetCommandConfigDataFile(this IToolbarCommand command)
        {
            return DataFolderFactory.Current.GetFile(
                UrlUtility.Combine(CommerceDataFolderVirtualPaths.Shared, "Toolbar/Commands", command.Name + ".config"), DataFileFormats.TypedJson);
        }

        public static object GetDefaultConfig(this IToolbarCommand command)
        {
            return GetCommandConfigDataFile(command).Read(command.ConfigType) ?? Activator.CreateInstance(command.ConfigType);
        }

        public static void SetDefaultCommandConfig(this IToolbarCommand command, object config)
        {
            GetCommandConfigDataFile(command).Write(config);
        }
    }
}
