using Kooboo.Commerce.Recommendations.Engine.Behaviors;
using Kooboo.Commerce.Recommendations.Engine.Collaborative;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Jobs
{
    public class RecomputeSimilarityMatrixJob : IJob
    {
        private int _updateBatchSize = 100;

        public int UpdateBatchSize
        {
            get { return _updateBatchSize; }
            set { _updateBatchSize = value; }
        }

        public void Execute(JobContext context)
        {
            var instance = context.Instance;
            var behaviorType = context.JobData["BehaviorType"];

            var matrix = SimilarityMatrixes.GetMatrix(instance, behaviorType);
            var newMatrix = matrix.PrepareRecomputation();
            Recompute(newMatrix, BehaviorStores.Get(instance, behaviorType));
            matrix.ReplaceWith(newMatrix);
        }

        private void Recompute(ISimilarityMatrix matrix, IBehaviorStore behaviorStore)
        {
            var allItems = behaviorStore.GetAllItems().ToList();
            if (allItems.Count == 0)
            {
                return;
            }

            var calculator = new ItemSimilarityCalculator(behaviorStore, NullItemPopularityProvider.Instance);
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