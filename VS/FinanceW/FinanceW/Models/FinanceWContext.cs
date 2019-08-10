using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace FinanceW.Models
{
    public class FinanceWContext : DbContext
    {
        public FinanceWContext(DbContextOptions<FinanceW.Models.FinanceWContext> options) : base(options)
        {
            
        }

        public DbSet<Bank> Banks { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<CurrencyConvert> CurrencyConvert { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Expense> Expense { get; set; }
        public DbSet<ReminderType> ReminderType { get; set; }
        public DbSet<PaymentReminder> PaymentReminder { get; set; }
        public DbSet<CreditCardCut> CreditCardCut { get; set; }
        public DbSet<CashIncome> CashIncome { get; set; }
        public DbSet<CashOutcome> CashOutcome { get; set; }
        public DbSet<PayExpense> PayExpense { get; set; }
        public DbSet<PayProduct> PayProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelbuilder);
        }

    }
}
