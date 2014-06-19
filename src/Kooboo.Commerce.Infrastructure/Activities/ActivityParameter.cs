using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public class ActivityParameter
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public object DefaultValue { get; set; }

        public ActivityParameter(string name)
        {
            Name = name;
        }
    }
}
