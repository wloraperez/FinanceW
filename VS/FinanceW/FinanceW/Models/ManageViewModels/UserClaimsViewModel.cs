using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinanceW.Models.ManageViewModels
{
    public class UserClaimsViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        public string ClaimType { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        public string ClaimValue { get; set; }

        public IList<ApplicationUser> UserList { get; set; }

        public ApplicationUser Usuario { get; set; }
    }
}
