using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Membership;
using Kooboo.CMS.Membership.Models;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    static class HttpContextExtensions
    {
        public static MembershipUser GetMembershipUser(this HttpContextBase context)
        {
            var membership = context.Membership();
            return membership == null ? null : membership.GetMembershipUser();
        }
    }
}