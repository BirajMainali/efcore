// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.EntityFrameworkCore.Infrastructure;

/// <summary>
///     Relational extension methods for <see cref="IProperty" />.
/// </summary>
public static class RelationalPropertyExtensions
{
    /// <summary>
    ///     Creates a comma-separated list of column names.
    /// </summary>
    /// <param name="properties">The properties to format.</param>
    /// <param name="storeObject">The identifier of the table-like store object containing the column.</param>
    /// <returns>A comma-separated list of column names.</returns>
    public static string FormatColumns(
        this IEnumerable<IReadOnlyProperty> properties,
        StoreObjectIdentifier storeObject)
        => "{" + string.Join(", ", properties.Select(p => "'" + p.GetColumnName(storeObject) + "'")) + "}";

    /// <summary>
    ///     Creates a list of column names.
    /// </summary>
    /// <param name="properties">The properties to format.</param>
    /// <param name="storeObject">The identifier of the table-like store object containing the column.</param>
    /// <returns>A list of column names.</returns>
    public static IReadOnlyList<string>? GetColumnNames(
        this IEnumerable<IReadOnlyProperty> properties,
        in StoreObjectIdentifier storeObject)
    {
        var propertyNames = new List<string>();
        foreach (var property in properties)
        {
            var columnName = property.GetColumnName(storeObject);

            // entity mapped to json can have part of it's key mapped to a column (i.e. part that is FK pointing to the PK of the owner)
            // and part that is not (i.e. synthesized key property based on ordinal in the collection)
            if (property.DeclaringEntityType.MappedToJson())
            {
                if (columnName != null)
                {
                    propertyNames.Add(columnName);
                }
            }
            else
            {
                if (columnName == null)
                {
                    return null;
                }

                propertyNames.Add(columnName);
            }
        }

        return propertyNames;
    }
}
