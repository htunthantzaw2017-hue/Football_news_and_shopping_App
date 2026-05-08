using ManchesterUnitedApp.Data;
using ManchesterUnitedApp.Data.Entities.Player;

namespace ManchesterUnitedApp.Repositories.PlayerRepository
{
    public class PlayerPositionRepository
    {
        private readonly ApplicationDbContext _context;
        public PlayerPositionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<PlayerPosition> GetAll()
        {
            return _context.PlayerPositions.ToList();
        }
    }
}
