using FinanceW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceW.Controllers
{
    public class FunctionsConvert : Controller
    {
        private readonly FinanceWContext _context;

        public FunctionsConvert(FinanceWContext context)
        {
            _context = context;
        }

        public PayProduct ConvertCurrency(PayProduct payProduct, decimal _amount, decimal _tax)
        {
            PayProduct _payProduct = new PayProduct();
            if (_amount > 0)
            {
                _payProduct.Amount = _amount;
                _payProduct.Tax = _tax;
            }
            else
            {
                _payProduct.Amount = payProduct.Amount;
                _payProduct.Tax = payProduct.Tax;
            }


            var productFrom = _context.Product.SingleOrDefaultAsync(p => p.ProductId == payProduct.ProductIdFrom);
            var productTo = _context.Product.SingleOrDefaultAsync(p => p.ProductId == payProduct.ProductIdTo);

            if (productFrom.Result.currency.CurrencyId != productTo.Result.currency.CurrencyId)
            {
                var currencyConvert = _context.CurrencyConvert.SingleOrDefaultAsync(p => p.CurrencyFromCurrencyId == productTo.Result.CurrencyId
                    && p.CurrencyToCurrencyId == productFrom.Result.CurrencyId && p.StatusCurrency == Models.Enum.StatusCurrency.Activa &&
                    (p.DateValidFrom <= payProduct.PayProductDate && p.DateValidTo >= payProduct.PayProductDate));

                if (currencyConvert != null)
                {
                    if (_amount > 0)
                    {
                        _payProduct.Amount = _amount * currencyConvert.Result.Multiple;
                        _payProduct.Tax = _tax * currencyConvert.Result.Multiple;
                    }
                    else
                    {
                        _payProduct.Amount = payProduct.Amount * currencyConvert.Result.Multiple;
                        _payProduct.Tax = payProduct.Tax * currencyConvert.Result.Multiple;
                    }
                }
            }

            return _payProduct;
        }

        public PayExpense ConvertCurrency(PayExpense payExpense, decimal _amount, decimal _tax)
        {
            PayExpense _payExpense = new PayExpense();
            if (_amount > 0)
            {
                _payExpense.Amount = _amount;
                _payExpense.Tax = _tax;
            }
            else
            {
                _payExpense.Amount = payExpense.Amount;
                _payExpense.Tax = payExpense.Tax;
            }
            
            var productFrom = _context.Product.SingleOrDefaultAsync(p => p.ProductId == payExpense.ProductId);
            var expenseTo = _context.Expense.SingleOrDefaultAsync(p => p.ExpenseId == payExpense.ExpenseId);

            if (productFrom.Result.currency.CurrencyId  != expenseTo.Result.currency.CurrencyId)
            {
                var currencyConvert = _context.CurrencyConvert.SingleOrDefaultAsync(p => p.CurrencyFromCurrencyId == expenseTo.Result.CurrencyId
                    && p.CurrencyToCurrencyId == productFrom.Result.CurrencyId && p.StatusCurrency == Models.Enum.StatusCurrency.Activa &&
                    (p.DateValidFrom <= payExpense.PayExpenseDate && p.DateValidTo >= payExpense.PayExpenseDate));

                if (currencyConvert != null)
                {
                    if (_amount > 0)
                    {
                        _payExpense.Amount = _amount * currencyConvert.Result.Multiple;
                        _payExpense.Tax = _tax * currencyConvert.Result.Multiple;
                    }
                    else
                    {
                        _payExpense.Amount = payExpense.Amount * currencyConvert.Result.Multiple;
                        _payExpense.Tax = payExpense.Tax * currencyConvert.Result.Multiple;
                    }
                }
            }

            return _payExpense;
        }
    }
}
