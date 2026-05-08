using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Models
{
    public class ItemTypeModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTimeOffset CreatedOn { get; set; }
    }
}
