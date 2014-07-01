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

        public Func<T, string> Serialize;

        public Func<string, T> Deserialize;

        public CachedFile(string path, Func<T, string> serialize, Func<string, T> deserialize)
        {
            _file = new ReaderWriterLockedFile(path);
            Serialize = serialize;
            Deserialize = deserialize;
            _cache = new Lazy<FileContent<T>>(Reload, true);
        }

        private FileContent<T> Reload()
        {
            var json = _file.Read();
            if (String.IsNullOrWhiteSpace(json))
            {
                return new FileContent<T>(null);
            }

            return new FileContent<T>(Deserialize(json));
        }

        public T Read()
        {
            return _cache.Value.Data;
        }

        public void Write(T data)
        {
            var json = data == null ? String.Empty : Serialize(data);

            _file.Write(json);
            _cache = new Lazy<FileContent<T>>(Reload, true);
        }

        public void Dispose()
        {
            _file.Dispose();
        }
    }
}
