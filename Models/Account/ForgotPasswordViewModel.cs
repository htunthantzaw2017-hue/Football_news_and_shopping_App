using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
