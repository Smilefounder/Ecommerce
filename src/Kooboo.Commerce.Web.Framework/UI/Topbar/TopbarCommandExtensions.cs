using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Folders;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.UI.Topbar
{
    public static class TopbarCommandExtensions
    {
        static DataFile GetCommandConfigDataFile(this ITopbarCommand command)
        {
            return DataFolders.Shared.GetFolder("Topbar/Commands").GetFile(command.Name + ".config", DataFileFormats.TypedJson);
        }

        public static object GetDefaultConfig(this ITopbarCommand command)
        {
            return GetCommandConfigDataFile(command).Read(command.ConfigType) ?? Activator.CreateInstance(command.ConfigType);
        }

        public static void UpdateDefaultCommandConfig(this ITopbarCommand command, object config)
        {
            GetCommandConfigDataFile(command).Write(config);
        }
    }
}
