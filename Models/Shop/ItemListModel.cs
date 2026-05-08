using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Models.Shop
{
    public class ItemListModel
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
        public string ItemTypeName { get; set; } = string.Empty;
        [Required]
        public byte[]? ItemImage { get; set; }
        public string? ImageType { get; set; }
        [Display(Name = "Author")]
        public string CreatedBy { get; set; }
        [Display(Name = "Created Date: ")]
        [DataType(DataType.Date)]
        public DateTimeOffset CreatedOn { get; set; }
    }
}
