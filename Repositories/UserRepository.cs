using ManchesterUnitedApp.Data;
using Microsoft.EntityFrameworkCore;

namespace ManchesterUnitedApp.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public string? GetUserName(string userId)
        {
            //return _context.Users.Find(userId)?.UserName;
            return _context.Users
        .AsNoTracking() // Recommended for read-only operations
        .Where(u => u.Id == userId)
        .Select(u => u.UserName)
        .FirstOrDefault();
        }
    }
}
