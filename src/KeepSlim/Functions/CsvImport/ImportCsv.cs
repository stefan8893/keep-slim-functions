using System.Globalization;
using Azure.Data.Tables;
using CsvHelper;
using CsvHelper.Configuration;
using KeepSlim.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OneOf;

namespace KeepSlim.Functions.CsvImport;

public class ImportCsv(ILogger<ImportCsv> logger, TableClient bodyDataTableClient)
{
    public async Task<ActionResult> Execute(HttpRequest request, bool dryRun)
    {
        logger.LogInformation("Importing csv file. dry run: {dryRun}", dryRun);
        var parsingResult = await TryParseCsv(request.Body);

        if (parsingResult.IsT1) return parsingResult.AsT1;
        var csvEntries = parsingResult.AsT0;

        logger.LogInformation("Csv parsed. File contains {count} records", csvEntries.Count);

        var existingBodyData = await LoadBodyDataFromAzureTables(csvEntries);
        var newEntries = FilterNewEntries(existingBodyData, csvEntries);

        logger.LogInformation("Csv contains {count} new records", newEntries.Count);

        if (dryRun)
            return new JsonResult(newEntries.Select(BodyDataDto.FromCsv));

        foreach (var b in newEntries.Select(x => x.ToTableEntity()))
        {
            logger.LogInformation("Add new table entity with row key: {rowKey}", b.RowKey);
            await bodyDataTableClient.AddEntityAsync(b);
        }

        return new JsonResult(newEntries.Select(BodyDataDto.FromCsv));
    }

    private static IReadOnlyList<CsvBodyDataRecord> FilterNewEntries(
        IEnumerable<BodyDataTableEntity> existingBodyDataRecords, IReadOnlyList<CsvBodyDataRecord> csvEntries)
    {
        var existingEntriesByRowKey = existingBodyDataRecords
            .Select(x => x.RecordedAt)
            .ToHashSet();

        return csvEntries
            .Where(x => !existingEntriesByRowKey.Contains(x.RecordedAt))
            .ToList();
    }

    private async Task<IEnumerable<BodyDataTableEntity>> LoadBodyDataFromAzureTables(
        IReadOnlyList<CsvBodyDataRecord> csvEntries)
    {
        if (!csvEntries.Any())
            return [];

        var oldestEntry = csvEntries.Min(x => x.RecordedAt);
        var latestEntry = csvEntries.Max(x => x.RecordedAt);

        var filter = (oldestEntry, latestEntry).ToTableFilter();
        return await bodyDataTableClient
            .QueryAsync<BodyDataTableEntity>(filter)
            .ToListAsync();
    }

    private async Task<OneOf<IReadOnlyList<CsvBodyDataRecord>, BadRequestObjectResult>> TryParseCsv(
        Stream requestBody)
    {
        try
        {
            return await ParseCsv(requestBody);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error while parsing csv file");
            return new BadRequestObjectResult("Invalid csv file");
        }
    }

    private static async Task<OneOf<IReadOnlyList<CsvBodyDataRecord>, BadRequestObjectResult>> ParseCsv(
        Stream requestBody)
    {
        using var memoryStream = new MemoryStream();
        await requestBody.CopyToAsync(memoryStream);

        if (memoryStream.Length == 0)
            return new BadRequestObjectResult("Missing csv file.");

        memoryStream.Position = 0;
        using var reader = new StreamReader(memoryStream);
        using var csvReader = new CsvReader(reader,
            new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true,
                ShouldSkipRecord = ctx => ctx.Row.Parser.Row <= 2,
                IgnoreBlankLines = true
            });

        csvReader.Context.RegisterClassMap<CsvBodyDataRecordMap>();

        return await csvReader
            .GetRecordsAsync<CsvBodyDataRecord>()
            .ToListAsync();
    }
}