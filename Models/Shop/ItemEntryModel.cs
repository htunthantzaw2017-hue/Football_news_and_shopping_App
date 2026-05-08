using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Models.Shop
{
    public class ItemEntryModel
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(225)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(1, 99999, ErrorMessage = "price must greater than 0")]
        public double Price { get; set; }
        [Required]
        [Range(0, 99999, ErrorMessage = "quantity must greater than 0")]
        public int Qty { get; set; }
        [Required]
        public IFormFile? UploadImage { get; set; }
        public byte[]? ItemImage { get; set; }
        public string ImageType { get; set; } = string.Empty;
        public Guid ItemTypeId { get; set; }
        public string CreatedBy { get; set; } = String.Empty;
    }
}
