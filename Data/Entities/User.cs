using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        public byte[]? ProfileImage { get; set; }

        
    }
}
