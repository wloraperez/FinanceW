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
    public class PayExpensesController : Controller
    {
        private readonly FinanceWContext _context;

        public PayExpensesController(FinanceWContext context)
        {
            _context = context;
        }

        // GET: PayExpenses
        public async Task<IActionResult> Index()
        {
            var financeWContext = _context.PayExpense.Include(p => p.Expense).Include(p => p.Product);
            return View(await financeWContext.ToListAsync());
        }

        // GET: PayExpenses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payExpense = await _context.PayExpense
                .Include(p => p.Expense)
                .Include(p => p.Product)
                .SingleOrDefaultAsync(m => m.PayExpenseId == id);
            if (payExpense == null)
            {
                return NotFound();
            }

            return View(payExpense);
        }

        // GET: PayExpenses/Create
        public IActionResult Create()
        {
            CreateInitial(0, 0);

            PayExpense payExpense = new PayExpense();
            payExpense.PayExpenseDate = DateTime.Today;
            return View(payExpense);
        }

        // POST: PayExpenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PayExpenseId,ProductId,ExpenseId,Description,Amount,Tax,PayExpenseDate")] PayExpense payExpense)
        {
            if (ModelState.IsValid)
            {
                payExpense.Product = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == payExpense.ProductId);

                payExpense.Expense = await _context.Expense.SingleOrDefaultAsync(e => e.ExpenseId == payExpense.ExpenseId);

                if (payExpense.Product.Balance < (payExpense.Amount + payExpense.Tax))
                {
                    CreateInitial(payExpense.ProductId, payExpense.ExpenseId);
                    ModelState.AddModelError("", "Balance de Producto origen insuficiente.");
                    return View(payExpense);
                }

                FunctionsConvert functionsConvert = new FunctionsConvert(_context);
                var _payExpense = functionsConvert.ConvertCurrency(payExpense, 0, 0);

                //restar el balance del producto origen
                payExpense.Product.Balance = payExpense.Product.Balance - _payExpense.Amount - _payExpense.Tax;

                payExpense.StatusPayExpense = Models.Enum.StatusCashFlow.Activo;
                payExpense.CreatedDate = DateTime.Today;
                _context.Add(payExpense);
                //_context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            CreateInitial(payExpense.ProductId, payExpense.ExpenseId);
            return View(payExpense);
        }

        // GET: PayExpenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payExpense = await _context.PayExpense.SingleOrDefaultAsync(m => m.PayExpenseId == id);
            if (payExpense == null)
            {
                return NotFound();
            }
            CreateInitial(payExpense.ProductId, payExpense.ExpenseId);
            return View(payExpense);
        }

        // POST: PayExpenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PayExpenseId,ProductId,ExpenseId,Description,Amount,Tax,PayExpenseDate,CreatedDate,StatusPayExpense")] PayExpense payExpense)
        {
            if (id != payExpense.PayExpenseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    PayExpense payExpenseOld = await _context.PayExpense.SingleOrDefaultAsync(c => c.PayExpenseId == payExpense.PayExpenseId);

                    if (payExpense.Amount != payExpenseOld.Amount)
                    {
                        FunctionsConvert functionsConvert = new FunctionsConvert(_context);

                        var _amount = payExpense.Amount - payExpenseOld.Amount;
                        var _tax = payExpense.Tax - payExpenseOld.Tax;

                        payExpense.Product = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == payExpense.ProductId);

                        payExpense.Expense = await _context.Expense.SingleOrDefaultAsync(e => e.ExpenseId == payExpense.ExpenseId);

                        var _payExpense = functionsConvert.ConvertCurrency(payExpense, _amount, _tax);

                        if (payExpense.Product.Balance < (_payExpense.Amount + _payExpense.Tax))
                        {
                            CreateInitial(payExpense.ProductId, payExpense.ExpenseId);
                            ModelState.AddModelError("", "Balance de Producto origen insuficiente.");
                            return View(payExpense);
                        }

                        payExpense.Product.Balance = payExpense.Product.Balance - _payExpense.Amount -  _payExpense.Tax;
                        //_context.Update(product);
                    }

                    _context.Entry(payExpenseOld).State = EntityState.Detached; //detach old to update the new
                    _context.Update(payExpense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayExpenseExists(payExpense.PayExpenseId))
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
            CreateInitial(payExpense.ProductId, payExpense.ExpenseId);
            return View(payExpense);
        }

        // GET: PayExpenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payExpense = await _context.PayExpense
                .Include(p => p.Expense)
                .Include(p => p.Product)
                .SingleOrDefaultAsync(m => m.PayExpenseId == id);
            if (payExpense == null)
            {
                return NotFound();
            }

            return View(payExpense);
        }

        // POST: PayExpenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payExpense = await _context.PayExpense.SingleOrDefaultAsync(m => m.PayExpenseId == id);
            payExpense.StatusPayExpense = Models.Enum.StatusCashFlow.Anulado;
            Product product = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == payExpense.ProductId);
            product.Balance = product.Balance + payExpense.Amount + payExpense.Tax;
            //_context.PayExpense.Remove(payExpense);
            _context.Update(payExpense);
            _context.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PayExpenseExists(int id)
        {
            return _context.PayExpense.Any(e => e.PayExpenseId == id);
        }

        private void CreateInitial(int? productId, int? expenseId)
        {
            ViewData["ProductId"] = new SelectList(_context.Product.Where(d => d.ProductTypeId == Models.Enum.ProductType.CuentaAhorro ||
                d.ProductTypeId == Models.Enum.ProductType.CuentaCorriente ||
                d.ProductTypeId == Models.Enum.ProductType.Efectivo ||
                d.ProductTypeId == Models.Enum.ProductType.TarjetaCreditoAE ||
                d.ProductTypeId == Models.Enum.ProductType.TarjetaCreditoMC ||
                d.ProductTypeId == Models.Enum.ProductType.TarjetaCreditoVis)
                .Select(s => new { ProductId = s.ProductId, Description = s.Alias + " - " + s.Balance.ToString("C") }), "ProductId", "Description", productId);

            ViewData["ExpenseId"] = new SelectList(_context.Expense.Select(s =>
                new { ExpenseId = s.ExpenseId, Description = s.Description + " - " + s.Amount.ToString("C") }), "ExpenseId", "Description", expenseId);
        }
    }
}
