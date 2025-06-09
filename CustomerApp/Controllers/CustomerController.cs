using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerApp.Data;
using CustomerApp.Domain.Models;
using CustomerApp.Domain.Services;

namespace CustomerApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IInvoiceService _invoiceService;
        private readonly AppDbContext _context;

        public CustomerController(ICustomerService customerService, IInvoiceService invoiceService, AppDbContext context)
        {
            _customerService = customerService;
            _invoiceService = invoiceService;
            _context = context;
        }

        public async Task<IActionResult> Index(string group = "A-E")
        {
            var (start, end) = ParseGroupRange(group);
            var customers = await _customerService.GetCustomersInGroupAsync(start, end);

            ViewBag.CurrentGroup = group;
            ViewBag.LastDeletedId = TempData["LastDeletedId"];
            return View(customers);
        }

        public IActionResult Add() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _customerService.AddCustomerAsync(customer);
                char firstLetter = char.ToUpper(customer.Name[0]);
                string group = firstLetter switch
                {
                    >= 'A' and <= 'E' => "A-E",
                    >= 'F' and <= 'J' => "F-J",
                    >= 'K' and <= 'O' => "K-O",
                    >= 'P' and <= 'T' => "P-T",
                    _ => "U-Z"
                };

                return RedirectToAction(nameof(Index), new { group });
            }
            return View(customer);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _customerService.UpdateCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> AddInvoice(int customerId, DateTime date, int paymentTermsId)
        {
            await _invoiceService.AddInvoiceAsync(customerId, date, paymentTermsId);
            return RedirectToAction("Details", new { id = customerId });
        }

        [HttpPost]
        public async Task<IActionResult> AddLineItem(int invoiceId, string description, decimal amount)
        {
            await _invoiceService.AddLineItemAsync(invoiceId, description, amount);
            var invoice = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
            return RedirectToAction("Details", new { id = invoice.CustomerId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _customerService.SoftDeleteCustomerAsync(id);
            TempData["LastDeletedId"] = id;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Undo(int id)
        {
            await _customerService.UndoDeleteCustomerAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerService.GetCustomerWithInvoicesAsync(id);
            if (customer == null) return NotFound();
            ViewBag.PaymentTerms = await _context.PaymentTerms.ToListAsync();
            return View(customer);
        }

        private (char start, char end) ParseGroupRange(string group)
        {
            if (string.IsNullOrEmpty(group) || group.Length != 3 || group[1] != '-')
                return ('A', 'E'); // Default fallback
            return (group[0], group[2]);
        }
    }
}