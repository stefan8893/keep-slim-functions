using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace KeepSlim.Functions.CsvImport;

[UsedImplicitly]
public class CsvImportFunction(ImportCsv importCsv)
{
    [Function("csv-import")]
    public Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest request)
    {
        var isDryRunKeyPresent = request.Query.ContainsKey("dryRun");
        var isDryRunValuePresent = bool.TryParse(request.Query["dryRun"], out var dryRunValue);
        var dryRun = isDryRunValuePresent ? dryRunValue : isDryRunKeyPresent;

        return importCsv.Execute(request, dryRun);
    }
}