using ManchesterUnitedApp.Data;
using ManchesterUnitedApp.Data.Entities.Post;
using Microsoft.EntityFrameworkCore;

namespace ManchesterUnitedApp.Repositories.PostRepositories
{
    public class PostRepository
    {
        private readonly ApplicationDbContext _context;
        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Post> GetAllQuery()
        {
            return _context.Posts
                .Include(p => p.Category)
                .Include(p => p.PostStatus);               
        }

        public List<Post> GetAll()
        {
            return GetAllQuery()
                .Where(p=> !p.IsDeleted)
                .ToList();
        }

        public Post? GetById(Guid id)
        {
            return _context.Posts.Where(p => !p.IsDeleted).FirstOrDefault(p => p.Id == id);
        }
        public Post? GetDetailById(Guid id)
        {
            return _context.Posts
                .Where(p => !p.IsDeleted)
                .FirstOrDefault(p => p.Id == id);
        }
        public void Add(Post entity)
        {
            _context.Posts.Add(entity);
            _context.SaveChanges();
        }
        public void Update(Post entity)
        {
            Post? exitingEntity = _context.Posts.FirstOrDefault(p => p.Id == entity.Id);
            if (exitingEntity == null)
            {
                throw new InvalidOperationException("Post not found.");
            }
            exitingEntity.Title = entity.Title;
            exitingEntity.Content = entity.Content;
            exitingEntity.UpdatedOn = DateTimeOffset.UtcNow;
            _context.SaveChanges();
        }
        public void Delete(Guid id)
        {
            Post? entity = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (entity == null)
            {
                throw new InvalidOperationException("Post not found.");

            }
            entity.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
