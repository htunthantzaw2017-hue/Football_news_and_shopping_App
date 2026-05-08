using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Models.Post
{
    public class CategoryModel
    {
        public Guid Id { get; set; }
        [MaxLength(300)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CreatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTimeOffset CreatedOn { get; set; }
    }
}
