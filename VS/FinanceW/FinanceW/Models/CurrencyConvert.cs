using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinanceW.Models
{
    [Table("CurrencyConvert")]
    public class CurrencyConvert
    {
        public int CurrencyConvertId { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [DataType(DataType.Currency)]
        public decimal Multiple { get; set; }

        //[Required(ErrorMessage = "Campo requerido.")]
        public int CurrencyFromCurrencyId { get; set; }
        public Currency CurrencyFrom { get; set; }

        //[Required(ErrorMessage = "Campo requerido.")]
        public int CurrencyToCurrencyId { get; set; }
        public Currency CurrencyTo { get; set; }

        [Required(ErrorMessage = "Campo requerido."), DataType(DataType.Date)]
        public DateTime DateValidFrom { get; set; }

        [Required(ErrorMessage = "Campo requerido."), DataType(DataType.Date)]
        public DateTime DateValidTo { get; set; }

        public Enum.StatusCurrency StatusCurrency { get; set; }
    }
}
