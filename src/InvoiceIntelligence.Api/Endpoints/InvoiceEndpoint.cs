using InvoiceIntelligence.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceIntelligence.Api.Endpoints
{
    public static class InvoiceEndpoints
    {
        public static void MapInvoiceEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/invoices");

            // POST /invoices
            group.MapPost("/", async ([FromHeader(Name ="X-Tenant-Id")] Guid tenantId, IFormFile file, IInvoiceStorage storage, CancellationToken cancellationToken) =>
            {
                Guid invoiceId = Guid.CreateVersion7();   // the public id (Option B)

                if(file.ContentType != "application/pdf")
                {
                    return Results.BadRequest("Only pdf allowed.");
                }


                if(file.Length > 5 * 1024 * 1024)
                {
                    return Results.BadRequest("Only 5MB Max");
                }
                using var stream = file.OpenReadStream();

                string blobName = await storage.SaveAsync(tenantId, invoiceId, stream, file.ContentType, cancellationToken);

                return Results.Created($"/invoices/{invoiceId}", blobName);
            })
            .DisableAntiforgery();   // API called by programs, not browser forms
        }
    }
}