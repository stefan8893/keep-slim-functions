using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace KeepSlim.Functions.BodyData;

[UsedImplicitly]
public class BodyDataFunction(GetBodyData getBodyData, DeleteBodyData deleteBodyData)
{
    [Function("body-data")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "delete")] HttpRequest request)
    {
        return request.Method switch
        {
            "GET" => await getBodyData.Execute(request.Query),
            "DELETE" => await deleteBodyData.Execute(request.Query),
            _ => new StatusCodeResult(StatusCodes.Status405MethodNotAllowed)
        };
    }
}