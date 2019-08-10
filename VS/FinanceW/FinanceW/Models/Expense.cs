using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceW.Models
{
    [Table("Expense")]
    public class Expense
    {
        public int ExpenseId { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Tipo de gasto")]
        public Enum.ExpenseType ExpenseTypeId { get; set; }
        public string UserId { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Moneda")]
        public int CurrencyId { get; set; }
        [ForeignKey("CurrencyId"), Display(Name = "Moneda")]
        public Currency currency { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [DataType(DataType.Currency), Display(Name = "Monto")]
        public decimal Amount { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date), Required(ErrorMessage = "Campo requerido."), Display(Name = "Fecha de corte")]
        public DateTime CutDay { get; set; }

        [Range(1, 31), Required(ErrorMessage = "Campo requerido."), Display(Name = "Días límites de pago")]
        public int PayDayLimit { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date), Display(Name = "Fecha de creación")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Estado")]
        public Enum.StatusExpense StatusExpense { get; set; }

        public List<PaymentReminder> PaymentReminder { get; set; }
    }
}
