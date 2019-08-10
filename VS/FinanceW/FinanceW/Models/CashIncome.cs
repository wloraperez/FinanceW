using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceW.Models
{
    [Table("CashIncome")]
    public class CashIncome
    {
        public int CashIncomeId { get; set; }

        [Display(Name = "Producto")]
        public int? ProductId { get; set; }
        [ForeignKey("ProductId"), Display(Name = "Producto")]
        public Product Product { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Monto")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Fecha de ingreso")]
        [DataType(DataType.Date)]
        public DateTime IncomeDate { get; set; }
        [Display(Name = "Fecha de registro")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Estado")]
        public Enum.StatusCashFlow StatusIncome { get; set; }
    }
}
