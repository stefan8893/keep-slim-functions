using System.Globalization;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KeepSlim.Functions.BodyData;

public class DeleteBodyData(ILogger<BodyDataFunction> logger, TableClient bodyDataTableClient)
{
    public async Task<ActionResult> Execute(IQueryCollection queryString)
    {
        logger.LogInformation("Deleting body data record");
        logger.LogDebug("Query String: {queryString}", queryString);

        if (!DateTime.TryParse(queryString["recordedAt"], out var recordedAt))
        {
            logger.LogError("Missing record identifier.");
            var now = DateTime.Now.ToString(Constants.RowKeyDateTimeFormatString, CultureInfo.InvariantCulture);
            return new BadRequestObjectResult(
                $"Missing record identifier. e.g. recordedAt={now}");
        }
        
        var rowKey = recordedAt.ToString(Constants.RowKeyDateTimeFormatString, CultureInfo.InvariantCulture);
        logger.LogInformation("Deleting record with row key '{rowKey}'", rowKey);
        await bodyDataTableClient.DeleteEntityAsync("body_data", rowKey);
        
        return new NoContentResult();
    }
}