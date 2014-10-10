using Kooboo.Commerce.Data.Folders;
using Kooboo.Commerce.Handlers;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Web.Url;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class MediaLibraryController : CommerceController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Selection()
        {
            return View();
        }

        [HttpGet]
        public ActionResult OpenFile()
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            foreach (var k in Request.QueryString.AllKeys)
                paras.Add(k, Request.QueryString[k]);
            return View("OpenUpload", paras);
        }

        private IList<string> GetBreadcrumb(string path)
        {
            var paths = new List<string>();
            paths.Add(CurrentInstance.DataFolders.Media.Name);
            paths.AddRange(path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));
            return paths;
        }

        [HttpGet]
        public ActionResult Files(string path, string search = null, int orderBy = 0, int pi = 0, int ps = 50)
        {
            if (String.IsNullOrEmpty(path))
            {
                path = "/";
            }

            var dir = new DirectoryInfo(Server.MapPath(CurrentInstance.DataFolders.Media.GetFolder(path).VirtualPath));

            // TODO: return empty instead of create folder
            if (!dir.Exists)
            {
                dir.Create();
            }

            IEnumerable<DirectoryInfo> folders = Enumerable.Empty<DirectoryInfo>();
            IEnumerable<FileInfo> files = Enumerable.Empty<FileInfo>();

            if (string.IsNullOrEmpty(search))
            {
                folders = dir.GetDirectories();
                files = dir.GetFiles();
            }
            else
            {
                search = string.Format("*{0}*", search);
                folders = dir.GetDirectories(search);
                files = dir.GetFiles(search);
            }

            switch (orderBy)
            {
                case 1:
                    files = files.OrderBy(o => o.Name);
                    break;
                case 0:
                default:
                    files = files.OrderByDescending(o => o.CreationTime);
                    break;
            }

            int totalRecords = 0;
            int totalPages = 0;
            int startIndex = 0;
            int endIndex = 0;


            if (pi >= 0 && ps > 0)
            {
                totalRecords = folders.Count() + files.Count();
                totalPages = Convert.ToInt32(Math.Ceiling((double)totalRecords / ps));
                if (pi > totalPages - 1)
                    pi = totalPages - 1;
                startIndex = pi * ps + 1;
                endIndex = startIndex + ps - 1 < totalRecords ? startIndex + ps - 1 : totalRecords;

                int left = (pi + 1) * ps - folders.Count();
                if (left > 0)
                {
                    int skip = left <= ps ? 0 : pi * ps - folders.Count();
                    left = left <= ps ? left : ps;
                    files = files.Skip(skip).Take(left);
                }
                else
                {
                    files = new FileInfo[0];
                }
                folders = folders.Skip(pi * ps).Take(ps);
            }

            var vs = folders.Select(o => new
            {
                Name = o.Name
            });
            var vfs = files.Where(f => !f.Name.StartsWith("_")).Select(f => new
            {
                FileName = f.Name,
                FileType = FileType.GetFileType(f.Extension),
                CreationDate = f.CreationTime.ToShortDateString(),
                Url = UrlUtility.Combine(CurrentInstance.DataFolders.Media.VirtualPath, path, f.Name),
                FileSize = FileType.GetFriendlyFileSize((int)f.Length),
                IsImage = FileType.IsImage(f.Extension)
            }).ToArray();
            var pagers = ps <= 0 ? null : new { TotalRecords = totalRecords, TotalPages = totalPages, StartIndex = startIndex, EndIndex = endIndex, PageIndex = pi, PageSize = ps };
            return Json(new { 
                Paths = GetBreadcrumb(path), 
                Folders = vs, 
                Files = vfs, 
                Pager = pagers 
            }, JsonRequestBehavior.AllowGet);
        }

        [HandleAjaxError]
        public void AddFolder(string path, string folder)
        {
            var parent = CurrentInstance.DataFolders.Media.GetFolder(path);
            parent.GetFolder(folder).Create();
        }

        [HttpPost]
        public void Upload(string path)
        {
            var context = ControllerContext.HttpContext;
            var statuses = new List<FilesStatus>();
            var headers = context.Request.Headers;

            if (string.IsNullOrEmpty(headers["X-File-Name"]))
            {
                UploadWholeFile(path, statuses);
            }
            else
            {
                UploadPartialFile(path, headers["X-File-Name"], statuses);
            }

            WriteJsonIframeSafe(context, statuses);
        }

        // Upload partial file
        private void UploadPartialFile(string path, string fileName, List<FilesStatus> statuses)
        {
            if (Request.Files.Count != 1)
                throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");

            var folder = CurrentInstance.DataFolders.Media;
            if (!String.IsNullOrEmpty(path))
            {
                folder = folder.GetFolder(path);
            }

            var file = GetRenamedFileIfExists(folder, fileName);

            var length = file.Write(Request.Files[0].InputStream);

            statuses.Add(new FilesStatus(fileName, file.Name, length, folder.VirtualPath));
        }

        // Upload entire file
        private void UploadWholeFile(string path, List<FilesStatus> statuses)
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                var folder = CurrentInstance.DataFolders.Media;
                if (!String.IsNullOrEmpty(path))
                {
                    folder = folder.GetFolder(path);
                }

                var dataFile = GetRenamedFileIfExists(folder, file.FileName);
                var length = dataFile.Write(file.InputStream);

                statuses.Add(new FilesStatus(file.FileName, dataFile.Name, length, folder.VirtualPath));
            }
        }

        private void WriteJsonIframeSafe(HttpContextBase context, List<FilesStatus> statuses)
        {
            context.Response.AddHeader("Vary", "Accept");

            if (context.Request["HTTP_ACCEPT"].Contains("application/json"))
            {
                context.Response.ContentType = "application/json";
            }
            else
            {
                context.Response.ContentType = "text/plain";
            }

            var json = JsonConvert.SerializeObject(statuses);
            context.Response.Write(json);
        }

        private DataFile GetRenamedFileIfExists(DataFolder folder, string fileName)
        {
            var fileNameWithExt = Path.GetFileNameWithoutExtension(fileName);
            var ext = Path.GetExtension(fileName);

            var file = folder.GetFile(fileName);
            int index = 1;
            while (file.Exists)
            {
                fileNameWithExt = String.Format("{0}({1}){2}", fileNameWithExt, index, ext);
                file = folder.GetFile(fileNameWithExt + ext);
                index++;
            }

            return file;
        }

        [HttpGet]
        public ActionResult Do(string file, string op, string args)
        {
            int status = 0;
            string message = string.Empty;
            var f = new FileInfo(Server.MapPath(file));
            if (f != null)
            {
                try
                {
                    switch (op.ToLower())
                    {
                        case "delete":
                            f.Delete();
                            status = 0;
                            message = "Delete Successfully";
                            break;
                        case "rename":
                            break;
                    }
                }
                catch (Exception ex)
                {
                    status = 1;
                    message = ex.Message;
                }
            }
            return Json(new { Status = status, Message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult OpenImage()
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            foreach (var k in Request.QueryString.AllKeys)
                paras.Add(k, Request.QueryString[k]);
            return View("ImageCrop", paras);
        }

        [HttpGet]
        public ActionResult SaveImage(string file, float x, float y, float width, float height, int? toWidth = null, int? toHeight = null)
        {
            if (file.IndexOf('?') >= 0)
            {
                file = file.Substring(0, file.IndexOf('?'));
            }
            string path = Server.MapPath(file);

            if (x >= 0 && y >= 0 && width > 0 && height > 0)
            {
                Image img = Image.FromFile(path);
                RectangleF rect = new RectangleF(x, y, width, height);
                var cropImg = ImageHelper.Crop(img, rect);

                if (toWidth.HasValue && toHeight.HasValue)
                {
                    cropImg = ImageHelper.Resize(cropImg, new Size(toWidth.Value, toHeight.Value));
                }

                cropImg.Save(path);
            }

            return Json(file, JsonRequestBehavior.AllowGet);
        }
    }
}
