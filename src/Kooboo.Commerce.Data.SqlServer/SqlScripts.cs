using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.SqlServer
{
    internal class SqlScripts
    {
        private StringBuilder _builder = new StringBuilder();

        public static string DropObjects(StoreItemCollection itemCollection)
        {
            var builder = new SqlScripts();

            foreach (var container in itemCollection.GetItems<EntityContainer>())
            {
                var entitySets = container.BaseEntitySets.OfType<EntitySet>().OrderBy(s => s.Name);

                foreach (var associationSet in container.BaseEntitySets.OfType<AssociationSet>().OrderBy(s => s.Name))
                {
                    builder.AppendDropForeignKeys(associationSet);
                }

                foreach (var entitySet in container.BaseEntitySets.OfType<EntitySet>().OrderBy(s => s.Name))
                {
                    builder.AppendDropTable(entitySet);
                }
            }

            return builder.GetCommandText();
        }

        public string GetCommandText()
        {
            return _builder.ToString();
        }

        private void AppendDropTable(EntitySet entitySet)
        {
            AppendObjectExistenceCheck(GetQuotedTableNameWithSchema(entitySet), "U");
            AppendSql(" drop table ");
            AppendTableName(entitySet);
            AppendSql(";");
            AppendNewLine();
        }

        private void AppendDropForeignKeys(AssociationSet associationSet)
        {
            var constraint = associationSet.ElementType.ReferentialConstraints.Single();
            var principalEnd = associationSet.AssociationSetEnds[constraint.FromRole.Name];
            var dependentEnd = associationSet.AssociationSetEnds[constraint.ToRole.Name];

            AppendObjectExistenceCheck(QuoteIdentifier(GetSchemaName(dependentEnd.EntitySet)) + "." + QuoteIdentifier(associationSet.Name), "F");
            AppendNewLine();
            AppendSql("alter table ");
            AppendTableName(dependentEnd.EntitySet);
            AppendSql(" drop constraint ");
            AppendIdentifier(associationSet.Name);
            AppendSql(";");
            AppendNewLine();
        }

        static string GetSchemaName(EntitySet entitySet)
        {
            return entitySet.GetMetadataPropertyValue<string>("Schema") ?? entitySet.EntityContainer.Name;
        }

        static string GetTableName(EntitySet entitySet)
        {
            return entitySet.GetMetadataPropertyValue<string>("Table") ?? entitySet.Name;
        }

        private void AppendObjectExistenceCheck(string objectId, string objectType)
        {
            AppendSql("if (OBJECT_ID('" + objectId + "', '" + objectType + "') IS NOT NULL)");
        }

        private void AppendTableName(EntitySet table)
        {
            AppendSql(GetQuotedTableNameWithSchema(table));
        }

        private void AppendIdentifier(string identifier)
        {
            AppendSql(QuoteIdentifier(identifier));
        }

        private string GetQuotedTableNameWithSchema(EntitySet table)
        {
            var fullTableName = String.Empty;
            var schemaName = GetSchemaName(table);
            var tableName = GetTableName(table);
            if (schemaName != null)
            {
                fullTableName += QuoteIdentifier(schemaName) + ".";
            }

            fullTableName += QuoteIdentifier(tableName);

            return fullTableName;
        }

        private string QuoteIdentifier(string identifier)
        {
            return "[" + identifier.Replace("]", "]]") + "]";
        }

        private void AppendSql(string text)
        {
            _builder.Append(text);
        }

        private void AppendNewLine()
        {
            _builder.Append("\r\n");
        }
    }
}
