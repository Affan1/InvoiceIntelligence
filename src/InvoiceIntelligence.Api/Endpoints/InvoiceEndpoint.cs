using InvoiceIntelligence.Core.Interfaces;

namespace InvoiceIntelligence.Api.Endpoints
{
    public static class InvoiceEndpoints
    {
        public static void MapInvoiceEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/invoices");

            // POST /invoices
            group.MapPost("/", async (IFormFile file, IInvoiceStorage storage, CancellationToken cancellationToken) =>
            {
                Guid tenantId = Guid.Empty;               // temporary — comes from a header next
                Guid invoiceId = Guid.CreateVersion7();   // the public id (Option B)

                using var stream = file.OpenReadStream();
                string blobName = await storage.SaveAsync(tenantId, invoiceId, stream, file.ContentType, cancellationToken);

                return Results.Ok(blobName);
            })
            .DisableAntiforgery();   // API called by programs, not browser forms
        }
    }
}