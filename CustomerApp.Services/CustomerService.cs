using CustomerApp.Data;
using CustomerApp.Domain.Models;
using CustomerApp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;

namespace CustomerApp.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetCustomersInGroupAsync(char start, char end)
        {
            string upperStart = start.ToString().ToUpper();
            string upperEnd = end.ToString().ToUpper();

            return await _context.Customers
                .Where(c => !c.IsDeleted &&
                            !string.IsNullOrEmpty(c.Name) &&
                            string.Compare(c.Name.Substring(0, 1), upperStart) >= 0 &&
                            string.Compare(c.Name.Substring(0, 1), upperEnd) <= 0)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<Customer> GetCustomerWithInvoicesAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.Invoices)
                    .ThenInclude(i => i.LineItems)
                .Include(c => c.Invoices)
                    .ThenInclude(i => i.PaymentTerms)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                customer.IsDeleted = true;
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UndoDeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                customer.IsDeleted = false;
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
