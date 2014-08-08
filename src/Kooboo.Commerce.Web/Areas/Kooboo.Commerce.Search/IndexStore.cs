using Kooboo.Commerce.Search.Facets;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search
{
    public class IndexStore : IDisposable
    {
        private Directory _directory;
        private readonly object _lock = new object();
        private IndexWriter _writer;
        private IndexSearcher _searcher;

        public Type DocumentType { get; private set; }

        public IndexStore(Type documentType, Directory directory, Analyzer analyzer)
        {
            DocumentType = documentType;
            _directory = directory;
            _writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            _searcher = new IndexSearcher(_writer.GetReader());
        }

        private IndexSearcher GetSearcher()
        {
            if (_searcher == null)
            {
                lock (_lock)
                {
                    if (_searcher == null)
                    {
                        _searcher = new IndexSearcher(_writer.GetReader());
                    }
                }
            }

            return _searcher;
        }

        public TopDocs Search(Query query, int topN)
        {
            return GetSearcher().Search(query, topN);
        }

        public TopFieldDocs Search(Query query, Filter filter, int topN, Sort sort)
        {
            return GetSearcher().Search(query, filter, topN, sort);
        }

        public IList<FacetResult> GetFacets(Query query, IEnumerable<Facet> facets)
        {
            var searcher = new FacetedSearcher(GetSearcher());
            return searcher.Search(query, facets);
        }

        public void Index(object model)
        {
            var doc = DocumentBuilder.Build(model);
            var keyFieldName = GetKeyFieldName();
            _writer.DeleteDocuments(new Term(keyFieldName, doc.GetField(keyFieldName).StringValue));
            _writer.AddDocument(doc);
        }

        public void Delete(object key)
        {
            var term = new TermQuery(new Term(GetKeyFieldName(), key.ToString()));
            _writer.DeleteDocuments(term);
        }

        private string GetKeyFieldName()
        {
            return EntityKey.GetKeyProperty(DocumentType).Name;
        }

        public void Commit()
        {
            lock (_lock)
            {
                _writer.Commit();

                if (_searcher != null)
                {
                    _searcher.Dispose();
                    _searcher = null;
                }
            }
        }

        public void Dispose()
        {
            _writer.Dispose();
            _directory.Dispose();

            if (_searcher != null)
            {
                _searcher.IndexReader.Dispose();
                _searcher.Dispose();
            }
        }
    }
}