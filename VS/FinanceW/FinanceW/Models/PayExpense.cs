using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceW.Models
{
    [Table("PayExpense")]
    public class PayExpense
    {
        public int PayExpenseId { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Producto origen")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId"), Display(Name = "Producto origen")]
        public Product Product { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Gasto recurrente")]
        public int ExpenseId { get; set; }
        [ForeignKey("ExpenseId"), Display(Name = "Gasto recurrente")]
        public Expense Expense { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Descripción")]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Monto")]
        public decimal Amount { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Impuesto por transacción")]
        public decimal Tax { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de pago")]
        public DateTime PayExpenseDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de registro")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Estado")]
        public Enum.StatusCashFlow StatusPayExpense { get; set; }
    }
}
