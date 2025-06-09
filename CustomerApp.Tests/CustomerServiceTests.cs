using System.Threading.Tasks;
using Xunit;
using CustomerApp.Domain.Models;
using CustomerApp.Services;
using CustomerApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CustomerApp.Tests
{
    public class CustomerServiceTests
    {
        private async Task<AppDbContext> GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"CustomerDb_{System.Guid.NewGuid()}")
                .Options;

            var context = new AppDbContext(options);

            await context.Customers.AddAsync(new Customer
            {
                Name = "Test User",
                Address1 = "123 Test St",
                City = "Testville",
                State = "TS",
                PostalCode = "12345",
                Phone = "123-456-7890",
                ContactEmail = "test@example.com",
                IsDeleted = false
            });

            await context.SaveChangesAsync();
            return context;
        }

        [Fact]
        public async Task AddCustomerAsync_ShouldAddCustomer()
        {
            var context = await GetDbContextAsync();
            var service = new CustomerService(context);

            var newCustomer = new Customer
            {
                Name = "Jane Doe",
                Address1 = "456 Main St",
                City = "Townsville",
                State = "ON",
                PostalCode = "98765",
                Phone = "987-654-3210",
                ContactEmail = "jane@example.com"
            };

            await service.AddCustomerAsync(newCustomer);

            Assert.Equal(2, context.Customers.Count());
            Assert.Contains(context.Customers, c => c.Name == "Jane Doe");
        }

        [Fact]
        public async Task SoftDeleteCustomerAsync_ShouldMarkCustomerAsDeleted()
        {
            var context = await GetDbContextAsync();
            var service = new CustomerService(context);
            var customer = context.Customers.First();

            await service.SoftDeleteCustomerAsync(customer.Id);
            var updated = await context.Customers.FindAsync(customer.Id);

            Assert.NotNull(updated);
            Assert.True(updated!.IsDeleted);
        }

        [Fact]
        public async Task UndoDeleteCustomerAsync_ShouldRestoreCustomer()
        {
            var context = await GetDbContextAsync();
            var customer = context.Customers.First();
            customer.IsDeleted = true;
            await context.SaveChangesAsync();

            var service = new CustomerService(context);
            await service.UndoDeleteCustomerAsync(customer.Id);

            var updated = await context.Customers.FindAsync(customer.Id);
            Assert.NotNull(updated);
            Assert.False(updated!.IsDeleted);
        }
    }
}
