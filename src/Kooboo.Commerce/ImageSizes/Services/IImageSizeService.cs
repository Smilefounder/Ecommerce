using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.ImageSizes.Services {

    public interface IImageSizeService {

        ImageSize GetById(string name);

        IQueryable<ImageSize> Query();

        void Create(ImageSize size);

        void Update(ImageSize size);

        void Delete(ImageSize size);

        void Enable(ImageSize size);

        void Disable(ImageSize size);
    }
}
