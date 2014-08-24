using Kooboo.Commerce.Recommendations.Engine.Behaviors;
using Kooboo.Commerce.Recommendations.Engine.Collaborative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Tasks
{
    public class RecomputeSimilarityMatrixTask
    {
        private readonly object _lock = new object();
        private ISimilarityMatrix _matrix;
        private IUserItemRelationReader _userItemRelationReader;
        private IBehaviorTimestampReader _behaviorTimestampReader;
        private IItemPopularityReader _itemPopularityReader;

        public string Name { get; private set; }

        public RecomputeSimilarityMatrixTask(string name, ISimilarityMatrix matrix, IUserItemRelationReader userItemRelationReader, IBehaviorTimestampReader behaviorTimestampReader, IItemPopularityReader itemPopularityReader)
        {
            Name = name;
            _matrix = matrix;
            _userItemRelationReader = userItemRelationReader;
            _behaviorTimestampReader = behaviorTimestampReader;
            _itemPopularityReader = itemPopularityReader;
        }

        public bool IsRunning { get; private set; }

        public Task Start()
        {
            if (IsRunning)
                throw new InvalidOperationException("Cannot start a task when it's running.");

            lock (_lock)
            {
                if (IsRunning)
                    throw new InvalidOperationException("Cannot start a task when it's running.");

                IsRunning = true;
            }

            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var snapshot = _matrix.CreateSnapshot();
                    Recompute(snapshot);
                    _matrix.ReplaceWith(snapshot);
                }
                catch (Exception ex)
                {
                    var message = "Similarity recomputation task '" + Name + "' failed: " + ex.Message
                        + Environment.NewLine + ex.StackTrace;
                    Kooboo.CMS.Web.HealthMonitoring.TextFileLogger.Log(message);
                }

                IsRunning = false;
            });
        }

        private void Recompute(ISimilarityMatrix snapshot)
        {
            var allItems = snapshot.AllItems().ToList();
            var calculator = new ItemSimilarityCalculator(_userItemRelationReader, _behaviorTimestampReader, _itemPopularityReader);

            foreach (var item in allItems)
            {
                foreach (var other in allItems)
                {
                    var similarity = calculator.CalculateSimilarity(item, other);
                    snapshot.UpdateSimilarity(item, other, similarity);
                }
            }
        }
    }
}