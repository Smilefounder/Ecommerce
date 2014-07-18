using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Folders;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Tabs.Queries
{
    public class SavedTabQueryManager
    {
        private DataFolder _folder;

        public SavedTabQueryManager()
            : this(DataFolderFactory.Current.GetFolder(UrlUtility.Combine(CommerceDataFolderVirtualPaths.Shared, "TabQueries"), DataFileFormats.TypedJson))
        {
        }

        public SavedTabQueryManager(DataFolder folder)
        {
            _folder = folder;
        }

        public IEnumerable<SavedTabQuery> FindAll(string pageName)
        {
            var folder = _folder.GetFolder(pageName, DataFileFormats.TypedJson);
            return folder.GetFiles("*.config").Select(f => f.Read<SavedTabQuery>());
        }

        public SavedTabQuery Find(string pageName, Guid id)
        {
            var folder = _folder.GetFolder(pageName, DataFileFormats.TypedJson);
            return folder.GetFile(id.ToString() + ".config").Read<SavedTabQuery>();
        }

        public SavedTabQuery Add(string pageName, SavedTabQuery query)
        {
            var folder = _folder.GetFolder(pageName, DataFileFormats.TypedJson);
            folder.GetFile(query.Id.ToString() + ".config").Write(query);
            return query;
        }

        public SavedTabQuery Update(string pageName, SavedTabQuery query)
        {
            var file = _folder.GetFolder(pageName, DataFileFormats.TypedJson).GetFile(query.Id.ToString() + ".config");
            var savedQuery = file.Read<SavedTabQuery>();
            savedQuery.DisplayName = query.DisplayName;
            savedQuery.Order = query.Order;
            savedQuery.Config = query.Config;

            file.Write(savedQuery);

            return savedQuery;
        }

        public void Delete(string pageName, Guid id)
        {
            var folder = _folder.GetFolder(pageName);
            folder.GetFile(id.ToString() + ".config").Delete();
        }
    }
}
