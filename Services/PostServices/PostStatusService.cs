using ManchesterUnitedApp.Models;
using ManchesterUnitedApp.Models.Post;
using ManchesterUnitedApp.Repositories;
using ManchesterUnitedApp.Repositories.PostRepositories;

namespace ManchesterUnitedApp.Services.PostServices
{
    public class PostStatusService
    {
        private readonly PostStatusRepository _poststatusRepo;
        public PostStatusService(PostStatusRepository poststatusRepo)
        {
            _poststatusRepo = poststatusRepo;
        }
        public List<PostStatusModel> GetAll()
        {
            return _poststatusRepo.GetAll()
                .Select(t => new PostStatusModel
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList();
        }
    }
}
