using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using InvoiceIntelligence.Core.Interfaces;

namespace InvoiceIntelligence.Infrastructure.Implementations
{
    public class InvoiceStorage : IInvoiceStorage   // public, so the Api can register it later
    {
        // The "invoices" cabinet. private = only this class uses it; readonly = set once, never replaced.
        private readonly BlobContainerClient _container;

        public InvoiceStorage(BlobServiceClient serviceClient)   // the building comes in from outside
        {
            // Step 2: walk to the cabinet once and remember it
            _container = serviceClient.GetBlobContainerClient("invoices");
        }

        public async Task<string> SaveAsync(Guid tenantId, Guid invoiceId, Stream content, string contentType, CancellationToken cancellationToken)
        {
            // A brand-new Azurite has no cabinets, so create it if it's missing
            await _container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

            // Step 1: write the label
            string blobName = $"{tenantId}/{invoiceId}.pdf";

            // Step 3: get the slot for that label (building → cabinet → file)
            BlobClient blob = _container.GetBlobClient(blobName);

            // Step 4: put the PDF in, and tell Azure it's a PDF
            var options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
            };
            await blob.UploadAsync(content, options, cancellationToken);

            // Step 5: give back the label
            return blobName;
        }
    }
}