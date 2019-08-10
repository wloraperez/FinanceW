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
    public class ReminderTypesController : Controller
    {
        private readonly FinanceWContext _context;

        public ReminderTypesController(FinanceWContext context)
        {
            _context = context;
        }

        // GET: ReminderTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ReminderType.ToListAsync());
        }

        // GET: ReminderTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reminderType = await _context.ReminderType
                .SingleOrDefaultAsync(m => m.ReminderTypeId == id);
            if (reminderType == null)
            {
                return NotFound();
            }

            return View(reminderType);
        }

        // GET: ReminderTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ReminderTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReminderTypeId,Description,RecurrenceHour,RecurrenceDay,RecurrenceMonth,RecurrenceYear")] ReminderType reminderType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reminderType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reminderType);
        }

        // GET: ReminderTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reminderType = await _context.ReminderType.SingleOrDefaultAsync(m => m.ReminderTypeId == id);
            if (reminderType == null)
            {
                return NotFound();
            }
            return View(reminderType);
        }

        // POST: ReminderTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReminderTypeId,Description,RecurrenceHour,RecurrenceDay,RecurrenceMonth,RecurrenceYear")] ReminderType reminderType)
        {
            if (id != reminderType.ReminderTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reminderType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReminderTypeExists(reminderType.ReminderTypeId))
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
            return View(reminderType);
        }

        // GET: ReminderTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reminderType = await _context.ReminderType
                .SingleOrDefaultAsync(m => m.ReminderTypeId == id);
            if (reminderType == null)
            {
                return NotFound();
            }

            return View(reminderType);
        }

        // POST: ReminderTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reminderType = await _context.ReminderType.SingleOrDefaultAsync(m => m.ReminderTypeId == id);
            _context.ReminderType.Remove(reminderType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReminderTypeExists(int id)
        {
            return _context.ReminderType.Any(e => e.ReminderTypeId == id);
        }
    }
}
