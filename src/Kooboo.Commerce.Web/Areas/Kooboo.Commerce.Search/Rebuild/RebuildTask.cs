using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using Lucene.Net.Documents;
using Lucene.Net.Store;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace Kooboo.Commerce.Search.Rebuild
{
    public class RebuildTask
    {
        private ManualResetEventSlim _cancelledEvent = new ManualResetEventSlim(false);
        private bool _cancelling = false;
        private readonly object _statusLock = new object();
        private IIndexSource _source;
        private IRebuildInfoManager _taskInfoManager;

        public int Progress { get; private set; }

        public RebuildStatus Status { get; private set; }

        public RebuildTaskContext Context { get; private set; }

        public Func<ICommerceInstanceManager> GetCommerceInstanceManager = () => EngineContext.Current.Resolve<ICommerceInstanceManager>();

        public RebuildTask(IIndexSource source, RebuildTaskContext context)
            : this(source, context, new FileBasedRebuildInfoManager())
        {
        }

        public RebuildTask(IIndexSource source, RebuildTaskContext context, IRebuildInfoManager taskInfoManager)
        {
            Require.NotNull(source, "source");
            Require.NotNull(context, "context");
            Require.NotNull(taskInfoManager, "taskInfoManager");

            _source = source;
            _taskInfoManager = taskInfoManager;
            Context = context;
        }

        public RebuildInfo GetTaskInfo()
        {
            return _taskInfoManager.Load(new IndexKey(Context.Instance, Context.ModelType, Context.Culture)) ?? new RebuildInfo();
        }

        public void Cancel()
        {
            if (Status != RebuildStatus.Running)
            {
                return;
            }

            lock (_statusLock)
            {
                if (Status != RebuildStatus.Running)
                {
                    return;
                }

                _cancelling = true;
                _cancelledEvent.Wait();
                _cancelledEvent.Reset();
            }
        }

        public void Start()
        {
            if (Status == RebuildStatus.Running)
            {
                return;
            }

            lock (_statusLock)
            {
                if (Status == RebuildStatus.Running)
                {
                    return;
                }

                Status = RebuildStatus.Running;
                new Thread(ThreadStart).Start();
            }
        }

        private void ThreadStart()
        {
            using (var instance = GetCommerceInstanceManager().GetInstance(Context.Instance))
            {
                using (var scope = Scope.Begin(instance))
                {
                    DoWork(instance);
                }
            }
        }

        private void DoWork(CommerceInstance instance)
        {
            IndexStore store = null;

            try
            {
                var rebuildDirectory = IndexStores.GetDirectory(Context.Instance, Context.Culture, Context.ModelType, true);
                var liveDirectory = IndexStores.GetDirectory(Context.Instance, Context.Culture, Context.ModelType, false);

                // Ensure temp folder are deleted (last rebuild might encounter errors when deleting the temp folder)
                Kooboo.IO.IOUtility.DeleteDirectory(rebuildDirectory, true);
                Kooboo.IO.IOUtility.DeleteDirectory(liveDirectory + "-tmp", true);

                var total = _source.Count(instance, Context.Culture);
                var totalRebuilt = 0;

                Progress = 0;

                store = new IndexStore(Context.ModelType, FSDirectory.Open(rebuildDirectory), Analyzers.GetAnalyzer(Context.Culture));

                foreach (var data in _source.Enumerate(instance, Context.Culture))
                {
                    if (_cancelling)
                    {
                        break;
                    }

                    store.Index(data);

                    totalRebuilt++;
                    Progress = (int)Math.Round(totalRebuilt * 100 / (double)total);
                }

                if (_cancelling)
                {
                    store.Dispose();

                    UpdateTaskInfo(info =>
                    {
                        info.ClearError();
                        info.LastRebuildStatus = RebuildStatus.Cancelled;
                    });

                    _cancelling = false;
                    _cancelledEvent.Set();

                    Status = RebuildStatus.Cancelled;
                }
                else
                {
                    store.Commit();
                    store.Dispose();

                    UpdateTaskInfo(info =>
                    {
                        info.ClearError();
                        info.LastRebuildStatus = RebuildStatus.Success;
                        info.LastSucceededRebuildTimeUtc = DateTime.UtcNow;
                    });

                    // Replace old index files with the new ones

                    IndexStores.Close(Context.Instance, Context.Culture, Context.ModelType);

                    var liveDirectoryExists = System.IO.Directory.Exists(liveDirectory);
                    if (liveDirectoryExists)
                    {
                        System.IO.Directory.Move(liveDirectory, liveDirectory + "-tmp");
                        Kooboo.IO.IOUtility.DeleteDirectory(liveDirectory, true);
                    }

                    System.IO.Directory.Move(rebuildDirectory, liveDirectory);

                    if (liveDirectoryExists)
                    {
                        Kooboo.IO.IOUtility.DeleteDirectory(liveDirectory + "-tmp", true);
                    }

                    Status = RebuildStatus.Success;
                }

                Progress = 0;
            }
            catch (Exception ex)
            {
                if (store != null)
                {
                    store.Dispose();
                }

                UpdateTaskInfo(info =>
                {
                    info.LastRebuildStatus = RebuildStatus.Failed;
                    info.LastRebuildError = ex.Message;
                    info.LastRebuildErrorDetail = ex.StackTrace;
                });

                Status = RebuildStatus.Failed;
            }
        }

        protected void UpdateTaskInfo(Action<RebuildInfo> action)
        {
            var indexKey = new IndexKey(Context.Instance, Context.ModelType, Context.Culture);
            var info = _taskInfoManager.Load(indexKey) ?? new RebuildInfo();
            action(info);
            _taskInfoManager.Save(indexKey, info);
        }
    }
}