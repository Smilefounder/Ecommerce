﻿using Kooboo.Commerce.Search.Facets;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Linq;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search
{
    public class DocumentIndexer : IDisposable
    {
        private Directory _directory;
        private readonly object _lock = new object();
        private IndexWriter _writer;
        private IndexSearcher _searcher;

        public Type DocumentType { get; private set; }

        public DocumentIndexer(Type documentType, Directory directory, Analyzer analyzer)
        {
            DocumentType = documentType;
            _directory = directory;
            _writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            _searcher = new IndexSearcher(_writer.GetReader());
        }

        public IndexReader GetReader()
        {
            return _searcher.IndexReader;
        }

        public TopDocs Search(Query query, int topN)
        {
            return _searcher.Search(query, topN);
        }

        public TopFieldDocs Search(Query query, Filter filter, int topN, Sort sort)
        {
            return _searcher.Search(query, filter, topN, sort);
        }

        public FacetResults Facets(Query query, IEnumerable<Facet> facets)
        {
            var searcher = new FacetedSearcher(_searcher);
            return searcher.Search(query, facets);
        }

        public void Index(Document document)
        {
            var keyFieldName = GetKeyFieldName();
            _writer.DeleteDocuments(new Term(keyFieldName, document.GetField(keyFieldName).StringValue));
            _writer.AddDocument(document);
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

                _searcher.Dispose();
                _searcher = new IndexSearcher(_writer.GetReader());
            }
        }

        public void Dispose()
        {
            _writer.Dispose();
            _directory.Dispose();
        }
    }
}