namespace CustomerApp.Domain.Models
{
    public class PaymentTerms
    {
        public int Id { get; set; }

        public required string Label { get; set; }

        public int DueDays { get; set; }

        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
