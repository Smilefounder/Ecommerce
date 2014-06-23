using System;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.Tabs
{
    public abstract class TabPlugin<TModel> : ITabPlugin, ISubmittable
    {
        public abstract string Name { get; }

        public virtual string DisplayName
        {
            get
            {
                return Name;
            }
        }

        public abstract string VirtualPath { get; }

        public virtual int Order
        {
            get
            {
                return 0;
            }
        }

        public Type ModelType
        {
            get
            {
                return typeof(TModel);
            }
        }

        public abstract bool IsVisible(ControllerContext controllerContext);

        public abstract void OnLoad(TabLoadContext context);

        public abstract void OnSubmit(TabSubmitContext<TModel> context);

        void ISubmittable.OnSubmit(TabSubmitContext context)
        {
            OnSubmit(new TabSubmitContext<TModel>(context));
        }
    }
}
