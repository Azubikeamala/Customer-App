using CustomerApp.Data;
using CustomerApp.Domain.Models;
using CustomerApp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;

namespace CustomerApp.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _context;

        public InvoiceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddInvoiceAsync(int customerId, DateTime date, int paymentTermsId)
        {
            var invoice = new Invoice
            {
                CustomerId = customerId,
                Date = date,
                PaymentTermsId = paymentTermsId,
                AmountPaid = 0,
                Customer = null!,         
                PaymentTerms = null!
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task AddLineItemAsync(int invoiceId, string description, decimal amount)
        {
            var invoice = await _context.Invoices
                .Include(i => i.LineItems)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice == null) return;

            var lineItem = new LineItem
            {
                InvoiceId = invoiceId,
                Description = description,
                Amount = amount,
                Invoice = null! 
            };

            _context.LineItems.Add(lineItem);
            await _context.SaveChangesAsync(); 

           
            invoice.AmountPaid = await _context.LineItems
                .Where(li => li.InvoiceId == invoiceId)
                .SumAsync(li => li.Amount);

            await _context.SaveChangesAsync();
        }

        public async Task<Invoice> GetInvoiceByIdAsync(int invoiceId)
        {
            return await _context.Invoices
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);
        }
    }
}
