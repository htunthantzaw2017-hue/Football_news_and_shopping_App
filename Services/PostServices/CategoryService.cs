using ManchesterUnitedApp.Models.Post;
using ManchesterUnitedApp.Repositories.PostRepositories;

namespace ManchesterUnitedApp.Services.PostServices
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepo;
        public CategoryService(CategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
        public List<CategoryModel> GetAll()
        {
            return _categoryRepo.GetAll()
                .Select(t => new CategoryModel
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList();
        }
    }
}
