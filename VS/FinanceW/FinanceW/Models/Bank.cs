using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FinanceW.Models
{
    [Table("Bank")]
    public class Bank
    {
        public int BankId { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        public string Description { get; set; }
        
        public Enum.Country Country { get; set; }
        public Enum.StatusBank StatusBank { get; set; }

        public List<Product> Product { get; set; }
    }
}
