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
    public class CreditCardCutsController : Controller
    {
        private readonly FinanceWContext _context;

        public CreditCardCutsController(FinanceWContext context)
        {
            _context = context;
        }

        // GET: CreditCardCuts
        public async Task<IActionResult> Index()
        {
            var financeWContext = _context.CreditCardCut.Include(p => p.Product).Where(p => p.Product.ProductTypeId == Models.Enum.ProductType.TarjetaCreditoAE ||
                  p.Product.ProductTypeId == Models.Enum.ProductType.TarjetaCreditoMC ||
                  p.Product.ProductTypeId == Models.Enum.ProductType.TarjetaCreditoVis);

            return View(await financeWContext.ToListAsync());
        }

        // GET: CreditCardCuts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditCardCut = await _context.CreditCardCut
                .Include(c => c.Product)
                .SingleOrDefaultAsync(m => m.CreditCardCutId == id);
            if (creditCardCut == null)
            {
                return NotFound();
            }

            return View(creditCardCut);
        }

        // GET: CreditCardCuts/Create
        public IActionResult Create()
        {
            CreateInitial(0);

            CreditCardCut creditCardCut = new CreditCardCut();
            creditCardCut.PayDayCut = DateTime.Today;
            creditCardCut.PayDayLimit = DateTime.Today.AddDays(20);
            return View(creditCardCut);
        }

        
        // POST: CreditCardCuts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CreditCardCutId,ProductId,PayDayCut,PayDayLimit,AmountCut")] CreditCardCut creditCardCut)
        {
            if (ModelState.IsValid)
            {
                creditCardCut.AmountPayment = 0;
                creditCardCut.AmountPending = creditCardCut.AmountCut;

                creditCardCut.CreatedDate = DateTime.Now;

                _context.Add(creditCardCut);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Alias", creditCardCut.ProductId);
            return View(creditCardCut);
        }

        // GET: CreditCardCuts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditCardCut = await _context.CreditCardCut.SingleOrDefaultAsync(m => m.CreditCardCutId == id);
            if (creditCardCut == null)
            {
                return NotFound();
            }
            CreateInitial(creditCardCut.ProductId);
            return View(creditCardCut);
        }

        // POST: CreditCardCuts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CreditCardCutId,ProductId,PayDayCut,PayDayLimit,AmountCut,AmountPayment,AmountPending,CreatedDate")] CreditCardCut creditCardCut)
        {
            if (id != creditCardCut.CreditCardCutId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(creditCardCut);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CreditCardCutExists(creditCardCut.CreditCardCutId))
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
            CreateInitial(creditCardCut.ProductId);
            return View(creditCardCut);
        }

        // GET: CreditCardCuts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditCardCut = await _context.CreditCardCut
                .Include(c => c.Product)
                .SingleOrDefaultAsync(m => m.CreditCardCutId == id);
            if (creditCardCut == null)
            {
                return NotFound();
            }

            return View(creditCardCut);
        }

        // POST: CreditCardCuts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var creditCardCut = await _context.CreditCardCut.SingleOrDefaultAsync(m => m.CreditCardCutId == id);
            _context.CreditCardCut.Remove(creditCardCut);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CreditCardCutExists(int id)
        {
            return _context.CreditCardCut.Any(e => e.CreditCardCutId == id);
        }

        private void CreateInitial(int productId)
        {
            ViewData["ProductId"] = new SelectList(_context.Product.Where(d => d.ProductTypeId == Models.Enum.ProductType.TarjetaCreditoAE ||
                d.ProductTypeId == Models.Enum.ProductType.TarjetaCreditoMC ||
                d.ProductTypeId == Models.Enum.ProductType.TarjetaCreditoVis)
                .Select(s => new { ProductId = s.ProductId, Description = s.Alias + " - " + s.Balance.ToString("C") }), "ProductId", "Description", productId);
        }
    }
}
