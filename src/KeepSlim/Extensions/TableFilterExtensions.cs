using System.Globalization;
using KeepSlim.Functions;

namespace KeepSlim.Extensions;

public static class TableFilterExtensions
{
    public static string ToTableFilter(this (DateTime start, DateTime end) filter)
    {
        var (start, end) = filter;
        var startDateFormatted = start.ToString(Constants.RowKeyDateTimeFormatString, CultureInfo.InvariantCulture);
        var endDateFormatted = end.ToString(Constants.RowKeyDateTimeFormatString, CultureInfo.InvariantCulture);
        
        return
            $"PartitionKey eq 'body_data' and RowKey ge '{startDateFormatted}' and RowKey le '{endDateFormatted}'";
    }
}