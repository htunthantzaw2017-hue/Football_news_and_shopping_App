using ManchesterUnitedApp.Data.Entities.Post;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManchesterUnitedApp.Models.Post
{
    public class PostDetailModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Category")]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Html)]
        public string Content { get; set; } = string.Empty;

        public byte[]? ItemImage { get; set; }

        [Display(Name = "Image Type")]
        public string ImageType { get; set; } = string.Empty;

        [Display(Name = "Status")]
        public string PostStatusName { get; set; } = string.Empty;

        [Display(Name = "Author")]
        public string CreatedBy { get; set; } = string.Empty;

        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset CreatedOn { get; set; }
    }
}
