using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceW.Models
{
    [Table("CreditCardCut")]
    public class CreditCardCut
    {
        public int CreditCardCutId { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Producto")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId"), Display(Name = "Producto")]
        public Product Product { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Fecha de corte")]
        public DateTime PayDayCut { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Fecha límite de pago")]
        public DateTime PayDayLimit { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Monto")]
        public decimal AmountCut { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Monto pagado")]
        public decimal AmountPayment { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Monto pendiente")]
        public decimal AmountPending { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime CreatedDate { get; set; }
    }
}
