using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kooboo.Commerce.IO
{
    class ReaderWriterLockedFile : IDisposable
    {
        private string _path;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public ReaderWriterLockedFile(string path)
        {
            _path = path;
        }

        public string Read()
        {
            _lock.EnterReadLock();

            try
            {
                return File.ReadAllText(_path, Encoding.UTF8);
            }
            catch (FileNotFoundException)
            {
                // FileNotFound is not an 'Exception' in this case
                return null;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void Write(string contents)
        {
            _lock.EnterWriteLock();

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_path));
                File.WriteAllText(_path, contents, Encoding.UTF8);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Dispose()
        {
            _lock.Dispose();
        }
    }
}
