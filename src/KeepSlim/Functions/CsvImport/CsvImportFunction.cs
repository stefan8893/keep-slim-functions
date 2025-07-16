using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace KeepSlim.Functions.CsvImport;

[UsedImplicitly]
public class CsvImportFunction(ImportCsv importCsv)
{
    [Function("csv-import")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest request)
    {
        return request.Method switch
        {
            "GET" => await importCsv.Execute(request, dryRun: true),
            "POST" => await importCsv.Execute(request, dryRun: false),
            _ => new StatusCodeResult(StatusCodes.Status405MethodNotAllowed)
        };
    }
}