using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Data.Entities.Post
{
    public class Post : Base
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; }
        public byte[]? ItemImage { get; set; }
        public string ImageType { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public Guid PostStatusId { get; set; }
        //Soft Delete
        public bool IsDeleted { get; set; }
        //Navigation properties
        public Category Category { get; set; }
        public PostStatus PostStatus { get; set; }
    }
}
