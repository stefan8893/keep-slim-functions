using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeepSlim;

public class HttpAuthExample(ILogger<HttpAuthExample> logger)
{
    private readonly ILogger<HttpAuthExample> _logger = logger;

    [Function("HttpAuthExample")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.User, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions! It works again");
    }
}
