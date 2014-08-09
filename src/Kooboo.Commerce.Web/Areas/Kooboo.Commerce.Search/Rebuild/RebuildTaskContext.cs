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

        public Type ModelType { get; private set; }

        public CultureInfo Culture { get; private set; }

        public RebuildTaskContext(string instance, Type modelType, CultureInfo culture)
        {
            Require.NotNullOrEmpty(instance, "instance");
            Require.NotNull(modelType, "modelType");
            Require.NotNull(culture, "culture");

            Instance = instance;
            ModelType = modelType;
            Culture = culture;
        }
    }
}