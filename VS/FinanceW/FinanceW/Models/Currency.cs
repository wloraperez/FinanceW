using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinanceW.Models
{
    [Table("Currency")]
    public class Currency
    {
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        public string Description { get; set; }

        public Enum.StatusCurrency StatusCurrency { get; set; }

        [InverseProperty("CurrencyFrom")]
        public List<CurrencyConvert> CurrencyConvertFrom { get; set; }

        [InverseProperty("CurrencyTo")]
        public List<CurrencyConvert> CurrencyConvertTo { get; set; }

        public List<Product> Product { get; set; }
    }
}
