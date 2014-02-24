using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace Kooboo.Commerce.Handlers
{
    public class FileType
    {
        public FileType()
        {
        }

        public FileType(string code, string name, string regex, int limitSize, bool showPreview)
        {
            TypeCode = code;
            TypeName = name;
            RegexPattern = regex;
            LimitFileSize = limitSize;
            ShowPreview = showPreview;
        }

        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public string RegexPattern { get; set; }
        public int LimitFileSize { get; set; }
        public bool ShowPreview { get; set; }

        public static FileType[] CommonTypes
        {
            get
            {
                return new FileType[]
                    {
                        new FileType("images", "图片", @"(\.|\/)(gif|jpe?g|png)$", 5 * 1024 *1024, true),
                        new FileType("audio", "音频", @"(\.|\/)(mp3)$", 10 * 1024 *1024, false),
                        new FileType("video", "视频", @"(\.|\/)(mpe?g|mp4)$", 100 * 1024 *1024, true),
                        new FileType("flash", "Flash", @"(\.|\/)(flv|swf)$", 10 * 1024 *1024, false),
                        new FileType("files", "其他文件", @".+$", 100 * 1024 *1024, false)
                    };
            }
        }

        public static string GetFileType(string ext)
        {
            if (ext.IndexOf('.') >= 0)
            {
                ext = ext.Substring(ext.LastIndexOf('.') + 1);
            }
            switch(ext.ToLower())
            {
                case "gif": return "image/gif";
                case "png": return "image/png";
                case "jpg":
                case "jpeg": 
                    return "image/jpg";
                case "mp3": return "audio/mp3";
                case "mp4":
                case "mpeg":
                    return "video/mp4";
                case "flv":
                case "swf":
                    return "flash/gif";
            }
            return "file/" + ext;
        }

        public static string GetFriendlyFileSize(int fileSize)
        {
            if (fileSize > 1073741824)
                return string.Format("{0}MB", fileSize / 1048576);
            if (fileSize > 1048576)
                return string.Format("{0}MB", fileSize / 1048576);
            if (fileSize > 1024)
                return string.Format("{0}KB", fileSize / 1024);
            return string.Format("{0}B", fileSize);
        }

        public static bool IsImage(string fileName)
        {
            if (fileName.IndexOf('.') >= 0)
            {
                fileName = fileName.Substring(fileName.LastIndexOf('.') + 1);
            }
            fileName = fileName.ToLower();
            return new string[] { "gif", "jpg", "jpeg", "png" }.Contains(fileName);
        }
    }

    public static class FileTypeExtensions
    {
        public static FileType GetFileTypeByExtension(this IEnumerable<FileType> fileTypes, string extension)
        {
            foreach (var ft in fileTypes)
            {
                if (Regex.IsMatch(extension, ft.RegexPattern))
                    return ft;
            }
            return null;
        }
    }
}