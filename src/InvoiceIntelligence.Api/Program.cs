using InvoiceIntelligence.Api.Endpoints;
using InvoiceIntelligence.Core.Interfaces;
using InvoiceIntelligence.Infrastructure.Implementations;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IInvoiceStorage, InvoiceStorage>();

builder.Services.AddAzureClients(async clientBuilder =>
{
    // Register clients for each service
    clientBuilder.AddBlobServiceClient(
        builder.Configuration.GetConnectionString("Storage"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapInvoiceEndpoints();

app.Run();


