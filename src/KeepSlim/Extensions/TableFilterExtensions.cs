using KeepSlim.Functions;

namespace KeepSlim.Extensions;

public static class TableFilterExtensions
{
    public static string ToTableFilter(this (DateTime start, DateTime end) filter)
    {
        var (start, end) = filter;
        var startDateFormatted = start.ToString(Constants.RowKeyDateTimeFormatString);
        var endDateFormatted = end.ToString(Constants.RowKeyDateTimeFormatString);
        
        return
            $"PartitionKey eq 'body_data' and RowKey ge '{startDateFormatted}' and RowKey le '{endDateFormatted}'";
    }
}