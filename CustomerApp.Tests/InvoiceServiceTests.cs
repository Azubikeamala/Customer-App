using System.Threading.Tasks;
using Xunit;
using CustomerApp.Data;
using CustomerApp.Domain.Models;
using CustomerApp.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace CustomerApp.Tests
{
    public class InvoiceServiceTests
    {
        private async Task<AppDbContext> GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"InvoiceDb_{Guid.NewGuid()}")
                .Options;

            var context = new AppDbContext(options);

            var customer = new Customer
            {
                Name = "Invoice User",
                Address1 = "123 Billing Ave",
                City = "Billtown",
                State = "BL",
                PostalCode = "99999",
                Phone = "111-222-3333",
                ContactEmail = "invoice@user.com"
            };

            var term = new PaymentTerms { Label = "Net 30", DueDays = 30 };

            context.Customers.Add(customer);
            context.PaymentTerms.Add(term);
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task AddInvoiceAsync_ShouldAddInvoice()
        {
            var context = await GetDbContextAsync();
            var service = new InvoiceService(context);

            var customerId = context.Customers.First().Id;
            var termId = context.PaymentTerms.First().Id;

            await service.AddInvoiceAsync(customerId, DateTime.Today, termId);

            Assert.Single(context.Invoices);
            var invoice = context.Invoices.Include(i => i.Customer).FirstOrDefault();
            Assert.NotNull(invoice);
            Assert.Equal(customerId, invoice!.CustomerId);
        }

        [Fact]
        public async Task AddLineItemAsync_ShouldAddItemToInvoiceAndUpdateAmountPaid()
        {
            var context = await GetDbContextAsync();
            var service = new InvoiceService(context);

            var customer = context.Customers.First();
            var term = context.PaymentTerms.First();

            var invoice = new Invoice
            {
                CustomerId = customer.Id,
                Customer = customer,
                Date = DateTime.Today,
                PaymentTermsId = term.Id,
                PaymentTerms = term,
                AmountPaid = 0
            };

            context.Invoices.Add(invoice);
            await context.SaveChangesAsync();

            await service.AddLineItemAsync(invoice.Id, "Hosting", 200);

            var updated = await context.Invoices
                .Include(i => i.LineItems)
                .FirstAsync(i => i.Id == invoice.Id);

            Assert.Single(updated.LineItems);
            Assert.Equal(200, updated.AmountPaid);
        }

        [Fact]
        public async Task GetInvoiceByIdAsync_ShouldReturnInvoice()
        {
            var context = await GetDbContextAsync();
            var service = new InvoiceService(context);

            var customer = context.Customers.First();
            var term = context.PaymentTerms.First();

            var invoice = new Invoice
            {
                CustomerId = customer.Id,
                Customer = customer,
                Date = DateTime.Today,
                PaymentTermsId = term.Id,
                PaymentTerms = term,
                AmountPaid = 0
            };

            context.Invoices.Add(invoice);
            await context.SaveChangesAsync();

            var result = await service.GetInvoiceByIdAsync(invoice.Id);

            Assert.NotNull(result);
            Assert.Equal(invoice.Id, result.Id);
            Assert.Equal(customer.Id, result.CustomerId);
        }
    }
}
