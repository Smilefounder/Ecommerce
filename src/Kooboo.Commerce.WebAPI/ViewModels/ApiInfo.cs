using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.WebAPI.ViewModels
{
    public class ApiControllerInfo
    {
        public string ControllerName { get; set; }
        public string Comments { get; set; }
        public IEnumerable<ApiActionInfo> Actions { get; set; }
    }

    public class ApiActionInfo
    {
        public string ActionName { get; set; }
        public string ApiRoute { get; set; }
        public string[] HttpMethods { get; set; }
        public string Comments { get; set; }

        public IEnumerable<ApiParameterInfo> Parameters { get; set; }
    }

    public class ApiParameterInfo
    {
        public string ParameterName { get; set; }
        public string TypeName { get; set; }
        public string Comments { get; set; }
    }
}