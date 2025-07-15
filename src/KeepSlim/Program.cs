using Azure.Data.Tables;
using Azure.Identity;
using KeepSlim.Functions.BodyData;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddSingleton(_ =>
{
    var storageUri = Environment.GetEnvironmentVariable("STORAGE_ACCOUNT_URI") ??
                     throw new ArgumentNullException("STORAGE_ACCOUNT_URI", "Missing storage uri");
    var tableName = Environment.GetEnvironmentVariable("STORAGE_ACCOUNT_TABLE") ??
                    throw new ArgumentNullException("STORAGE_ACCOUNT_TABLE", "Missing storage table name");

    var credential = new DefaultAzureCredential();
    return new TableClient(new Uri(storageUri), tableName, credential);
});

builder.Services.AddScoped<GetBodyData>();
builder.Services.AddScoped<DeleteBodyData>();

builder.Build().Run();