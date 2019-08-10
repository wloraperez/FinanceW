using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinanceW.Models;

namespace FinanceW.Controllers
{
    public class PaymentRemindersController : Controller
    {
        private readonly FinanceWContext _context;

        public PaymentRemindersController(FinanceWContext context)
        {
            _context = context;
        }

        // GET: PaymentReminders
        public async Task<IActionResult> Index()
        {
            var financeWContext = _context.PaymentReminder.Include(p => p.Expense).Include(p => p.Product).Include(p => p.ReminderType);
            return View(await financeWContext.ToListAsync());
        }

        // GET: PaymentReminders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentReminder = await _context.PaymentReminder
                .Include(p => p.Expense)
                .Include(p => p.Product)
                .Include(p => p.ReminderType)
                .SingleOrDefaultAsync(m => m.PaymentReminderId == id);
            if (paymentReminder == null)
            {
                return NotFound();
            }

            return View(paymentReminder);
        }

        // GET: PaymentReminders/Create
        public IActionResult Create()
        {
            PaymentReminder paymentReminder = new PaymentReminder();
            paymentReminder.StartDate = DateTime.Today;
            //paymentReminder.StartTime = 
            paymentReminder.EndDate = DateTime.Today.AddDays(1);
            
            ViewData["ExpenseId"] = new SelectList(_context.Expense, "ExpenseId", "Description");
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Alias");
            ViewData["ReminderTypeId"] = new SelectList(_context.ReminderType, "ReminderTypeId", "Description");
            return View(paymentReminder);
        }

        // POST: PaymentReminders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentReminderId,ReminderTypeId,ProductId,ExpenseId,StartDate,StartTime,EndDate,EndTime,StatusPaymentReminder")] PaymentReminder paymentReminder)
        {
            if (ModelState.IsValid)
            {
                paymentReminder.CreatedDate = DateTime.Today;
                paymentReminder.StatusPaymentReminder = Models.Enum.StatusPaymentReminder.Activo;
                _context.Add(paymentReminder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExpenseId"] = new SelectList(_context.Expense, "ExpenseId", "Description", paymentReminder.ExpenseId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Alias", paymentReminder.ProductId);
            ViewData["ReminderTypeId"] = new SelectList(_context.ReminderType, "ReminderTypeId", "Description", paymentReminder.ReminderTypeId);
            return View(paymentReminder);
        }

        // GET: PaymentReminders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentReminder = await _context.PaymentReminder.SingleOrDefaultAsync(m => m.PaymentReminderId == id);
            if (paymentReminder == null)
            {
                return NotFound();
            }
            ViewData["ExpenseId"] = new SelectList(_context.Expense, "ExpenseId", "Description", paymentReminder.ExpenseId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Alias", paymentReminder.ProductId);
            ViewData["ReminderTypeId"] = new SelectList(_context.ReminderType, "ReminderTypeId", "Description", paymentReminder.ReminderTypeId);
            return View(paymentReminder);
        }

        // POST: PaymentReminders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentReminderId,ReminderTypeId,ProductId,ExpenseId,StartDate,StartTime,EndDate,EndTime,CreatedDate,StatusPaymentReminder")] PaymentReminder paymentReminder)
        {
            if (id != paymentReminder.PaymentReminderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paymentReminder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentReminderExists(paymentReminder.PaymentReminderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExpenseId"] = new SelectList(_context.Expense, "ExpenseId", "Description", paymentReminder.ExpenseId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Alias", paymentReminder.ProductId);
            ViewData["ReminderTypeId"] = new SelectList(_context.ReminderType, "ReminderTypeId", "Description", paymentReminder.ReminderTypeId);
            return View(paymentReminder);
        }

        // GET: PaymentReminders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentReminder = await _context.PaymentReminder
                .Include(p => p.Expense)
                .Include(p => p.Product)
                .Include(p => p.ReminderType)
                .SingleOrDefaultAsync(m => m.PaymentReminderId == id);
            if (paymentReminder == null)
            {
                return NotFound();
            }

            return View(paymentReminder);
        }

        // POST: PaymentReminders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paymentReminder = await _context.PaymentReminder.SingleOrDefaultAsync(m => m.PaymentReminderId == id);
            _context.PaymentReminder.Remove(paymentReminder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentReminderExists(int id)
        {
            return _context.PaymentReminder.Any(e => e.PaymentReminderId == id);
        }
    }
}
