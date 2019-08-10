using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinanceW.Models;

namespace FinanceW.Controllers
{
    public class CurrencyConvertsController : Controller
    {
        private readonly FinanceWContext _context;

        public CurrencyConvertsController(FinanceWContext context)
        {
            _context = context;
        }

        // GET: CurrencyConverts
        public async Task<IActionResult> Index()
        {
            var financeWContext = _context.CurrencyConvert.Include(c => c.CurrencyFrom).Include(c => c.CurrencyTo);
            return View(await financeWContext.ToListAsync());
        }

        // GET: CurrencyConverts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currencyConvert = await _context.CurrencyConvert
                .Include(c => c.CurrencyFrom)
                .Include(c => c.CurrencyTo)
                .SingleOrDefaultAsync(m => m.CurrencyConvertId == id);
            if (currencyConvert == null)
            {
                return NotFound();
            }

            return View(currencyConvert);
        }

        // GET: CurrencyConverts/Create
        public IActionResult Create()
        {
            ViewData["CurrencyFromCurrencyId"] = new SelectList(_context.Currency, "CurrencyId", "Description");
            ViewData["CurrencyToCurrencyId"] = new SelectList(_context.Currency, "CurrencyId", "Description");
            return View();
        }

        // POST: CurrencyConverts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CurrencyConvertId,Multiple,CurrencyFromCurrencyId,CurrencyToCurrencyId,DateValidFrom,DateValidTo,StatusCurrency")] CurrencyConvert currencyConvert)
        {
            if (ModelState.IsValid)
            {
                _context.Add(currencyConvert);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CurrencyFromCurrencyId"] = new SelectList(_context.Currency, "CurrencyId", "Description", currencyConvert.CurrencyFromCurrencyId);
            ViewData["CurrencyToCurrencyId"] = new SelectList(_context.Currency, "CurrencyId", "Description", currencyConvert.CurrencyToCurrencyId);
            return View(currencyConvert);
        }

        // GET: CurrencyConverts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currencyConvert = await _context.CurrencyConvert.SingleOrDefaultAsync(m => m.CurrencyConvertId == id);
            if (currencyConvert == null)
            {
                return NotFound();
            }
            ViewData["CurrencyFromCurrencyId"] = new SelectList(_context.Currency, "CurrencyId", "Description", currencyConvert.CurrencyFromCurrencyId);
            ViewData["CurrencyToCurrencyId"] = new SelectList(_context.Currency, "CurrencyId", "Description", currencyConvert.CurrencyToCurrencyId);
            return View(currencyConvert);
        }

        // POST: CurrencyConverts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CurrencyConvertId,Multiple,CurrencyFromCurrencyId,CurrencyToCurrencyId,DateValidFrom,DateValidTo,StatusCurrency")] CurrencyConvert currencyConvert)
        {
            if (id != currencyConvert.CurrencyConvertId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(currencyConvert);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CurrencyConvertExists(currencyConvert.CurrencyConvertId))
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
            ViewData["CurrencyFromCurrencyId"] = new SelectList(_context.Currency, "CurrencyId", "Description", currencyConvert.CurrencyFromCurrencyId);
            ViewData["CurrencyToCurrencyId"] = new SelectList(_context.Currency, "CurrencyId", "Description", currencyConvert.CurrencyToCurrencyId);
            return View(currencyConvert);
        }

        // GET: CurrencyConverts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currencyConvert = await _context.CurrencyConvert
                .Include(c => c.CurrencyFrom)
                .Include(c => c.CurrencyTo)
                .SingleOrDefaultAsync(m => m.CurrencyConvertId == id);
            if (currencyConvert == null)
            {
                return NotFound();
            }

            return View(currencyConvert);
        }

        // POST: CurrencyConverts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currencyConvert = await _context.CurrencyConvert.SingleOrDefaultAsync(m => m.CurrencyConvertId == id);
            _context.CurrencyConvert.Remove(currencyConvert);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurrencyConvertExists(int id)
        {
            return _context.CurrencyConvert.Any(e => e.CurrencyConvertId == id);
        }
    }
}
