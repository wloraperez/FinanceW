using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace FinanceW.Models
{
    [Table("Product")]
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Tipo de producto")]
        public Enum.ProductType ProductTypeId { get; set; }

        public string UserId { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name = "Número de producto")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        public string Alias { get; set; }

        public int BankId { get; set; }
        [ForeignKey("BankId"), Display(Name = "Banco")]
        public Bank bank { get; set; }

        public int CurrencyId { get; set; }
        [ForeignKey("CurrencyId"), Display(Name = "Moneda")]
        public Currency currency { get; set; }

        [Required(ErrorMessage = "Campo requerido."),DataType(DataType.Currency), Display(Name = "Monto Total")]
        public decimal TotalAmount { get; set; }

        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        [Required(ErrorMessage = "Campo requerido."), DataType(DataType.Date), Display(Name = "Fecha de apertura")]
        public DateTime OpeningDate { get; set; }

        [DataType(DataType.Date), Display(Name = "Fecha de corte")]
        public DateTime CutDay { get; set; }
        [Display(Name = "Días para pagar corte")]
        public int DaysToPayCut { get; set; }

        [DataType(DataType.Date), Display(Name = "Fecha de creación")]
        public DateTime CreatedDate { get; set; }
        [DataType(DataType.Date), Display(Name = "Fecha de actualización")]
        public DateTime UpdatedDate { get; set; }
        [Display(Name = "Estado")]
        public Enum.StatusProduct StatusProduct { get; set; }

        public List<PaymentReminder> PaymentReminder { get; set; }
        public List<CreditCardCut> CreditCardCut { get; set; }
        public List<CashIncome> CashIncome { get; set; }
        public List<CashOutcome> CashOutcome { get; set; }
    }
}
