using Kooboo.Commerce.API.Expressions;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider
{
    public class LocalObjectQueryExecutor : IQueryExecutor
    {
        private ICommerceDatabase _database;

        public LocalObjectQueryExecutor(ICommerceDatabase database)
        {
            _database = database;
        }

        public object ExecuteScalar<T>(IObjectQuery<T> query, ScalarOptions options) where T : class
        {
            throw new NotImplementedException();
        }

        public List<T> ExecuteList<T>(IObjectQuery<T> query, ListOptions options) where T : class
        {
            // TODO: This is not correct. Because this T is the type of DTO.
            //       So we have to first add non-generic version of Repository.

            var linqQuery = _database.GetRepository<T>().Query();
            if (options.Predicate != null)
            {
                var predicate = new LambdaStringifier().Stringify(options.Predicate);
                linqQuery = linqQuery.Where(predicate);
            }

            if (options.SortDefinitions != null && options.SortDefinitions.Count > 0)
            {
                foreach (var sortDefinition in options.SortDefinitions)
                {
                    linqQuery = linqQuery.OrderBy(sortDefinition.Property + " " + sortDefinition.Direction) as IQueryable<T>;
                }
            }

            if (options.Skip > 0)
            {
                linqQuery = linqQuery.Skips(options.Skip) as IQueryable<T>;
            }
            if (options.Take > 0)
            {
                linqQuery = linqQuery.Takes(options.Take) as IQueryable<T>;
            }

            return linqQuery.ToList();
        }

        public IPagedList<T> ExecutePagedList<T>(IObjectQuery<T> query, PaginationOptions options) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
