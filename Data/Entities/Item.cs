using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Data.Entities
{
    public class Item : Base
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; }
        public byte[]? ItemImage { get; set; } 
        public string? ImageType { get; set; }
        [Required]
        public int Qty { get; set; }
        public bool IsDeleted { get; set; }
        public Guid ItemTypeId { get; set;}
        //Navigation properties
        public ItemType ItemType { get; set; }
        public ICollection<Purchase> Purchases{ get; set; }
    }
}
