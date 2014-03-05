using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public abstract class AggregateRoot
    {
        [Required]
        public virtual AggregateMetadata Metadata { get; protected internal set; }
    }
}
