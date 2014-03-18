using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Search
{
    public interface ISearch
    {
        string TypeName { get; }
        string Directory { get; }
        IDocumentBuilder DocumentBuilder { get; }
        IIndexPathBuilder IndexPathBuilder { get; }

        void Initialize();
        void Scope(IDictionary<string, object> values);
        void NewQuery();
        void Where(string fieldName, string word, ClauseOccur occur = ClauseOccur.SHOULD, float boost = 1.0f);
        void Equals(string fieldName, string word, ClauseOccur occur = ClauseOccur.SHOULD, float boost = 1.0f);
        void Wildcard(string fieldName, string word, ClauseOccur occur = ClauseOccur.SHOULD, float boost = 1.0f);
        void Range(string fieldName, object lowerValue, object upperValue, bool includeLower = true, bool includeUpper = true, ClauseOccur occur = ClauseOccur.SHOULD, float boost = 1.0f);
        void Prefix(string fieldName, string prefix, ClauseOccur occur = ClauseOccur.SHOULD, float boost = 1.0f);

        void Facet(params string[] fieldNames);
        void SortBy(string fieldName, bool descending = false);

        IEnumerable<IHit> Query(int pageIndex, int pageSize, out int totalCount, out IEnumerable<FacetGroup> facetedResults);
        void Close();
    }
}
