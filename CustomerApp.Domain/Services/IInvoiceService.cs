using CustomerApp.Domain.Models;

namespace CustomerApp.Domain.Services
{
    public interface IInvoiceService
    {
        Task AddInvoiceAsync(int customerId, DateTime date, int paymentTermsId);
        Task AddLineItemAsync(int invoiceId, string description, decimal amount);
        Task<Invoice> GetInvoiceByIdAsync(int invoiceId);
    }
}
