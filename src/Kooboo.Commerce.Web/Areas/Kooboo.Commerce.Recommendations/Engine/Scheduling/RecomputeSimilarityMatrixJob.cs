using Kooboo.Commerce.Recommendations.Engine.Behaviors;
using Kooboo.Commerce.Recommendations.Engine.Collaborative;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Scheduling
{
    public class RecomputeSimilarityMatrixJob : IJob
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private ISimilarityMatrix _matrix;
        private IItemsReader _itemsReader;
        private IUserItemRelationReader _userItemRelationReader;
        private IBehaviorTimestampReader _behaviorTimestampReader;
        private IItemPopularityReader _itemPopularityReader;

        public string Id { get; private set; }

        public RecomputeSimilarityMatrixJob(string jobId, ISimilarityMatrix matrix, IItemsReader itemsReader, IUserItemRelationReader userItemRelationReader, IBehaviorTimestampReader behaviorTimestampReader, IItemPopularityReader itemPopularityReader)
        {
            Id = jobId;
            _matrix = matrix;
            _itemsReader = itemsReader;
            _userItemRelationReader = userItemRelationReader;
            _behaviorTimestampReader = behaviorTimestampReader;
            _itemPopularityReader = itemPopularityReader;
        }

        public void Execute()
        {
            var snapshot = _matrix.CreateSnapshot();
            Recompute(snapshot);
            _matrix.ReplaceWith(snapshot);
        }

        private void Recompute(ISimilarityMatrix snapshot)
        {
            var allItems = _itemsReader.GetItems().ToList();
            if (allItems.Count == 0)
            {
                return;
            }

            var calculator = new ItemSimilarityCalculator(_userItemRelationReader, _behaviorTimestampReader, _itemPopularityReader);

            foreach (var item in allItems)
            {
                foreach (var other in allItems)
                {
                    if (item == other)
                    {
                        continue;
                    }

                    var similarity = calculator.CalculateSimilarity(item, other);
                    snapshot.UpdateSimilarity(item, other, similarity);
                }
            }
        }
    }
}