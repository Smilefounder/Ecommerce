using Kooboo.Commerce.Recommendations.Engine.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Collaborative
{
    public class SimilarityComputationTask
    {
        private ISimilarityMatrix _matrix;
        private IUserItemRelationReader _userItemRelationReader;
        private IBehaviorTimestampReader _behaviorTimeReader;
        private IItemPopularityReader _popularityReader;

        public SimilarityComputationTask(
            ISimilarityMatrix matrix, IUserItemRelationReader userItemRelationReader, IBehaviorTimestampReader behaviorTimeReader, IItemPopularityReader popularityReader)
        {
            _matrix = matrix;
            _userItemRelationReader = userItemRelationReader;
            _behaviorTimeReader = behaviorTimeReader;
            _popularityReader = popularityReader;
        }

        public void Recompute()
        {
            var all = _matrix.AllItems().ToList();
            Recompute(all, all);
        }

        public void Recompute(IEnumerable<string> items)
        {
            Recompute(items, _matrix.AllItems().ToList());
        }

        private void Recompute(IEnumerable<string> items, List<string> allItems)
        {
            var calculator = new ItemSimilarityCalculator(_userItemRelationReader, _behaviorTimeReader, _popularityReader);

            foreach (var item in items)
            {
                foreach (var other in allItems)
                {
                    var similarity = calculator.CalculateSimilarity(item, other);
                    _matrix.UpdateSimilarity(item, other, similarity);
                }
            }
        }

        static Task _currentTask;
        static readonly object _lock = new object();

        public static Task Current
        {
            get
            {
                return _currentTask;
            }
        }

        public static Task Start(
            ISimilarityMatrix onlineMatrix, IUserItemRelationReader userItemRelationReader, IBehaviorTimestampReader behaviorTimeReader, IItemPopularityReader popularityReader)
        {
            if (_currentTask != null)
            {
                return _currentTask;
            }

            lock (_lock)
            {
                if (_currentTask != null)
                {
                    return _currentTask;
                }

                _currentTask = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var snapshot = onlineMatrix.CreateSnapshot();
                        var task = new SimilarityComputationTask(snapshot, userItemRelationReader, behaviorTimeReader, popularityReader);
                        task.Recompute();
                        onlineMatrix.ReplaceWith(snapshot);
                    }
                    catch (Exception ex)
                    {
                        // TODO: Better logging
                        Kooboo.CMS.Web.HealthMonitoring.TextFileLogger.Log(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                })
                .ContinueWith(_ =>
                {
                    _currentTask = null;
                });

                return _currentTask;
            }
        }
    }
}