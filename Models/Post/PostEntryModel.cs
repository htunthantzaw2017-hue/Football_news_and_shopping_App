using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Models.Post
{
    public class PostEntryModel
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public IFormFile? UploadImage { get; set; }
        public byte[]? ItemImage { get; set; }
        public string ImageType { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public Guid PostStatusId { get; set; }
        public string CreatedBy { get; set; } = String.Empty;
    }
}
