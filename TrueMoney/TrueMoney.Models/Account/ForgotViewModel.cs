using System.ComponentModel.DataAnnotations;

namespace TrueMoney.Models.Account
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}