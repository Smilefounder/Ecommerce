using Kooboo.Commerce.Data.Folders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Folders
{
    public static class DataFolders
    {
        static readonly Lazy<DataFolder> _root;

        static DataFolders()
        {
            _root = new Lazy<DataFolder>(() => DataFolderFactory.Current.GetFolder("/Commerce_Data", DataFileFormats.Json));
        }

        public static DataFolder Root
        {
            get
            {
                return _root.Value;
            }
        }

        public static DataFolder Shared
        {
            get
            {
                return Root.GetFolder("Shared");
            }
        }

        public static DataFolder Instances
        {
            get
            {
                return Root.GetFolder("Instances");
            }
        }
    }
}
