using System.Globalization;
using CsvHelper.Configuration;
using JetBrains.Annotations;

namespace KeepSlim.Functions.CsvImport;

[UsedImplicitly]
public class CsvBodyDataRecord
{
    public required string Datum { get; set; }
    public required string Zeit { get; set; }

    public DateTime RecordedAt =>
        DateTime.ParseExact($"{Datum} {Zeit}", "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);

    public required double Gewicht { get; set; }
    public required double Fett { get; set; }
    public required double Muskel { get; set; }
    public required double BMI { get; set; }
    public required double Wasser { get; set; }
    public required int Kalorienverbrauch { get; set; }
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

public class CsvBodyDataRecordMap : ClassMap<CsvBodyDataRecord>
{
    public CsvBodyDataRecordMap()
    {
        Map(m => m.Datum).Index(0);
        Map(m => m.Zeit).Index(1);
        Map(m => m.Gewicht).Index(2).TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
        Map(m => m.Fett).Index(3).TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
        Map(m => m.Muskel).Index(4).TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
        Map(m => m.BMI).Index(5).TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
        Map(m => m.Wasser).Index(6).TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
        Map(m => m.Kalorienverbrauch).Index(7);
        Map(m => m.Geräte).Index(8);
    }
}