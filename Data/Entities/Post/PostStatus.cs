using Microsoft.Identity.Client;

namespace ManchesterUnitedApp.Data.Entities.Post
{
    public class PostStatus
    {
        public Guid Id { get; set; }
        public String Name { get; set; } = String.Empty;
        //navigation properties
        public ICollection<Post> Posts { get; set; }
    }
}
