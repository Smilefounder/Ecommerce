using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Configuration;

namespace Kooboo.Commerce.Handlers
{
    /// <summary>
    /// Summary description for UploadFile
    /// </summary>
    public class UploadHandler : IHttpHandler
    {
        private const string handlerPath = "/Handlers/";

        public UploadHandler()
        {
        }

        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            HandleMethod(context);
        }

        // Handle request based on method
        private void HandleMethod(HttpContext context)
        {
            string action = context.Request.QueryString["action"];
            switch (context.Request.HttpMethod)
            {
                case "HEAD":
                case "GET":
                    if (action == "DELETE")
                    {
                        DeleteFile(context);
                    }
                    else
                    {
                        if (GivenFilename(context)) DeliverFile(context);
                        else ListCurrentFiles(context);
                    }
                    break;

                case "POST":
                case "PUT":
                    UploadFile(context);
                    break;

                case "DELETE":
                    DeleteFile(context);
                    break;

                case "OPTIONS":
                    ReturnOptions(context);
                    break;

                default:
                    context.Response.ClearHeaders();
                    context.Response.StatusCode = 405;
                    break;
            }
        }

        private static void ReturnOptions(HttpContext context)
        {
            string verbs = "OPTIONS, HEAD, GET, POST, PUT, DELETE";
            context.Response.AddHeader("Allow", verbs);
            context.Response.AddHeader("Access-Control-Allow-Methods", verbs);
            context.Response.StatusCode = 200;
        }

        // Delete file from the server
        private void DeleteFile(HttpContext context)
        {
            var file = new FileInfo(context.Server.MapPath(context.Request["f"]));
            if (file != null && file.Exists)
            {
                file.Delete();
            }
        }

        // Upload file to the server
        private void UploadFile(HttpContext context)
        {
            var statuses = new List<FilesStatus>();
            var headers = context.Request.Headers;

            if (string.IsNullOrEmpty(headers["X-File-Name"]))
            {
                UploadWholeFile(context, statuses);
            }
            else
            {
                UploadPartialFile(headers["X-File-Name"], context, statuses);
            }

            WriteJsonIframeSafe(context, statuses);
        }

        // Upload partial file
        private void UploadPartialFile(string fileName, HttpContext context, List<FilesStatus> statuses)
        {
            if (context.Request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var inputStream = context.Request.Files[0].InputStream;

            string uploadPath = GetUploadPath(fileName, context);
            var file = GetRenamedFileIfExists(uploadPath + fileName, context);

            if (file != null)
            {
                if (!file.Directory.Exists)
                    file.Directory.Create();
                using (var fs = new FileStream(file.FullName, FileMode.Append, FileAccess.Write))
                {
                    var buffer = new byte[1024];

                    var l = inputStream.Read(buffer, 0, 1024);
                    while (l > 0)
                    {
                        fs.Write(buffer, 0, l);
                        l = inputStream.Read(buffer, 0, 1024);
                    }
                    fs.Flush();
                    fs.Close();
                }
                statuses.Add(new FilesStatus(handlerPath, uploadPath, fileName, file));
            }
        }

        // Upload entire file
        private void UploadWholeFile(HttpContext context, List<FilesStatus> statuses)
        {
            for (int i = 0; i < context.Request.Files.Count; i++)
            {
                var file = context.Request.Files[i];

                string uploadPath = GetUploadPath(file.FileName, context);
                var sfile = GetRenamedFileIfExists(uploadPath + file.FileName, context);
                if (!sfile.Directory.Exists)
                    sfile.Directory.Create();
                file.SaveAs(sfile.FullName);

                statuses.Add(new FilesStatus(handlerPath, uploadPath, file.FileName, sfile.Name, file.ContentLength, sfile.FullName));
            }
        }

        private void WriteJsonIframeSafe(HttpContext context, List<FilesStatus> statuses)
        {
            context.Response.AddHeader("Vary", "Accept");
            try
            {
                if (context.Request["HTTP_ACCEPT"].Contains("application/json"))
                    context.Response.ContentType = "application/json";
                else
                    context.Response.ContentType = "text/plain";
            }
            catch
            {
                context.Response.ContentType = "text/plain";
            }

            var serializer = new JavaScriptSerializer();
            var jsonObj = serializer.Serialize(statuses.ToArray());
            context.Response.Write(jsonObj);
        }

        private static bool GivenFilename(HttpContext context)
        {
            return !string.IsNullOrEmpty(context.Request["f"]);
        }

        private void DeliverFile(HttpContext context)
        {
            var filename = context.Request["f"];
            string uploadPath = GetUploadPath(filename, context);
            var file = new FileInfo(context.Server.MapPath(uploadPath + filename));

            if (file.Exists)
            {
                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
                context.Response.ContentType = "application/octet-stream";
                context.Response.ClearContent();
                context.Response.WriteFile(file.FullName);
            }
            else
                context.Response.StatusCode = 404;
        }

        private void ListCurrentFiles(HttpContext context)
        {
            var dir = new DirectoryInfo(context.Server.MapPath(UploadPath));
            var files = dir.GetFiles("*.*", SearchOption.AllDirectories)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .Select(f => new FilesStatus(handlerPath, UploadPath, f.Name, f))
                    .ToArray();

            var serializer = new JavaScriptSerializer();
            string jsonObj = serializer.Serialize(files); // js.Serialize(files);
            context.Response.AddHeader("Content-Disposition", "inline; filename=\"files.json\"");
            context.Response.Write(jsonObj);
            context.Response.ContentType = "application/json";
        }

        private FileInfo GetRenamedFileIfExists(string relativeFile, HttpContext context)
        {
            var file = new FileInfo(context.Server.MapPath(relativeFile));
            string fileName = file.Name.Replace(file.Extension, "");
            string ext = file.Extension;
            int idx = 1;
            while (file.Exists)
            {
                file = new FileInfo(Path.Combine(file.DirectoryName,
                    string.Format("{0}({1}){2}", fileName, idx, ext)));
                idx++;
            }
            return file;
        }

        public string GetUploadPath(string fileName, HttpContext context)
        {
            if (fileName.StartsWith(UploadPath))
                return string.Empty;
            string fileExtension = fileName.Substring(fileName.LastIndexOf('.'));
            //FileType ft = FileType.CommonTypes.GetFileTypeByExtension(fileExtension.ToLower());
            string subPath = context.Request.QueryString["path"]; //ft == null ? "files" : ft.TypeCode;
            string owner = context.Request.QueryString["owner"];
            if (string.IsNullOrEmpty(owner))
                owner = "public";
            return string.Concat(UploadPath, owner, "/", subPath, "/");
        }

        private static string uploadPath;
        public string UploadPath
        {
            get
            {
                if (string.IsNullOrEmpty(uploadPath))
                {
                    uploadPath = ConfigurationManager.AppSettings["UploadPath"];
                    if (string.IsNullOrEmpty(uploadPath))
                        uploadPath = "/Uploads/";
                    if (!uploadPath.StartsWith("/"))
                        uploadPath = "/" + uploadPath;
                    if (!uploadPath.EndsWith("/"))
                        uploadPath += "/";
                }
                return uploadPath;
            }
        }
    }
}