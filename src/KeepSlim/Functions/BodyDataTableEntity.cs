using System.Globalization;
using System.Runtime.Serialization;
using Azure;
using Azure.Data.Tables;

namespace KeepSlim.Functions;

public class BodyDataTableEntity : ITableEntity
{
    public string PartitionKey { get; set; } = "body_data";
    public required string RowKey { get; set; }

    [IgnoreDataMember]
    public DateTime RecordedAt =>
        DateTime.ParseExact(RowKey, Constants.RowKeyDateTimeFormatString, CultureInfo.InvariantCulture);

    public required double Weight { get; init; }
    public required double MuscleMass { get; init; }
    public required double BodyFat { get; init; }
    public required double BodyWater { get; init; }
    public required double Bmi { get; init; }
    public required int DailyCalorieRequirement { get; init; }
    
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}