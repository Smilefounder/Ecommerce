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
        private ISimilarityMatrix _matrix;
        private IBehaviorStore _behaviorStore;
        private IItemPopularityProvider _itemPopularityReader;
        private int _updateBatchSize = 100;

        public int UpdateBatchSize
        {
            get { return _updateBatchSize; }
            set { _updateBatchSize = value; }
        }

        public string Id { get; private set; }

        public RecomputeSimilarityMatrixJob(string jobId, ISimilarityMatrix matrix, IBehaviorStore behaviorStore, IItemPopularityProvider itemPopularityReader)
        {
            Id = jobId;
            _matrix = matrix;
            _behaviorStore = behaviorStore;
            _itemPopularityReader = itemPopularityReader;
        }

        public void Execute()
        {
            var newMatrix = _matrix.PrepareRecomputation();
            Recompute(newMatrix);
            _matrix.ReplaceWith(newMatrix);
        }

        private void Recompute(ISimilarityMatrix matrix)
        {
            var allItems = _behaviorStore.GetAllItems().ToList();
            if (allItems.Count == 0)
            {
                return;
            }

            var calculator = new ItemSimilarityCalculator(_behaviorStore, _itemPopularityReader);
            var batch = new Dictionary<ItemPair, double>();

            for (var i = 0; i < allItems.Count; i++)
            {
                for (var j = i + 1; j < allItems.Count; j++)
                {
                    var item1 = allItems[i];
                    var item2 = allItems[j];

                    var similarity = calculator.CalculateSimilarity(item1, item2);
                    if (similarity > 0)
                    {
                        batch.Add(new ItemPair(item1, item2), similarity);

                        if (batch.Count == _updateBatchSize)
                        {
                            matrix.AddSimilarities(batch);
                            batch.Clear();
                        }
                    }
                }
            }

            if (batch.Count > 0)
            {
                matrix.AddSimilarities(batch);
            }
        }
    }
}