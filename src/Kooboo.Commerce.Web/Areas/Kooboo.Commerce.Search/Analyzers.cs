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
        static Lucene.Net.Util.Version Version = Lucene.Net.Util.Version.LUCENE_30;

        public static readonly Analyzer Default = new StandardAnalyzer(Version);

        static readonly Dictionary<CultureInfo, Analyzer> _analyzers = new Dictionary<CultureInfo, Analyzer>();

        static Analyzers()
        {
            var analyzers = new Dictionary<string, Analyzer>(StringComparer.OrdinalIgnoreCase)
            {
                { "zh", new Lucene.Net.Analysis.Cn.ChineseAnalyzer() },
                { "nl", new Lucene.Net.Analysis.Nl.DutchAnalyzer(Version) },
                { "de", new Lucene.Net.Analysis.De.GermanAnalyzer(Version) },
                { "ru", new Lucene.Net.Analysis.Ru.RussianAnalyzer(Version) },
                { "th", new Lucene.Net.Analysis.Th.ThaiAnalyzer(Version) },
                { "fr", new Lucene.Net.Analysis.Fr.FrenchAnalyzer(Version) },
                { "ar", new Lucene.Net.Analysis.AR.ArabicAnalyzer(Version) },
                { "pt", new Lucene.Net.Analysis.BR.BrazilianAnalyzer(Version) },
                { "cs", new Lucene.Net.Analysis.Cz.CzechAnalyzer(Version) },
                { "el", new Lucene.Net.Analysis.El.GreekAnalyzer(Version) }
            };

            foreach (var culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                if (analyzers.ContainsKey(culture.TwoLetterISOLanguageName))
                {
                    _analyzers.Add(culture, analyzers[culture.TwoLetterISOLanguageName]);
                }
            }
        }

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