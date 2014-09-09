using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Collaborative
{
    public interface ISimilarityMatrix
    {
        IEnumerable<string> AllItems();

        double GetSimilarity(string item1, string item2);

        double[] GetSimilarities(params ItemPair[] itemPairs);

        IDictionary<string, double> GetMostSimilarItems(string itemId, int topN, ISet<string> ignoredItems);

        void AddSimilarities(IDictionary<ItemPair, double> similarities);

        ISimilarityMatrix PrepareRecomputation();

        void ReplaceWith(ISimilarityMatrix newMatrix);
    }
}