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
    public class CashOutcomesController : Controller
    {
        private readonly FinanceWContext _context;

        public CashOutcomesController(FinanceWContext context)
        {
            _context = context;
        }

        // GET: CashOutcomes
        public async Task<IActionResult> Index()
        {
            var financeWContext = _context.CashOutcome.Include(c => c.Product);
            return View(await financeWContext.ToListAsync());
        }

        // GET: CashOutcomes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashOutcome = await _context.CashOutcome
                .Include(c => c.Product)
                .SingleOrDefaultAsync(m => m.CashOutcomeId == id);
            if (cashOutcome == null)
            {
                return NotFound();
            }

            return View(cashOutcome);
        }

        // GET: CashOutcomes/Create
        public IActionResult Create()
        {
            CreateInitial(0);

            CashOutcome cashOutcome = new CashOutcome();
            cashOutcome.OutcomeDate = DateTime.Today;
            return View(cashOutcome);
        }

        // POST: CashOutcomes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CashOutcomeId,ProductId,Description,Amount,OutcomeDate")] CashOutcome cashOutcome)
        {
            if (ModelState.IsValid)
            {
                cashOutcome.Product = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == cashOutcome.ProductId);
                //product.Balance = product.Balance - cashOutcome.Amount;
                cashOutcome.Product.Balance = cashOutcome.Product.Balance - cashOutcome.Amount;

                cashOutcome.CreatedDate = DateTime.Today;
                cashOutcome.StatusOutcome = Models.Enum.StatusCashFlow.Activo;
                _context.Add(cashOutcome);
                //_context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            CreateInitial(cashOutcome.ProductId);
            return View(cashOutcome);
        }

        // GET: CashOutcomes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashOutcome = await _context.CashOutcome.SingleOrDefaultAsync(m => m.CashOutcomeId == id);
            if (cashOutcome == null)
            {
                return NotFound();
            }
            CreateInitial(cashOutcome.ProductId);
            return View(cashOutcome);
        }

        // POST: CashOutcomes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CashOutcomeId,ProductId,Description,Amount,OutcomeDate,CreatedDate,StatusOutcome")] CashOutcome cashOutcome)
        {
            if (id != cashOutcome.CashOutcomeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    CashOutcome cashOutcomeOld = await _context.CashOutcome.SingleOrDefaultAsync(c => c.CashOutcomeId == cashOutcome.CashOutcomeId);
                    Product product = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == cashOutcome.ProductId);

                    if (cashOutcome.Amount != cashOutcomeOld.Amount)
                    {
                        var _amount = cashOutcome.Amount - cashOutcomeOld.Amount;
                        product.Balance = product.Balance - _amount;
                        _context.Update(product);
                    }

                    _context.Update(cashOutcome);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CashOutcomeExists(cashOutcome.CashOutcomeId))
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
            CreateInitial(cashOutcome.ProductId);
            return View(cashOutcome);
        }

        // GET: CashOutcomes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashOutcome = await _context.CashOutcome
                .Include(c => c.Product)
                .SingleOrDefaultAsync(m => m.CashOutcomeId == id);
            if (cashOutcome == null)
            {
                return NotFound();
            }

            return View(cashOutcome);
        }

        // POST: CashOutcomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cashOutcome = await _context.CashOutcome.SingleOrDefaultAsync(m => m.CashOutcomeId == id);
            Product product = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == cashOutcome.ProductId);

            //_context.CashOutcome.Remove(cashOutcome);

            product.Balance = product.Balance + cashOutcome.Amount;
            cashOutcome.StatusOutcome = Models.Enum.StatusCashFlow.Anulado;
            _context.Update(product);
            _context.Update(cashOutcome);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CashOutcomeExists(int id)
        {
            return _context.CashOutcome.Any(e => e.CashOutcomeId == id);
        }

        private void CreateInitial(int? productId)
        {
            //ViewData["ExpenseId"] = new SelectList(_context.Expense, "ExpenseId", "Description");
            ViewData["ProductId"] = new SelectList(_context.Product.Where(d => d.ProductTypeId == Models.Enum.ProductType.CuentaAhorro ||
                d.ProductTypeId == Models.Enum.ProductType.CuentaCorriente ||
                d.ProductTypeId == Models.Enum.ProductType.Efectivo ||
                d.ProductTypeId == Models.Enum.ProductType.TarjetaCreditoAE ||
                d.ProductTypeId == Models.Enum.ProductType.TarjetaCreditoMC ||
                d.ProductTypeId == Models.Enum.ProductType.TarjetaCreditoVis)
                .Select(s => new { ProductId = s.ProductId, Description = s.Alias + " - " + s.Balance.ToString("C") }), "ProductId", "Description", productId);
        }
    }
}