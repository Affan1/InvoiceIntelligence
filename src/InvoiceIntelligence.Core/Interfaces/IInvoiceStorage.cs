namespace InvoiceIntelligence.Core.Interfaces
{
    public interface IInvoiceStorage 
    {
        Task<string> SaveAsync(Guid tenantId, Guid invoiceId, Stream content, string contentType, CancellationToken cancellationToken);
    }
}
