using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kooboo.Commerce.IO
{
    class FileContent<T>
    {
        public T Data { get; set; }

        public FileContent(T data)
        {
            Data = data;
        }
    }

    class CachedFile<T>  : IDisposable
        where T : class
    {
        private ReaderWriterLockedFile _file;
        private Lazy<FileContent<T>> _cache;

        private Func<T, string> _serialize;
        private Func<string, T> _deserialize;

        public CachedFile(string path, Func<T, string> serialize, Func<string, T> deserialize)
        {
            _file = new ReaderWriterLockedFile(path);
            _serialize = serialize;
            _deserialize = deserialize;
            _cache = new Lazy<FileContent<T>>(Reload, true);
        }

        private FileContent<T> Reload()
        {
            var json = _file.Read();
            if (String.IsNullOrWhiteSpace(json))
            {
                return new FileContent<T>(null);
            }

            return new FileContent<T>(_deserialize(json));
        }

        /// <summary>
        /// Read content from the cache.
        /// Note that if the result object is mutable, you might want to return a cloned copy to your api users for safety.
        /// </summary>
        public T Read()
        {
            return _cache.Value.Data;
        }

        public void Write(T data)
        {
            var json = data == null ? String.Empty : _serialize(data);

            _file.Write(json);
            InvalidateCache();
        }

        public void Delete()
        {
            _file.Delete();
            InvalidateCache();
        }

        private void InvalidateCache()
        {
            _cache = new Lazy<FileContent<T>>(Reload, true);
        }

        public void Dispose()
        {
            _file.Dispose();
        }
    }
}
