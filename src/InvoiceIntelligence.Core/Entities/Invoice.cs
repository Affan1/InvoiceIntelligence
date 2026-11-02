namespace InvoiceIntelligence.Core.Entities
{
    public class Invoice
    {
        public int ID { get; set; }

        public Guid InvoiceID { get; set; }

        public Guid TenantID { get; set; }

        public required string OriginalFileName { get; set; }

        public required string FilePath { get; set; }

        public required string ContainerName { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset UpdateDate { get; set; }


    }
}
