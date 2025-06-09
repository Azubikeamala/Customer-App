using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerApp.Domain.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public decimal AmountPaid { get; set; }

        // Foreign Key Relationships
        public int CustomerId { get; set; }
        public required Customer Customer { get; set; }

        public int PaymentTermsId { get; set; }
        public required PaymentTerms PaymentTerms { get; set; }

        public ICollection<LineItem> LineItems { get; set; } = new List<LineItem>();

        [NotMapped]
        public DateTime DueDate => Date.AddDays(PaymentTerms?.DueDays ?? 30);
    }
}
