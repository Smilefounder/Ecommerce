using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    [ComplexType]
    public class AggregateMetadata
    {
        [Required, StringLength(50)]
        public virtual string CommerceName { get; protected set; }

        protected AggregateMetadata() { }

        public AggregateMetadata(string commerceName)
        {
            Require.NotNullOrEmpty(commerceName, "commerceName");
            CommerceName = commerceName;
        }

        public override string ToString()
        {
            return CommerceName;
        }
    }
}
