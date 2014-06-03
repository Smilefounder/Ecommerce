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
        private string handlerPath = "/Handlers/";
        private string uploadPath = "/Uploads/";

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
        public string delete_url { get; set; }
        public string delete_type { get; set; }
        public string error { get; set; }

        public FilesStatus(string handlerPath, string uploadPath, string originalName, FileInfo fileInfo)
        {
            this.handlerPath = handlerPath;
            this.uploadPath = uploadPath;
            SetValues(originalName, fileInfo.Name, (int)fileInfo.Length, fileInfo.FullName);
        }

        public FilesStatus(string handlerPath, string uploadPath, string originalName, string fileName, int fileLength, string fullPath)
        {
            this.handlerPath = handlerPath;
            this.uploadPath = uploadPath;
            SetValues(originalName, fileName, fileLength, fullPath);
        }

        private void SetValues(string originalName, string fileName, int fileLength, string fullPath)
        {
            this.originalName = originalName;
            name = fileName;
            type = FileType.GetFileType(fileName);
            size = fileLength;
            friendly_size = FileType.GetFriendlyFileSize(fileLength);
            is_image = FileType.IsImage(fileName);
            progress = "1.0";
            url = UrlUtility.Combine(uploadPath, fileName); // handlerPath + "UploadHandler.ashx?f=" + fileName;
            delete_url = handlerPath + "UploadHandler.ashx?f=" + url;
            delete_type = "DELETE";

            var ext = Path.GetExtension(fullPath);

            var fileSize = ConvertBytesToMegabytes(new FileInfo(fullPath).Length);
            if (fileSize > 3 || !FileType.IsImage(ext)) thumbnail_url = "/Content/images/generalFile.png";
            else thumbnail_url = @"data:image/png;base64," + EncodeFile(fullPath);
        }

        private string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
        }

        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
    }
}