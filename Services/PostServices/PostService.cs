using ManchesterUnitedApp.Data.Entities.Post;
using ManchesterUnitedApp.Models.Post;
using ManchesterUnitedApp.Repositories;
using ManchesterUnitedApp.Repositories.PostRepositories;

namespace ManchesterUnitedApp.Services.PostServices
{
    public class PostService
    {
        private readonly PostRepository _repo;
        private readonly UserRepository _userRepo;
        public PostService(PostRepository repo, UserRepository userRepo)
        {
            _repo = repo;
            _userRepo = userRepo;

        }
        public List<PostListModel> GetAll(bool includeDrafts = false)
        {
            // The Repository's GetAll() includes Category and PostStatus via Include()
            IQueryable<Post> query = _repo.GetAllQuery()
                .Where(p => !p.IsDeleted);

            // If not including drafts (for public view), filter out "Draft" status.
            if (!includeDrafts)
            {
                // **FIX:** Add filtering condition to exclude posts where PostStatus.Name is "Draft"
                query = query.Where(p => p.PostStatus != null && p.PostStatus.Name != "Draft");
            }

            List<PostListModel> model = query
                .Select(p => new PostListModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content.Length > 100 ? p.Content.Substring(0, 100) + "..." : p.Content,
                    CreatedBy = _userRepo.GetUserName(p.CreatedBy) ?? String.Empty,
                    CreatedOn = p.CreatedOn,
                    CategoryName = p.Category != null ? p.Category.Name : String.Empty,
                    PostStatusName = p.PostStatus != null ? p.PostStatus.Name : String.Empty,
                    ItemImage = p.ItemImage,
                    ImageType = p.ImageType
                })
                .OrderByDescending(p => p.CreatedOn) // Usually, most recent posts are at the top
                .ToList();

            return model;
        }
        public PostEntryModel? GetById(Guid id)
        {
            Post? entity = _repo.GetById(id);
            if (entity != null)
            {
                return new PostEntryModel
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Content = entity.Content,
                    ItemImage = entity.ItemImage,
                    ImageType = entity.ImageType
                };
            }
            return null;
        }
        public PostDetailModel? GetDetailById(Guid id)
        {
            Post? entity = _repo.GetDetailById(id);
            if (entity != null)
            {
                string createdUserName = _userRepo.GetUserName(entity.CreatedBy) ?? String.Empty;
                var model = new PostDetailModel
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Content = entity.Content,
                    ItemImage = entity.ItemImage,
                    ImageType = entity.ImageType,
                    CreatedBy = _userRepo.GetUserName(entity.CreatedBy) ?? String.Empty,
                    CreatedOn = entity.CreatedOn
                };
                return model;
            }
            return null;
        }

        public void Add(PostEntryModel model)
        {
            var now = DateTimeOffset.UtcNow;
            Post entity = new Post()
            {
                Title = model.Title,
                Content = model.Content,
                CategoryId = model.CategoryId,
                PostStatusId = model.PostStatusId,
                ItemImage = model.ItemImage,
                ImageType = model.ImageType,
                CreatedBy = model.CreatedBy,
                CreatedOn = now,
                UpdatedBy = model.CreatedBy,
                UpdatedOn = now,
                IsDeleted = false
            };
            _repo.Add(entity);
        }
        public void Update(PostEntryModel model)
        {
            Post? entity = _repo.GetById(model.Id);
            if (entity == null)
            {
                return;
            }
            entity.Title = model.Title;
            entity.Content = model.Content;
            entity.CategoryId = model.CategoryId;
            entity.PostStatusId = model.PostStatusId;
            entity.ItemImage = model.ItemImage;
            entity.ImageType = model.ImageType;
            entity.UpdatedOn = DateTimeOffset.UtcNow;

            _repo.Update(entity);
        }

        public void Delete(Guid id)
        {
            _repo.Delete(id);
        }
    }
}
