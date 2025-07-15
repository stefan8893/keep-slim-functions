using Azure.Data.Tables;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using KeepSlim.Functions;

namespace KeepSlim.Functions.Http;

[UsedImplicitly]
public class QueryBodyData(ILogger<QueryBodyData> logger, TableClient bodyDataTableClient)
{
    [Function("body-data")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest request)
    {
        logger.LogInformation("Querying body data");
        logger.LogDebug("Query String: {queryString}", request.QueryString);
        if (!DateOnly.TryParse(request.Query["startDate"], out var startDate) ||
            !DateOnly.TryParse(request.Query["endDate"], out var endDate))
        {
            logger.LogError("Missing start and/or end date.");
            var today = DateTime.Today;
            return new BadRequestObjectResult(
                $"Missing start or end date. e.g. startDate={today:yyyy-MM-dd}&endDate={today:yyyy-MM-dd}");
        }

        var startDateStartOfDay = startDate.ToDateTime(TimeOnly.MinValue);
        var endDateEndOfDay = endDate.ToDateTime(new TimeOnly(23, 59, 59, 999));
        logger.LogInformation("Fetching body data for the time range: {startDate:o} - {endDate:o}", startDateStartOfDay,
            endDateEndOfDay);


        var bodyData = await GetBodyData(startDateStartOfDay, endDateEndOfDay);
        return new JsonResult(bodyData)
        {
            StatusCode = StatusCodes.Status200OK
        };
    }

    private async Task<IEnumerable<BodyDataDto>> GetBodyData(DateTime startDate, DateTime endDate)
    {
        var filter = ToFilter(startDate, endDate);
        logger.LogInformation("Query filter: {filter}", filter);

        var bodyData = new List<BodyDataDto>();
        await foreach (var page in bodyDataTableClient
                           .QueryAsync<TableEntity>(filter)
                           .AsPages(pageSizeHint: 1000))
        {
            bodyData.AddRange(page.Values.Select(BodyDataDto.FromTableEntity));
        }

        return bodyData;
    }

    private static string ToFilter(DateTime startDate, DateTime endDate)
    {
        var startDateFormatted = startDate.ToString(Constants.RowKeyDateTimeFormatString);
        var endDateFormatted = endDate.ToString(Constants.RowKeyDateTimeFormatString);
        
        return
            $"PartitionKey eq 'body_data' and RowKey ge '{startDateFormatted}' and RowKey le '{endDateFormatted}'";
    }
}