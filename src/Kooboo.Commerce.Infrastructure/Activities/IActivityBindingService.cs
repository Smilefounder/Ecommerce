using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities.Services
{
    public interface IActivityBindingService
    {
        ActivityBinding GetById(int bindingId);

        IQueryable<ActivityBinding> Query();

        void Create(ActivityBinding binding);

        void Update(ActivityBinding binding);

        void Delete(ActivityBinding binding);

        void Enable(ActivityBinding binding);

        void Disable(ActivityBinding binding);
    }

    [Dependency(typeof(IActivityBindingService))]
    public class ActivityService : IActivityBindingService
    {
        private IRepository<ActivityBinding> _repository;

        public ActivityService(IRepository<ActivityBinding> repository)
        {
            _repository = repository;
        }

        public ActivityBinding GetById(int bindingId)
        {
            return _repository.Get(o => o.Id == bindingId);
        }

        public IQueryable<ActivityBinding> Query()
        {
            return _repository.Query();
        }

        public void Create(ActivityBinding binding)
        {
            _repository.Insert(binding);
        }

        public void Update(ActivityBinding binding)
        {
            _repository.Update(binding, o => new object[] { o.Id });
        }

        public void Delete(ActivityBinding binding)
        {
            _repository.Delete(binding);
        }

        public void Enable(ActivityBinding binding)
        {
            binding.Enable();
            _repository.Update(binding, o => new object[] { o.Id });
        }

        public void Disable(ActivityBinding binding)
        {
            binding.Disable();
            _repository.Update(binding, o => new object[] { o.Id });
        }
    }
}
