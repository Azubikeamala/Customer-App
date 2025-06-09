using CustomerApp.Domain.Models;

namespace CustomerApp.Domain.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetCustomersInGroupAsync(char start, char end);
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<Customer> GetCustomerWithInvoicesAsync(int id);
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task SoftDeleteCustomerAsync(int id);
        Task UndoDeleteCustomerAsync(int id);
    }
}
