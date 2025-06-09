using System.ComponentModel.DataAnnotations;

namespace CustomerApp.Domain.Models
{
    public class LineItem
    {
        public int Id { get; set; }

        [Required]
        public required string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        // Foreign Key
        public int InvoiceId { get; set; }
        public required Invoice Invoice { get; set; }
    }
}
