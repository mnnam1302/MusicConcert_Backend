using Contracts.Enumerations;
using MongoDB.Bson.Serialization.Conventions;

namespace Contracts.Extensions;

public static class SortOrderExtension
{
    public static string ConvertStringToSortOrder(string? sortOrder)
    {
        return !string.IsNullOrWhiteSpace(sortOrder)
            ? sortOrder.ToUpper().Equals("ASC")
            ? SortOrder.Ascending : SortOrder.Descending
            : SortOrder.Descending;
    }
}