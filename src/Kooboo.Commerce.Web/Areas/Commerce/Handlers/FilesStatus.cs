using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Kooboo.Web.Url;

namespace Kooboo.Commerce.Handlers
{
    public class FilesStatus
    {
        public string group { get; set; }
        public string originalName { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public bool is_image { get; set; }
        public int size { get; set; }

        public string friendly_size { get; set; }
        public string progress { get; set; }
        public string url { get; set; }
        public string thumbnail_url { get; set; }
        public string error { get; set; }

        public FilesStatus(string originalName, string fileName, int fileLength, string folder)
        {
            this.originalName = originalName;
            name = fileName;
            type = FileType.GetFileType(fileName);
            size = fileLength;
            friendly_size = FileType.GetFriendlyFileSize(fileLength);
            is_image = FileType.IsImage(fileName);
            progress = "1.0";
            url = UrlUtility.Combine(folder, fileName);
            thumbnail_url = "/Content/images/generalFile.png";
        }
    }
}