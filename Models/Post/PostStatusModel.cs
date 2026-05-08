using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Models.Post
{
    public class PostStatusModel
    {
        public Guid Id { get; set; }
        public String Name { get; set; } = String.Empty;
        public string CreatedBy { get; set; }
        [Display(Name = "Created Date: ")]
        [DataType(DataType.Date)]
        public DateTimeOffset CreatedOn { get; set; }
    }
}
