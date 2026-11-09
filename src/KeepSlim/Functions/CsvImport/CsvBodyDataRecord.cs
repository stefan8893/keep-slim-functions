using System.Globalization;
using CsvHelper;

namespace KeepSlim.Functions.CsvImport;

[CsvRecord]
public partial class CsvBodyDataRecord
{
    [CsvColumn(Index = 0)]
    public required string Datum { get; set; }

    [CsvColumn(Index = 1)]
    public required string Zeit { get; set; }

    public DateTime RecordedAt =>
        DateTime.ParseExact($"{Datum} {Zeit}", "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);

    [CsvColumn(Index = 2)]
    public required double Gewicht { get; set; }

    [CsvColumn(Index = 3)]
    public required double Fett { get; set; }

    [CsvColumn(Index = 4)]
    public required double Muskel { get; set; }

    [CsvColumn(Index = 5)]
    public required double BMI { get; set; }

    [CsvColumn(Index = 6)]
    public required double Wasser { get; set; }

    [CsvColumn(Index = 7)]
    public required int Kalorienverbrauch { get; set; }

    [CsvColumn(Index = 8)]
    public required string Geräte { get; set; }

    public BodyDataTableEntity ToTableEntity()
    {
        return new BodyDataTableEntity
        {
            RowKey = RecordedAt.ToString(Constants.RowKeyDateTimeFormatString, CultureInfo.InvariantCulture),
            Weight = Gewicht,
            MuscleMass = Muskel,
            BodyFat = Fett,
            BodyWater = Wasser,
            Bmi = BMI,
            DailyCalorieRequirement = Kalorienverbrauch,
        };
    }
}