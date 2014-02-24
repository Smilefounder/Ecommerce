using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.AddIns
{
    public class AddInPackage
    {
        private ZipFile _zip;
        private IList<ValidationResult> _validationResults;

        public AddInMeta Meta { get; private set; }

        public AddInPackage(Stream stream)
        {
            Require.NotNull(stream, "stream");
            _zip = ZipFile.Read(stream);
            ParsePackage();
        }

        public IList<ValidationResult> Validate()
        {
            var results = new List<ValidationResult>();
            if (Meta == null)
            {
                results.Add(new ValidationResult("Missing meta.config file."));
            }

            if (_validationResults != null)
            {
                results.AddRange(_validationResults);
            }

            return results;
        }

        private void ParsePackage()
        {
            var metaEntry = _zip[AddInMeta.FileName];

            if (metaEntry == null)
            {
                metaEntry = _zip.Entries.FirstOrDefault(x => x.FileName.Contains(AddInMeta.FileName, StringComparison.OrdinalIgnoreCase));
            }

            if (metaEntry != null)
            {
                using (var stream = new MemoryStream())
                {
                    metaEntry.Extract(stream);
                    stream.Position = 0;

                    Meta = AddInMeta.LoadFrom(stream);
                    _validationResults = Meta.Validate();
                }
            }
        }

        public void Extract(string path)
        {
            _zip.ExtractAll(path, ExtractExistingFileAction.OverwriteSilently);
        }
    }
}
