using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Rebuild
{
    public class RebuildTaskContext
    {
        public string Instance { get; private set; }

        public Type DocumentType { get; private set; }

        public CultureInfo Culture { get; private set; }

        public RebuildTaskContext(string instance, Type documentType, CultureInfo culture)
        {
            Require.NotNullOrEmpty(instance, "instance");
            Require.NotNull(documentType, "documentType");
            Require.NotNull(culture, "culture");

            Instance = instance;
            DocumentType = documentType;
            Culture = culture;
        }
    }
}