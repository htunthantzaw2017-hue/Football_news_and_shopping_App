using ManchesterUnitedApp.Data;
using ManchesterUnitedApp.Data.Entities.Player;
using Microsoft.EntityFrameworkCore;

namespace ManchesterUnitedApp.Repositories.PlayerRepository
{
    public class TeamRepository
    {
        private readonly ApplicationDbContext _context;
        public TeamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Team> GetAll()
        {
            // include GenderType so we can show GenderTypeName
            return _context.Teams
                .Include(t => t.Players)
                .ToList();
        }

        public Team? GetById(Guid id)
        {
            return _context.Teams
                .Include(t => t.Players)
                .FirstOrDefault(t => t.Id == id);
        }
    }
}
