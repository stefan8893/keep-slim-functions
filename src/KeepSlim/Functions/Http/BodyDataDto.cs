using System.Globalization;
using Azure.Data.Tables;

namespace KeepSlim.Functions.Http;

public class BodyDataDto
{
    public required DateTime RecordedAt { get; init; }
    public required double Weight { get; init; }
    public required double MuscleMass { get; init; }
    public required double BodyFat { get; init; }
    public required double Water { get; init; }
    public required double Bmi { get; init; }

    public static BodyDataDto FromTableEntity(TableEntity tableEntity)
    {
        return new BodyDataDto
        {
            RecordedAt = DateTime.ParseExact(tableEntity.RowKey, "yyyy-MM-ddThh:mm:ss", CultureInfo.InvariantCulture),
            Weight = GetDoubleSafe(tableEntity, "Weight"),
            MuscleMass =GetDoubleSafe(tableEntity, "MuscleMass"),
            BodyFat = GetDoubleSafe(tableEntity, "BodyFat"),
            Water = GetDoubleSafe(tableEntity, "BodyWater"),
            Bmi = GetDoubleSafe(tableEntity, "Bmi"),
        };
    }
    
    private static double GetDoubleSafe(TableEntity entity, string key)
    {
        if (!entity.TryGetValue(key, out var val))
            throw new KeyNotFoundException($"Key '{key}' not found.");

        return val switch
        {
            double d => d,
            int i => i,
            long l => l,
            float f => f,
            null => throw new InvalidCastException("Value is null."),
            _ => throw new InvalidCastException($"Value is not of type {val.GetType()} and can therefore not convert to type double.")
        };
    }
}