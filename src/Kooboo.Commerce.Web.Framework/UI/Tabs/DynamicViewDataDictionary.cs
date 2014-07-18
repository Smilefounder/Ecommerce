using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Tabs
{
    class DynamicViewDataDictionary : DynamicObject
    {
        private readonly Func<ViewDataDictionary> _viewDataThunk;

        private ViewDataDictionary ViewData
        {
            get
            {
                return this._viewDataThunk();
            }
        }

        public DynamicViewDataDictionary(Func<ViewDataDictionary> viewDataThunk)
        {
            this._viewDataThunk = viewDataThunk;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return this.ViewData.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this.ViewData[binder.Name];
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this.ViewData[binder.Name] = value;
            return true;
        }
    }
}
