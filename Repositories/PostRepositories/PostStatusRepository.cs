using ManchesterUnitedApp.Data;
using ManchesterUnitedApp.Data.Entities;
using ManchesterUnitedApp.Data.Entities.Post;

namespace ManchesterUnitedApp.Repositories.PostRepositories
{
    public class PostStatusRepository
    {
        private readonly ApplicationDbContext _context;
        public PostStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<PostStatus> GetAll()
        {
            return _context.PostStatuses.ToList();
        }
    }
}
