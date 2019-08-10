using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceW.Models
{
    [Table("PayProduct")]
    public class PayProduct
    {
        public int PayProductId { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Producto origen")]
        public int ProductIdFrom { get; set; }
        [ForeignKey("ProductIdFrom"), Display(Name = "Producto origen")]
        public Product ProductFrom { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Producto destino")]
        public int ProductIdTo { get; set; }
        [ForeignKey("ProductIdTo"), Display(Name = "Producto destino")]
        public Product ProductTo { get; set; }

        [Display(Name = "Corte de tarjeta de crédito")]
        public int? CreditCardCutId { get; set; }
        [ForeignKey("CreditCardCutId"), Display(Name = "Corte de tarjeta de crédito")]
        public CreditCardCut CreditCardCut { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Monto")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Display(Name = "Impuesto por transacción")]
        [DataType(DataType.Currency)]
        public decimal Tax { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Fecha de pago")]
        [DataType(DataType.Date)]
        public DateTime PayProductDate { get; set; }

        [Display(Name = "Fecha de registro")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Estado de pago")]
        public Enum.StatusPayment StatusPayProduct { get; set; }
    }
}
