using CustomerApp.Domain.Models;

namespace CustomerApp.Data
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Customers.Any())
                return;

            var net30 = new PaymentTerms { Label = "Net 30" };
            var net60 = new PaymentTerms { Label = "Net 60" };
            var dueNow = new PaymentTerms { Label = "Due on Receipt" };

            context.PaymentTerms.AddRange(net30, net60, dueNow);
            context.SaveChanges();

            var customer1 = new Customer
            {
                Name = "Archer Hill Company",
                Address1 = "PO Box 86373",
                Address2 = "N/A",
                City = "Chicago",
                State = "IL",
                PostalCode = "60701",
                Phone = "312-555-1312",
                ContactEmail = "contact@archerhill.com"
            };

            var customer2 = new Customer
            {
                Name = "Baskin Gas & Electric",
                Address1 = "Box 52901",
                Address2 = "N/A",
                City = "San Francisco",
                State = "CA",
                PostalCode = "94112",
                Phone = "415-444-2020",
                ContactEmail = "info@baskingas.com"
            };

            var customer3 = new Customer
            {
                Name = "Ghostly Publishers Weekly",
                Address1 = "129 Marion Ave",
                Address2 = "N/A",
                City = "Marion",
                State = "OH",
                PostalCode = "43402",
                Phone = "740-444-7890",
                ContactEmail = "editor@ghostlypw.com"
            };

            var invoice1 = new Invoice
            {
                Date = DateTime.Today.AddDays(-10), // April 4
                PaymentTerms = net30,
                PaymentTermsId = net30.Id,
                AmountPaid = 0,
                Customer = customer1,
                CustomerId = 0,
                LineItems = new List<LineItem>
                {
                    new LineItem { Description = "Network Consultation", Amount = 1200, Invoice = null! },
                    new LineItem { Description = "Firewall Setup", Amount = 450, Invoice = null! }
                }
            };

            var invoice2 = new Invoice
            {
                Date = DateTime.Today.AddDays(-5), // April 9
                PaymentTerms = net60,
                PaymentTermsId = net60.Id,
                AmountPaid = 200,
                Customer = customer2,
                CustomerId = 0,
                LineItems = new List<LineItem>
                {
                    new LineItem { Description = "Cloud Infrastructure Audit", Amount = 800, Invoice = null! }
                }
            };

            var invoice3 = new Invoice
            {
                Date = DateTime.Today.AddDays(-2), // April 12
                PaymentTerms = dueNow,
                PaymentTermsId = dueNow.Id,
                AmountPaid = 0,
                Customer = customer3,
                CustomerId = 0,
                LineItems = new List<LineItem>
                {
                    new LineItem { Description = "Monthly Hosting", Amount = 300, Invoice = null! },
                    new LineItem { Description = "Email Setup", Amount = 150, Invoice = null! }
                }
            };

            customer1.Invoices = new List<Invoice> { invoice1 };
            customer2.Invoices = new List<Invoice> { invoice2 };
            customer3.Invoices = new List<Invoice> { invoice3 };

            context.Customers.AddRange(customer1, customer2, customer3);
            context.SaveChanges();
        }
    }
}
