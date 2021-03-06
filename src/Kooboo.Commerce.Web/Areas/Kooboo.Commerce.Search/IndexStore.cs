﻿using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;

namespace Kooboo.Commerce.Search
{
    public class IndexStore : IDisposable
    {
        private Directory _directory;
        private readonly object _lock = new object();
        private Analyzer _analyzer;
        private IndexWriter _writer;
        private IndexSearcher _searcher;

        public Type ModelType { get; private set; }

        public IndexStore(Type modelType, Directory directory, Analyzer analyzer)
        {
            ModelType = modelType;
            _directory = directory;
            _analyzer = analyzer;
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

        public IndexQuery Query()
        {
            return new IndexQuery(ModelType, GetSearcher(), _analyzer);
        }

        public void Index(object model)
        {
            var doc = ModelConverter.ToDocument(model);
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
            return EntityKey.GetKeyProperty(ModelType).Name;
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