using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinanceW.Models;
using Microsoft.AspNetCore.Identity;

namespace FinanceW.Controllers
{
    public class CashIncomesController : Controller
    {
        private readonly FinanceWContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CashIncomesController(FinanceWContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CashIncomes
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var financeWContext = _context.CashIncome.Include(c => c.Product).Where(c => c.Product.UserId == user.Id);
            return View(await financeWContext.ToListAsync());
        }

        // GET: CashIncomes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashIncome = await _context.CashIncome
                .Include(c => c.Product)
                .SingleOrDefaultAsync(m => m.CashIncomeId == id);
            if (cashIncome == null)
            {
                return NotFound();
            }

            return View(cashIncome);
        }

        // GET: CashIncomes/Create
        public IActionResult Create()
        {
            CreateInitial(0);
            CashIncome cashIncome = new CashIncome();
            cashIncome.IncomeDate = DateTime.Today;
            return View(cashIncome);
        }

        // POST: CashIncomes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CashIncomeId,ProductId,Description,Amount,IncomeDate")] CashIncome cashIncome)
        {
            if (ModelState.IsValid)
            {
                cashIncome.Product = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == cashIncome.ProductId);
                //product.Balance = product.Balance + cashIncome.Amount;
                cashIncome.Product.Balance = cashIncome.Product.Balance + cashIncome.Amount;

                cashIncome.StatusIncome = Models.Enum.StatusCashFlow.Activo;
                cashIncome.CreatedDate = DateTime.Now;
                _context.Add(cashIncome);
                //_context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            CreateInitial(cashIncome.ProductId);
            return View(cashIncome);
        }

        // GET: CashIncomes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashIncome = await _context.CashIncome.SingleOrDefaultAsync(m => m.CashIncomeId == id);
            if (cashIncome == null)
            {
                return NotFound();
            }
            CreateInitial(cashIncome.ProductId);
            return View(cashIncome);
        }

        // POST: CashIncomes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CashIncomeId,ProductId,Description,Amount,IncomeDate,CreatedDate,StatusIncome")] CashIncome cashIncome)
        {
            if (id != cashIncome.CashIncomeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cashIncomeOld = await _context.CashIncome.SingleOrDefaultAsync(c => c.CashIncomeId == cashIncome.CashIncomeId) ;
                    cashIncome.Product = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == cashIncome.ProductId);

                    if (cashIncome.Amount != cashIncomeOld.Amount)
                    {
                        var _amount = cashIncome.Amount - cashIncomeOld.Amount;
                        cashIncome.Product.Balance = cashIncome.Product.Balance + _amount;
                        //_context.Update(product);
                    }
                    _context.Entry(cashIncomeOld).State = EntityState.Detached; //detach old to update the new
                    _context.Update(cashIncome);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CashIncomeExists(cashIncome.CashIncomeId))
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
            CreateInitial(cashIncome.ProductId);
            return View(cashIncome);
        }

        // GET: CashIncomes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashIncome = await _context.CashIncome
                .Include(c => c.Product)
                .SingleOrDefaultAsync(m => m.CashIncomeId == id);
            if (cashIncome == null)
            {
                return NotFound();
            }

            return View(cashIncome);
        }

        // POST: CashIncomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cashIncome = await _context.CashIncome.SingleOrDefaultAsync(m => m.CashIncomeId == id);
            Product product = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == cashIncome.ProductId);
            product.Balance = product.Balance - cashIncome.Amount;
            cashIncome.StatusIncome = Models.Enum.StatusCashFlow.Anulado;
            //_context.CashIncome.Remove(cashIncome);
            _context.Update(product);
            _context.Update(cashIncome);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CashIncomeExists(int id)
        {
            return _context.CashIncome.Any(e => e.CashIncomeId == id);
        }

        private void CreateInitial(int? productId)
        {
            //ViewData["ExpenseId"] = new SelectList(_context.Expense, "ExpenseId", "Description");
            ViewData["ProductId"] = new SelectList(_context.Product.Where(d => d.ProductTypeId == Models.Enum.ProductType.CuentaAhorro ||
                d.ProductTypeId == Models.Enum.ProductType.CuentaCorriente ||
                d.ProductTypeId == Models.Enum.ProductType.Efectivo)
                .Select(s => new { ProductId = s.ProductId, Descripcion = s.Alias + " - " + s.Balance.ToString("C") }), "ProductId", "Descripcion", productId);
        }
    }
}
