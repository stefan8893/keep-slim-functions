using System.Globalization;
using Azure.Data.Tables;

namespace KeepSlim.Functions.Http;

public class BodyDataDto
{
    public required DateTime RecordedAt { get; init; }
    public double Weight { get; init; }
    public double MuscleMass { get; init; }
    public double BodyFat { get; init; } = 0;
    public double Water { get; init; } = 0;
    public double Bmi { get; init; } = 0;

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