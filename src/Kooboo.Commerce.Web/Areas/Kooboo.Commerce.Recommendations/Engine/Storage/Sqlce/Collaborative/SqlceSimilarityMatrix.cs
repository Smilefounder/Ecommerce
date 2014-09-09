using Kooboo.Commerce.Recommendations.Engine.Collaborative;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce.Collaborative
{
    public class SqlceSimilarityMatrix : ISimilarityMatrix
    {
        public string Instance { get; private set; }

        public string BehaviorType { get; private set; }

        public string DatabaseName { get; private set; }

        public SqlceSimilarityMatrix(string instance, string behaviorType, string databaseName)
        {
            Require.NotNullOrEmpty(instance, "instance");
            Require.NotNullOrEmpty(behaviorType, "behaviorType");
            Require.NotNullOrEmpty(databaseName, "matrixName");

            Instance = instance;
            BehaviorType = behaviorType;
            DatabaseName = databaseName;
        }

        public IEnumerable<string> AllItems()
        {
            using (var db = CreateDbContext())
            {
                return db.Similarities.Select(it => it.Item1).Distinct().ToList();
            }
        }

        public double GetSimilarity(string item1, string item2)
        {
            return GetSimilarities(new ItemPair(item1, item2))[0];
        }

        public double[] GetSimilarities(params ItemPair[] itemPairs)
        {
            var result = new double[itemPairs.Length];

            using (var db = CreateDbContext())
            {
                for (var i = 0; i < itemPairs.Length; i++)
                {
                    var pair = itemPairs[i];
                    if (pair.Item1 == pair.Item2)
                    {
                        result[i] = 1;
                    }
                    else
                    {
                        var similarity = db.Similarities.Find(new[] { pair.Item1, pair.Item2 });
                        if (similarity != null)
                        {
                            result[i] = similarity.Similarity;
                        }
                        else
                        {
                            result[i] = 0;
                        }
                    }
                }
            }

            return result;
        }

        public IDictionary<string, double> GetMostSimilarItems(string itemId, int topN, ISet<string> ignoredItems)
        {
            using (var db = CreateDbContext())
            {
                // 因为此处我们不需要非常精确的取出前N个最相似物品，多一些少一些是影响不大的，
                // 所以直接多取一些数据出来，然后再过滤掉要忽略的物品，以免进行多次查询。
                var takeCount = topN;
                if (ignoredItems != null && ignoredItems.Count > 0)
                {
                    takeCount += ignoredItems.Count;
                }

                var items = db.Similarities.Where(it => it.Item1 == itemId)
                                           .OrderByDescending(it => it.Similarity)
                                           .Take(takeCount)
                                           .ToList();

                var result = new Dictionary<string, double>();
                foreach (var item in items)
                {
                    if (ignoredItems == null || ignoredItems.Count == 0 || !ignoredItems.Contains(item.Item2))
                    {
                        result.Add(item.Item2, item.Similarity);
                    }
                }

                return result;
            }
        }

        public void AddSimilarities(IDictionary<ItemPair, double> similarities)
        {
            using (var db = CreateDbContext())
            {
                foreach (var each in similarities)
                {
                    if (each.Key.Item1 == each.Key.Item2)
                    {
                        continue;
                    }

                    // 对称阵，则 similarity(item1, item2) 和 similarity(item2, item1) 是相等的，但必须冗余保存两份，否则不方便查询
                    db.Similarities.Add(new ItemSimilarity
                    {
                        Item1 = each.Key.Item1,
                        Item2 = each.Key.Item2,
                        Similarity = each.Value
                    });
                    db.Similarities.Add(new ItemSimilarity
                    {
                        Item1 = each.Key.Item2,
                        Item2 = each.Key.Item1,
                        Similarity = each.Value
                    });
                }

                db.SaveChanges();
            }
        }

        public ISimilarityMatrix PrepareRecomputation()
        {
            return new SqlceSimilarityMatrix(Instance, BehaviorType, DatabaseName + "_snapshot_" + Guid.NewGuid().ToString("N"));
        }

        private SimilarityMatrixDbContext CreateDbContext()
        {
            return new SimilarityMatrixDbContext(Instance, DatabaseName);
        }

        public void ReplaceWith(ISimilarityMatrix snapshot)
        {
            var snapshotDb = snapshot as SqlceSimilarityMatrix;
            var snapshotDbPath = Paths.Database(snapshotDb.Instance, snapshotDb.DatabaseName);
            // If there's no data in the online matrix, the snapshot database will not be created
            if (!File.Exists(snapshotDbPath))
            {
                return;
            }

            var currentDbPath = Paths.Database(Instance, DatabaseName);

            if (File.Exists(currentDbPath))
            {
                var tempPath = Paths.Database(Instance, DatabaseName + "_tmp");
                File.Move(currentDbPath, tempPath);
                File.Move(snapshotDbPath, currentDbPath);
                File.Delete(tempPath);
            }
            else
            {
                File.Move(snapshotDbPath, currentDbPath);
            }
        }
    }
}