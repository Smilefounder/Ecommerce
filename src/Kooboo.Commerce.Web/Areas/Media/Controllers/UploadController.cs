using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Configuration;
using Kooboo.Commerce.Handlers;

namespace Kooboo.Commerce.Web.Areas.Media.Controllers
{
    public class UploadController : Controller
    {
        private string uploadFolder = "~/Uploads/";

        public UploadController()
        {
            uploadFolder = ConfigurationManager.AppSettings["UploadPath"] ?? "~/Uploads/";
            if (!uploadFolder.EndsWith("/"))
                uploadFolder += "/";
        }

        [HttpGet]
        public ActionResult Index(string owner, string path, int orderBy = 0, int pi = 0, int ps = 50)
        {
            return View();
        }

        [HttpGet]
        public ActionResult OpenFile()
        {
            Dictionary<string, object> paras = new Dictionary<string,object>();
            foreach (var k in Request.QueryString.AllKeys)
                paras.Add(k, Request.QueryString[k]);
            return View("OpenUpload", paras);
        }

        private DirectoryInfo GetFolder(string owner, string path, out string[] subPaths)
        {
            if (string.IsNullOrEmpty(owner))
                owner = "public";

            var paths = new List<string>();
            paths.Add(owner);
            paths.AddRange(path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));

            var dir = new DirectoryInfo(Server.MapPath(uploadFolder));
            foreach (var p in paths)
            {
                var dp = Path.Combine(dir.FullName, p);
                dir = new DirectoryInfo(dp);
                if (!dir.Exists)
                    dir.Create();
            }
            subPaths = paths.ToArray();
            return dir;
        }

        [HttpGet]
        public ActionResult Files(string owner, string path, string search = null, int orderBy = 0, int pi = 0, int ps = 50)
        {
            string[] paths = new string[0];
            var dir = GetFolder(owner, path, out paths);

            var folders = dir.GetDirectories().Select(o => new
            {
                Name = o.Name
            });

            var files = dir.GetFiles().AsEnumerable();

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

            if (pi >= 0 && ps > 0)
            {
                files = files.Skip(pi * ps).Take(ps);
            }
            string basePath = (uploadFolder + string.Join("/", paths.ToArray())).TrimStart('~') + "/";
            var vfs = files.Where(f => !f.Name.StartsWith("_")).Select(f => new
            {
                FileName = f.Name,
                FileType = FileType.GetFileType(f.Extension),
                CreationDate = f.CreationTime.ToShortDateString(),
                Url = basePath + f.Name,
                FileSize = FileType.GetFriendlyFileSize((int)f.Length),
                IsImage = FileType.IsImage(f.Extension)
            }).ToArray();

            return Json(new { Paths = paths, Folders = folders, Files = vfs }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddFolder(string owner, string path, string folder)
        {
            string[] paths = new string[0];
            var dir = GetFolder(owner, path, out paths);
            dir = new DirectoryInfo(Path.Combine(dir.FullName, folder));
            if(!dir.Exists)
            {
                dir.Create();
                return Json(new { status = 0, message = string.Format("Folder {0} created.", folder) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = 1, message = string.Format("Folder {0} already exists.", folder) }, JsonRequestBehavior.AllowGet);
            }
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
                            message ="Delete Successfully";
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
