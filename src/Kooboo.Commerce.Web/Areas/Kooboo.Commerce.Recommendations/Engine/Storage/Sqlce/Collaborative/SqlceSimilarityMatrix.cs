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
        public string InstanceName { get; private set; }

        public string MatrixName { get; private set; }

        public SqlceSimilarityMatrix(string instanceName, string matrixName)
        {
            Require.NotNullOrEmpty(instanceName, "instanceName");
            Require.NotNullOrEmpty(matrixName, "matrixName");

            InstanceName = instanceName;
            MatrixName = matrixName;
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

        public IDictionary<string, double> GetMostSimilarItems(string itemId, int topN)
        {
            using (var db = CreateDbContext())
            {
                // 如果是对称阵，则 similarity(item1, item2) 和 similarity(item2, item1) 是相等的，但必须冗余保存两份，否则此处不方便查询
                var items = db.Similarities.Where(it => it.Item1 == itemId)
                                           .OrderByDescending(it => it.Similarity)
                                           .Take(topN)
                                           .ToList();

                return items.ToDictionary(it => it.Item2, it => it.Similarity);
            }
        }

        public void UpdateSimilarity(string item1, string item2, double similarity)
        {
            UpdateSimilarities(new Dictionary<ItemPair, double> { { new ItemPair(item1, item2), similarity } });
        }

        public void UpdateSimilarities(IDictionary<ItemPair, double> similarities)
        {
            using (var db = CreateDbContext())
            {
                foreach (var each in similarities)
                {
                    if (each.Value == 0)
                    {
                        db.Database.ExecuteSqlCommand("delete from " + typeof(ItemSimilarity).Name + " where Item1=@p1 and Item2=@p2", each.Key.Item1, each.Key.Item2);
                    }
                    else
                    {
                        var similarity = db.Similarities.Find(new[] { each.Key.Item1, each.Key.Item2 });
                        if (similarity == null)
                        {
                            similarity = new ItemSimilarity
                            {
                                Item1 = each.Key.Item1,
                                Item2 = each.Key.Item2,
                                Similarity = each.Value
                            };
                            db.Similarities.Add(similarity);
                        }
                        else
                        {
                            similarity.Similarity = each.Value;
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        public ISimilarityMatrix CreateSnapshot()
        {
            return new SqlceSimilarityMatrix(InstanceName, MatrixName + "_snapshot");
        }

        private SimilarityMatrixDbContext CreateDbContext()
        {
            return new SimilarityMatrixDbContext(InstanceName, MatrixName);
        }

        public void ReplaceWith(ISimilarityMatrix snapshot)
        {
            var snapshotDb = snapshot as SqlceSimilarityMatrix;
            var currentDbPath = Paths.Database(InstanceName, MatrixName);
            var tempDbPath = Paths.Database(InstanceName, MatrixName + "_tmp");
            var newDbPath = Paths.Database(snapshotDb.InstanceName, snapshotDb.MatrixName);

            File.Move(currentDbPath, tempDbPath);
            File.Move(newDbPath, currentDbPath);
            File.Delete(tempDbPath);
        }
    }
}