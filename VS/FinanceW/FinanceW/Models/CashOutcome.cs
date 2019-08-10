using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceW.Models
{
    [Table("CashOutcome")]
    public class CashOutcome
    {
        public int CashOutcomeId { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Producto")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId"), Display(Name = "Producto")]
        public Product Product { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Monto")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Fecha de gasto")]
        [DataType(DataType.Date)]
        public DateTime OutcomeDate { get; set; }
        [ Display(Name = "Fecha de registro")]
        public DateTime CreatedDate { get; set; }
        [ Display(Name = "Estado")]
        public Enum.StatusCashFlow StatusOutcome { get; set; }
    }
}
