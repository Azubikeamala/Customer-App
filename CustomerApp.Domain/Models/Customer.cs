using System.ComponentModel.DataAnnotations;

namespace CustomerApp.Domain.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Address1 { get; set; }

        public string? Address2 { get; set; }

        [Required]
        public required string City { get; set; }

        [Required]
        [StringLength(2, ErrorMessage = "State must be 2 characters")]
        public required string State { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        public required string PostalCode { get; set; }

        [Required]
        [Phone]
        public required string Phone { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Contact Email")]
        public required string ContactEmail { get; set; }

        public bool IsDeleted { get; set; } = false;

        // Assuming Invoice is in the same namespace
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
