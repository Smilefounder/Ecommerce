using Kooboo.Commerce.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Activities
{
    public class ActivityModel
    {
        public string Name { get; set; }

        public bool AllowAsyncExecution { get; set; }

        public string EditorVirtualPath { get; set; }

        public ActivityModel() { }

        public ActivityModel(IActivity activity)
        {
            Name = activity.Name;
            AllowAsyncExecution = activity.AllowAsyncExecution;

            var editor = activity.GetEditor();
            if (editor != null)
            {
                EditorVirtualPath = editor.VirtualPath;
            }
        }
    }
}