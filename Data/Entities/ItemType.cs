using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Data.Entities
{
    public class ItemType:Base
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        //navigation properties
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
