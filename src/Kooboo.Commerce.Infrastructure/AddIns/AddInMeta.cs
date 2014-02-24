using Kooboo.Commerce.ComponentModel;
using Kooboo.Commerce.ComponentModel.DataAnnotations;
using Kooboo.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Kooboo.Commerce.AddIns
{
    [DataContract]
    public class AddInMeta
    {
        public static readonly string FileName = "meta.config";

        [DataMember(Order = 0)]
        [Required]
        public string Id { get; set; }

        [DataMember(Order = 1)]
        public string Description { get; set; }

        [DataMember(Order = 2)]
        public string Author { get; set; }

        [DataMember(Order = 3)]
        [Required, VersionNumber]
        public string Version { get; set; }

        // TODO: Supported commerce version

        // TODO: Settings entry after installation

        public IList<ValidationResult> Validate()
        {
            return ObjectValidator.Validate(this);
        }

        public static AddInMeta LoadFrom(string path)
        {
            return DataContractSerializationHelper.Deserialize<AddInMeta>(path);
        }

        public static AddInMeta LoadFrom(Stream stream)
        {
            return (AddInMeta)DataContractSerializationHelper.Deserialize(typeof(AddInMeta), null, stream);
        }
    }
}
