using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Facets
{
    public class FacetedSearcher
    {
        private IndexSearcher _searcher;

        public FacetedSearcher(IndexSearcher searcher)
        {
            _searcher = searcher;
        }

        public IList<FacetResult> Search(Query query, IEnumerable<Facet> facets)
        {
            var reader = _searcher.IndexReader;
            var allCollector = new GatherAllCollector();
            _searcher.Search(query, allCollector);

            // Key: Facet name, Value: { Key: Term }
            var facetsByName = new Dictionary<string, Dictionary<string, FacetValue>>();
            var encodedRanges = facets.Where(f => f.Ranges != null && f.Ranges.Count > 0)
                                      .ToDictionary(f => f.Name, f => f.Ranges.Select(r => EncodedFacetRange.Encode(r)).ToList());

            using (var termDocs = reader.TermDocs())
            {
                foreach (var facet in facets)
                {
                    var isRangeFacet = facet.Ranges != null && facet.Ranges.Count > 0;
                    var field = facet.Name;

                    // Loop all terms of the specified field
                    using (var termEnum = reader.Terms(new Term(field)))
                    {
                        Dictionary<string, FacetValue> valuesByTerm;
                        if (!facetsByName.TryGetValue(field, out valuesByTerm))
                        {
                            valuesByTerm = new Dictionary<string, FacetValue>();
                            facetsByName.Add(field, valuesByTerm);
                        }

                        do
                        {
                            if (termEnum.Term == null || termEnum.Term.Field != field)
                            {
                                break;
                            }

                            if (isRangeFacet && IsLowPrecisionNumber(termEnum.Term.Text))
                            {
                                continue;
                            }

                            var totalDocs = termEnum.DocFreq();
                            termDocs.Seek(termEnum.Term);

                            // Loop thought all documents containing the term
                            while (termDocs.Next() && totalDocs > 0)
                            {
                                totalDocs--;

                                // Collect the filtered result only
                                if (!allCollector.Documents.Contains(termDocs.Doc))
                                {
                                    continue;
                                }
                                if (reader.IsDeleted(termDocs.Doc))
                                {
                                    continue;
                                }

                                // Check document hits
                                if (!isRangeFacet)
                                {
                                    FacetValue facetValue;
                                    if (!valuesByTerm.TryGetValue(termEnum.Term.Text, out facetValue))
                                    {
                                        facetValue = new FacetValue(termEnum.Term.Text, 1);
                                        valuesByTerm.Add(termEnum.Term.Text, facetValue);
                                    }
                                    else
                                    {
                                        facetValue.Hits++;
                                    }
                                }
                                else
                                {
                                    foreach (var range in encodedRanges[facet.Name])
                                    {
                                        if (range.Includes(termEnum.Term.Text))
                                        {
                                            FacetValue facetValue;
                                            if (!valuesByTerm.TryGetValue(range.Label, out facetValue))
                                            {
                                                facetValue = new FacetValue(range.Label, 1);
                                                valuesByTerm.Add(range.Label, facetValue);
                                            }
                                            else
                                            {
                                                facetValue.Hits++;
                                            }
                                        }
                                    }
                                }
                            }

                        } while (termEnum.Next());
                    }
                }
            }

            var results = new List<FacetResult>();
            foreach (var each in facetsByName)
            {
                results.Add(new FacetResult(each.Key, each.Value.Values));
            }

            return results;
        }

        // TODO: How it works?
        static bool IsLowPrecisionNumber(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return false;
            }

            return value[0] - NumericUtils.SHIFT_START_INT != 0 &&
                   value[0] - NumericUtils.SHIFT_START_LONG != 0;
        }
    }
}