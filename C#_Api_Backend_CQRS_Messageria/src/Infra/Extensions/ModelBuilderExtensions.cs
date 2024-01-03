using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Utils.Extensions;

namespace Infra.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static PropertyBuilder<decimal?> HasPrecision(this PropertyBuilder<decimal?> builder, int precision, int scale)
        {
            return builder.HasColumnType($"decimal({precision},{scale})");
        }

        public static PropertyBuilder<decimal> HasPrecision(this PropertyBuilder<decimal> builder, int precision, int scale)
        {
            return builder.HasColumnType($"decimal({precision},{scale})");
        }

        public static void ApplySqlServerNamingConventions(this ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().ToPascalCase());

                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToPascalCase());
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToCamelCase());
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.SetName(index.GetName().ToSnakeCase());
                }
            }
        }

        public static void ApplyPostgresNamingConventions(this ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().ToSnakeCase());

                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToSnakeCase());
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToSnakeCase());
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.SetName(index.GetName().ToSnakeCase());
                }
            }
        }

        private static Dictionary<IProperty, int> _maxLengthMetadataCache;

        private static Dictionary<IProperty, int> GetMaxLengthMetadata(this DbContext db)
        {
            if (_maxLengthMetadataCache == null)
            {
                _maxLengthMetadataCache = new Dictionary<IProperty, int>();

                var entities = db.Model.GetEntityTypes();
                foreach (var entityType in entities)
                {
                    foreach (var property in entityType.GetProperties())
                    {
                        var annotation = property.GetAnnotations().FirstOrDefault(a => a.Name == "MaxLength");
                        if (annotation != null)
                        {
                            var maxLength = Convert.ToInt32(annotation.Value);
                            if (maxLength > 0)
                            {
                                _maxLengthMetadataCache[property] = maxLength;
                            }
                        }
                    }
                }
            }

            return _maxLengthMetadataCache;
        }

        public static void AutoTruncateStringToMaxLength(this DbContext db)
        {
            var entries = db?.ChangeTracker?.Entries();
            if (entries == null)
            {
                return;
            }

            var maxLengthMetadata = db.GetMaxLengthMetadata();

            foreach (var entry in entries)
            {
                var propertyValues = entry.CurrentValues.Properties.Where(p => p.ClrType == typeof(string));

                foreach (var prop in propertyValues)
                {
                    if (entry.CurrentValues[prop.Name] != null)
                    {
                        var stringValue = entry.CurrentValues[prop.Name].ToString();
                        if (maxLengthMetadata.ContainsKey(prop))
                        {
                            var maxLength = maxLengthMetadata[prop];
                            stringValue = TruncateString(stringValue, maxLength);
                        }

                        entry.CurrentValues[prop.Name] = stringValue;
                    }
                }
            }
        }

        private static string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}