using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Kooboo.Commerce.Settings
{
    public class ImageType
    {
        public string Name { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool AllowMultiple { get; set; }

        public ImageType() { }

        public ImageType(string name, int width, int height)
            : this(name, width, height, false)
        {
        }

        public ImageType(string name, int width, int height, bool allowMultiple)
        {
            Name = name;
            Width = width;
            Height = height;
            AllowMultiple = allowMultiple;
        }
    }
}
