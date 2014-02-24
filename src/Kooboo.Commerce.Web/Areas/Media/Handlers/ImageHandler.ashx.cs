using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Drawing;

namespace Kooboo.Commerce.Handlers
{
    /// <summary>
    /// Summary description for Image
    /// </summary>
    public class ImageHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            FileInfo fi = new FileInfo(context.Request.PhysicalPath);
            if (!fi.Exists)
            {
                context.Response.StatusCode = 404;
                context.Response.End();
                return;
            }
            int width = 400;
            int height = 300;
            if (context.Request.QueryString["w"] != null)
                int.TryParse(context.Request.QueryString["w"], out width);
            if (context.Request.QueryString["h"] != null)
                int.TryParse(context.Request.QueryString["h"], out height);
            fi = new FileInfo(Path.Combine(fi.Directory.FullName, string.Format("{0}_{1}_{2}.jpg", fi.Name.Replace(fi.Extension, ""), width, height)));
            if (!fi.Exists)
            {
                Image img = Image.FromFile(context.Request.PhysicalPath);
                float scale = 1.0f;
                ResizeMode mode = ResizeMode.MaxWidthOrHeight;
                if (context.Request.QueryString["s"] != null)
                    float.TryParse(context.Request.QueryString["s"], out scale);
                if (context.Request.QueryString["m"] != null)
                {
                    int m = (int)mode;
                    int.TryParse(context.Request.QueryString["m"], out m);
                    mode = (ResizeMode)m;
                }

                Image smallImg = ImageHelper.ResizeImage(img, width, height, mode, scale);
                smallImg.Save(fi.FullName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            context.Response.ContentType = "image/jpeg";
            context.Response.StatusCode = 200;
            context.Response.WriteFile(fi.FullName);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}