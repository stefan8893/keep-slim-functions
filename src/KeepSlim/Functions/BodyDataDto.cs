using KeepSlim.Functions.CsvImport;

namespace KeepSlim.Functions;

public class BodyDataDto
{
    public required DateTime RecordedAt { get; init; }
    public required double Weight { get; init; }
    public required double MuscleMass { get; init; }
    public required double BodyFat { get; init; }
    public required double Water { get; init; }
    public required double Bmi { get; init; }
    public required int DailyCalorieRequirement { get; init; }

    public static BodyDataDto FromCsv(CsvBodyDataRecord csvEntry)
    {
        return new BodyDataDto
        {
            RecordedAt = csvEntry.RecordedAt,
            Weight = csvEntry.Gewicht,
            MuscleMass = csvEntry.Muskel,
            BodyFat = csvEntry.Fett,
            Water = csvEntry.Wasser,
            Bmi = csvEntry.BMI,
            DailyCalorieRequirement = csvEntry.Kalorienverbrauch
        };
    }
    
    public static BodyDataDto FromBodyDataTableEntity(BodyDataTableEntity tableEntity)
    {
        return new BodyDataDto
        {
            RecordedAt = tableEntity.RecordedAt,
            Weight = tableEntity.Weight,
            MuscleMass = tableEntity.MuscleMass,
            BodyFat = tableEntity.BodyFat,
            Water = tableEntity.BodyWater,
            Bmi = tableEntity.Bmi,
            DailyCalorieRequirement = tableEntity.DailyCalorieRequirement,
        };
    }
}