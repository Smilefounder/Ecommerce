using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Catalogs {

    public class Catalog {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
