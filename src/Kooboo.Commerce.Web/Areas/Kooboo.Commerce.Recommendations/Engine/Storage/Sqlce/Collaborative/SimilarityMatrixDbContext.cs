using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.SqlServerCompact;
using System.IO;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce.Collaborative
{
    public class SimilarityMatrixDbContext : DbContext
    {
        public DbSet<ItemSimilarity> Similarities { get; set; }

        public SimilarityMatrixDbContext(string instance, string dbName)
            : base(SqlceDbContextHelper.CreateConnection(instance, dbName), SqlceDbContextHelper.GetModel("SimilarityMatrixDbContext"), true)
        {
        }
    }
}