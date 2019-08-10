using System.ComponentModel.DataAnnotations;

namespace FinanceW.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
