using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Activities.Simple
{
    public class SimpleActivityParameters : ActivityParameters
    {
        [Required]
        public string Parameter1
        {
            get
            {
                return GetValue("Parameter1");
            }
            set
            {
                SetValue("Parameter1", value);
            }
        }

        public int Parameter2
        {
            get
            {
                return GetValue<int>("Parameter2");
            }
            set
            {
                SetValue("Parameter2", value);
            }
        }
    }
}
