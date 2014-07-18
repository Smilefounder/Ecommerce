using Kooboo.Commerce.Web.Framework.UI;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Tabs
{
    public abstract class TabPlugin<TModel> : ITabPlugin
    {
        public abstract string Name { get; }

        public virtual string DisplayName
        {
            get
            {
                return Name;
            }
        }

        public virtual string VirtualPath
        {
            get
            {
                return null;
            }
        }

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

        public abstract IEnumerable<MvcRoute> ApplyTo { get; }

        public abstract void OnLoad(TabLoadContext context);

        public abstract void OnSubmit(TabSubmitContext<TModel> context);

        void ITabPlugin.OnSubmit(TabSubmitContext context)
        {
            OnSubmit(new TabSubmitContext<TModel>(context));
        }
    }
}
