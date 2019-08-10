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
    public class PayCreditCardsController : Controller
    {
        private readonly FinanceWContext _context;

        public PayCreditCardsController(FinanceWContext context)
        {
            _context = context;
        }

        // GET: PayProducts
        public async Task<IActionResult> Index()
        {
            var financeWContext = _context.PayProduct.Include(p => p.CreditCardCut).Include(p => p.ProductFrom).Include(p => p.ProductTo).Where(p =>
                  p.ProductTo.ProductTypeId == Models.Enum.ProductType.TarjetaCreditoAE ||
                  p.ProductTo.ProductTypeId == Models.Enum.ProductType.TarjetaCreditoMC ||
                  p.ProductTo.ProductTypeId == Models.Enum.ProductType.TarjetaCreditoVis);

            return View(await financeWContext.ToListAsync());
        }

        // GET: PayProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payProduct = await _context.PayProduct
                .Include(p => p.CreditCardCut)
                .Include(p => p.ProductFrom)
                .Include(p => p.ProductTo)
                .SingleOrDefaultAsync(m => m.PayProductId == id);
            if (payProduct == null)
            {
                return NotFound();
            }

            return View(payProduct);
        }

        // GET: PayProducts/Create
        public IActionResult Create()
        {
            CreateInitial(0, 0, 0);
            PayProduct payProduct = new PayProduct();
            payProduct.PayProductDate = DateTime.Today;
            return View(payProduct);
        }

        // POST: PayProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PayProductId,ProductIdFrom,ProductIdTo,CreditCardCutId,Description,Amount,Tax,PayProductDate")] PayProduct payProduct)
        {
            if (ModelState.IsValid)
            {
                payProduct.ProductFrom = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == payProduct.ProductIdFrom);
                payProduct.ProductTo = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == payProduct.ProductIdTo);
                payProduct.CreditCardCut = await _context.CreditCardCut.SingleOrDefaultAsync(p => p.CreditCardCutId == payProduct.CreditCardCutId);

                if (payProduct.ProductFrom.Balance < (payProduct.Amount + payProduct.Tax))
                {
                    CreateInitial(payProduct.ProductIdFrom, payProduct.ProductIdTo, payProduct.CreditCardCutId);
                    ModelState.AddModelError("", "Balance de Producto origen insuficiente.");
                    return View(payProduct);
                }
                FunctionsConvert functionsConvert = new FunctionsConvert(_context);
                var _payProduct = functionsConvert.ConvertCurrency(payProduct, 0, 0);

                //restar el balance del producto origen
                payProduct.ProductFrom.Balance = payProduct.ProductFrom.Balance - payProduct.Amount - payProduct.Tax;

                //sumar al balance a la tc destino
                payProduct.ProductTo.Balance = payProduct.ProductTo.Balance + payProduct.Amount;

                //actualizar corte de tarjeta
                payProduct.CreditCardCut.AmountPayment = payProduct.CreditCardCut.AmountPayment + payProduct.Amount;
                payProduct.CreditCardCut.AmountPending = payProduct.CreditCardCut.AmountPending - payProduct.Amount;

                payProduct.CreatedDate = DateTime.Today;
                payProduct.StatusPayProduct = Models.Enum.StatusPayment.Activo;
                _context.Add(payProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            CreateInitial(payProduct.ProductIdFrom, payProduct.ProductIdTo, payProduct.CreditCardCutId);
            return View(payProduct);
        }

        // GET: PayProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payProduct = await _context.PayProduct.SingleOrDefaultAsync(m => m.PayProductId == id);
            if (payProduct == null)
            {
                return NotFound();
            }
            CreateInitial(payProduct.ProductIdFrom, payProduct.ProductIdTo, payProduct.CreditCardCutId);
            return View(payProduct);
        }

        // POST: PayProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PayProductId,ProductIdFrom,ProductIdTo,CreditCardCutId,Description,Amount,Tax,PayProductDate,CreatedDate,StatusPayProduct")] PayProduct payProduct)
        {
            if (id != payProduct.PayProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    PayProduct payProductOld = await _context.PayProduct.SingleOrDefaultAsync(p => p.PayProductId == payProduct.PayProductId);

                    if (payProduct.Amount != payProductOld.Amount)
                    {
                        FunctionsConvert functionsConvert = new FunctionsConvert(_context);

                        var _amount = payProduct.Amount - payProductOld.Amount;
                        var _tax = payProduct.Tax - payProductOld.Tax;

                        //enviar monto y tax para convertir moneda
                        var _payProduct = functionsConvert.ConvertCurrency(payProduct, _amount, _tax);

                        payProduct.ProductFrom = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == payProduct.ProductIdFrom);
                        payProduct.ProductTo = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == payProduct.ProductIdTo);
                        payProduct.CreditCardCut = await _context.CreditCardCut.SingleOrDefaultAsync(p => p.CreditCardCutId == payProduct.CreditCardCutId);

                        if (payProduct.ProductFrom.Balance < (_payProduct.Amount + _payProduct.Tax))
                        {
                            CreateInitial(payProduct.ProductIdFrom, payProduct.ProductIdTo, payProduct.CreditCardCutId);
                            ModelState.AddModelError("", "Balance de Producto origen insuficiente.");
                            return View(payProduct);
                        }

                        //restar el balance del producto origen
                        payProduct.ProductFrom.Balance = payProduct.ProductFrom.Balance - _payProduct.Amount - _payProduct.Tax;

                        //sumar al balance a la tc destino
                        payProduct.ProductTo.Balance = payProduct.ProductTo.Balance + _amount;

                        //actualizar corte de tarjeta
                        payProduct.CreditCardCut.AmountPayment = payProduct.CreditCardCut.AmountPayment + _amount;
                        payProduct.CreditCardCut.AmountPending = payProduct.CreditCardCut.AmountPending - _amount;
                    }
                    _context.Entry(payProductOld).State = EntityState.Detached; //detach old to update the new
                    _context.Update(payProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayProductExists(payProduct.PayProductId))
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
            CreateInitial(payProduct.ProductIdFrom, payProduct.ProductIdTo, payProduct.CreditCardCutId);
            return View(payProduct);
        }

        // GET: PayProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payProduct = await _context.PayProduct
                .Include(p => p.CreditCardCut)
                .Include(p => p.ProductFrom)
                .Include(p => p.ProductTo)
                .SingleOrDefaultAsync(m => m.PayProductId == id);
            if (payProduct == null)
            {
                return NotFound();
            }

            return View(payProduct);
        }

        // POST: PayProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payProduct = await _context.PayProduct.SingleOrDefaultAsync(m => m.PayProductId == id);

            payProduct.ProductFrom = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == payProduct.ProductIdFrom);
            payProduct.ProductTo = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == payProduct.ProductIdTo);
            payProduct.CreditCardCut = await _context.CreditCardCut.SingleOrDefaultAsync(p => p.CreditCardCutId == payProduct.CreditCardCutId);
            //sumar el balance del producto origen
            payProduct.ProductFrom.Balance = payProduct.ProductFrom.Balance + payProduct.Amount + payProduct.Tax;

            //restar al balance a la tc destino
            payProduct.ProductTo.Balance = payProduct.ProductTo.Balance - payProduct.Amount;

            //actualizar corte de tarjeta
            payProduct.CreditCardCut.AmountPayment = payProduct.CreditCardCut.AmountPayment - payProduct.Amount;
            payProduct.CreditCardCut.AmountPending = payProduct.CreditCardCut.AmountPending + payProduct.Amount;

            payProduct.StatusPayProduct = Models.Enum.StatusPayment.Inactivo;

            _context.Update(payProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PayProductExists(int id)
        {
            return _context.PayProduct.Any(e => e.PayProductId == id);
        }

        private void CreateInitial(int? productIdFrom, int? productIdTo, int? creditCardCutId)
        {
            ViewData["ProductIdFrom"] = new SelectList(_context.Product.Where(p => p.ProductTypeId == Models.Enum.ProductType.CuentaAhorro ||
            p.ProductTypeId == Models.Enum.ProductType.CuentaCorriente ||
            p.ProductTypeId == Models.Enum.ProductType.Efectivo)
                .Select(s => new { ProductId = s.ProductId, Description = s.Alias + " - " + s.Balance.ToString("C") }), "ProductId", "Description", productIdFrom);

            ViewData["ProductIdTo"] = new SelectList(_context.Product.Where(p => p.ProductTypeId == Models.Enum.ProductType.TarjetaCreditoAE ||
            p.ProductTypeId == Models.Enum.ProductType.TarjetaCreditoMC ||
            p.ProductTypeId == Models.Enum.ProductType.TarjetaCreditoVis)
                .Select(s => new { ProductId = s.ProductId, Description = s.Alias + " - " + s.Balance.ToString("C") }), "ProductId", "Description", productIdFrom);

            //ViewData["CreditCardCutId"] = new SelectList(_context.CreditCardCut.Select(c => 
            //new { CreditCardCutId = c.CreditCardCutId, Description = c.PayDayCut + " - " + c.PayDayLimit }), "CreditCardCutId", "Descripcion", creditCardCutId);
            ViewData["CreditCardCutId"] = new SelectList(_context.CreditCardCut.Select(c =>
            new { CreditCardCutId = c.CreditCardCutId, Description = c.PayDayCut.ToShortDateString() + " - " + c.PayDayLimit.ToShortDateString() + " - " + c.AmountPending.ToString("C") }), "CreditCardCutId", "Description", creditCardCutId);
        }
    }
}
