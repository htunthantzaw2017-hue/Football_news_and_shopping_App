using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Models.Post
{
    public class PostListModel
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; }
        public byte[]? ItemImage { get; set; }
        public string ImageType { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string PostStatusName { get; set; } = string.Empty;
        public string CreatedBy { get; set; }
        [Display(Name = "Created Date: ")]
        [DataType(DataType.Date)]
        public DateTimeOffset CreatedOn { get; set; }
    }
}
