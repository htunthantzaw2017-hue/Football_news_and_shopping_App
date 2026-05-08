using ManchesterUnitedApp.Data;
using ManchesterUnitedApp.Data.Entities.Post;

namespace ManchesterUnitedApp.Repositories.PostRepositories
{
    public class CategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<Category> GetAll()
        {
            return _context.Categories.ToList();
        }
    }
}
