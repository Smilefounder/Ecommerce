using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Kooboo.Commerce.Web.Mvc
{
    public class EFContractResolver : DefaultContractResolver
    {
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var members = base.GetSerializableMembers(objectType);
            members.RemoveAll(memberInfo => (IsMemberEntityWrapper(memberInfo)));
            return members;
        }

        private static bool IsMemberEntityWrapper(MemberInfo memberInfo)
        {
            return memberInfo.Name == "_entityWrapper";
        }
    }
}
