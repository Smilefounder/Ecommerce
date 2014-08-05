using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Facets
{
    class GatherAllCollector : Collector
    {
        private int _docBase = 0;

        public HashSet<int> Documents { get; private set; }

        public GatherAllCollector()
        {
            Documents = new HashSet<int>();
        }

        public override bool AcceptsDocsOutOfOrder
        {
            get
            {
                return true;
            }
        }

        public override void Collect(int doc)
        {
            Documents.Add(_docBase + doc);
        }

        public override void SetNextReader(Lucene.Net.Index.IndexReader reader, int docBase)
        {
            _docBase = docBase;
        }

        public override void SetScorer(Scorer scorer)
        {
        }
    }
}