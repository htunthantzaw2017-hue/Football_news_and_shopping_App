using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Data.Entities.Post
{
    public class Category
    {
        public Guid Id { get; set; }
        [MaxLength(300)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        //Navigation property
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
