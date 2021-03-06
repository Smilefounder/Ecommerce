﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Folders
{
    public class CachedDataFile : DataFile
    {
        private DataFile _file;
        private CacheContainer _cache;

        public CachedDataFile(DataFile file)
            : base(file.VirtualPath, file.Format)
        {
            _file = file;
        }

        public override bool Exists
        {
            get
            {
                return _file.Exists;
            }
        }

        public override object Read(Type type)
        {
            if (_cache == null)
            {
                lock (_file)
                {
                    if (_cache == null)
                    {
                        _cache = new CacheContainer
                        {
                            Content = _file.Read(type)
                        };
                    }
                }
            }

            return _cache.Content;
        }

        public override void Write(object content)
        {
            lock (_file)
            {
                _file.Write(content);
                _cache = null;
            }
        }

        public override void Delete()
        {
            lock (_file)
            {
                _file.Delete();
                _cache = null;
            }
        }

        class CacheContainer
        {
            public object Content;
        }
    }
}
