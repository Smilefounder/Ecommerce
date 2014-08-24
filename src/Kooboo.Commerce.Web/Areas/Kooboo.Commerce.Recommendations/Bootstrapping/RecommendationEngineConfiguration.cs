﻿using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Folders;
using Kooboo.Commerce.Recommendations.Engine.Behaviors;
using Kooboo.Commerce.Recommendations.Engine.Collaborative;
using Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce.Behaviors;
using Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce.Collaborative;
using Kooboo.Commerce.Recommendations.Engine.Scheduling;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.SqlServerCompact;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Recommendations.Engine;

namespace Kooboo.Commerce.Recommendations.Bootstrapping
{
    public static class RecommendationEngineConfiguration
    {
        public static void Configure()
        {
            // Generate db folder for each instances
            var manager = EngineContext.Current.Resolve<ICommerceInstanceManager>();
            foreach (var instance in manager.GetInstances())
            {
                var folder = DataFolders.Instances.GetFolder(instance.Name).GetFolder("Recommendations");
                if (!folder.Exists)
                {
                    folder.Create();
                }

                Initialize(instance.Name);
            }

            // Configure DbConfiguration
            DbConfiguration.Loaded += DbConfiguration_Loaded;
        }

        static void DbConfiguration_Loaded(object sender, System.Data.Entity.Infrastructure.DependencyResolution.DbConfigurationLoadedEventArgs e)
        {
            e.AddDependencyResolver(new SingletonDependencyResolver<DbProviderServices>(
                SqlCeProviderServices.Instance, SqlCeProviderServices.ProviderInvariantName), true);
        }

        public static void Initialize(string instance)
        {
            foreach (var behaviorType in BehaviorTypes.All())
            {
                SqlceBehaviorStores.Set(instance, behaviorType, new SqlceBehaviorStore(instance, behaviorType));

                var matrixName = GetSimilarityMatrixName(behaviorType);
                var matrix = new SqlceSimilarityMatrix(instance, matrixName);
                SimilarityMatrixes.SetMatrix(instance, matrixName, matrix);
                RelatedItemsReaders.AddReader(instance, new ItemToItemRelatedItemsReader(matrix));
            }

            // TODO: Make configurable in backend
            BehaviorObservers.Add(instance, new BufferedBehaviorObserver(new SqlceBehaviorStoreUpdater(instance), 1000, TimeSpan.FromSeconds(10)));
            RecommendationEngines.Set(instance, new AggregateRecommendationEngine(CreateRecommendationEngines(instance)));

            Schedulers.Start(instance);
            ScheduleJobs(instance);
        }

        public static void Dispose(string instance)
        {
            RecommendationEngines.Remove(instance);

            foreach (var behaviorType in BehaviorTypes.All())
            {
                Schedulers.Stop(instance);
                BehaviorObservers.Remove(instance);
                SqlceBehaviorStores.Remove(instance);
                SimilarityMatrixes.RemoveMatrix(instance);
                RelatedItemsReaders.RemoveReaders(instance);
            }
        }

        static void ScheduleJobs(string instance)
        {
            var scheduler = Schedulers.Get(instance);

            foreach (var behaviorType in BehaviorTypes.All())
            {
                var job = new RecomputeSimilarityMatrixJob(
                    "Recompute " + behaviorType + " matrix"
                    , SimilarityMatrixes.GetMatrix(instance, GetSimilarityMatrixName(behaviorType))
                    , SqlceBehaviorStores.Get(instance, behaviorType)
                    , SqlceBehaviorStores.Get(instance, behaviorType)
                    , SqlceBehaviorStores.Get(instance, behaviorType)
                    , NullItemPopularityReader.Instance);

                // TODO: Make configurable in backend, current configuration is only used for testing
                scheduler.Schedule(job, interval: TimeSpan.FromMinutes(2), startTimeUtc: DateTime.UtcNow.AddSeconds(10));
            }
        }

        static string GetSimilarityMatrixName(string behaviorType)
        {
            return "SimilarityMatrix_" + behaviorType;
        }

        static IEnumerable<IRecommendationEngine> CreateRecommendationEngines(string instance)
        {
            foreach (var behaviorType in BehaviorTypes.All())
            {
                var featureBuilder = new BehaviorBasedFeatureBuilder(() =>
                {
                    var store = SqlceBehaviorStores.Get(instance, behaviorType);
                    return store.GetRecentBehaviors(50);
                });

                yield return new FeatureBasedRecommendationEngine(featureBuilder, RelatedItemsReaders.GetReaders(instance));
            }
        }
    }
}