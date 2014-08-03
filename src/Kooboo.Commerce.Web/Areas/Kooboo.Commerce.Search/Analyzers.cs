using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search
{
    public static class Analyzers
    {
        public static readonly Analyzer Default = new StandardAnalyzer(LuceneConstants.Version);

        static readonly Dictionary<CultureInfo, Analyzer> _analyzers = new Dictionary<CultureInfo,Analyzer>();

        public static Analyzer GetAnalyzer(CultureInfo culture)
        {
            Analyzer analyzer = null;

            if (_analyzers.TryGetValue(culture, out analyzer))
            {
                return analyzer;
            }

            return Default;
        }
    }
}