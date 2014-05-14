using Kooboo.Commerce.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Activities
{
    public class ActivityDescriptorModel
    {
        public string Name { get; set; }

        public bool AllowAsyncExecution { get; set; }

        public bool Configurable { get; set; }

        public string ConfigViewVirtualPath { get; set; }

        public ActivityDescriptorModel() { }

        public ActivityDescriptorModel(IActivityDescriptor descriptor)
        {
            Name = descriptor.Name;
            AllowAsyncExecution = descriptor.AllowAsyncExecution;
            Configurable = descriptor.Configurable;
            ConfigViewVirtualPath = descriptor.ConfigViewVirtualPath;
        }
    }
}