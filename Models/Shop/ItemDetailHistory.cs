using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Models.Shop
{
    public class ItemDetailHistory
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(225)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; }
        [Required]
        [Range(0, 99999, ErrorMessage = "quantity must greater than 0")]
        public int Qty { get; set; }
        [Required]
        public byte[]? ItemImage { get; set; }
        public string? ImageType { get; set; }
        public Guid ItemTypeId { get; set; }
        public string ItemTypeName { get; set; } = string.Empty;
        [Display(Name = "Admin")]
        public string CreatedBy { get; set; } = String.Empty;
        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset CreatedOn { get; set; }
    }
}
