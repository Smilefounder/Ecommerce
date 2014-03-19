using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;

namespace Kooboo.Commerce.ImageSizes.Services {

    [Dependency(typeof(IImageSizeService))]
    public class ImageSizeService : IImageSizeService {

        private IRepository<ImageSize> Repository;
        public ImageSizeService(IRepository<ImageSize> repository) {
            Repository = repository;
        }

        public ImageSize GetById(string name) {
            return Repository.Get(o => o.Name == name);
        }

        public IQueryable<ImageSize> Query() {
            return Repository.Query();
        }

        public bool Create(ImageSize size) {
            return Repository.Insert(size);
        }

        public bool Update(ImageSize size) {
            return Repository.Update(size, k => new object[] { k.Name });
        }

        public bool Delete(ImageSize size) {
            return Repository.Delete(size);
        }

        public void Enable(ImageSize size) {
            size.IsEnabled = true;
            Repository.Update(size, k => new object[] { k.Name });
        }

        public void Disable(ImageSize size) {
            size.IsEnabled = false;
            Repository.Update(size, k => new object[] { k.Name });
        }
    }
}
