using ManchesterUnitedApp.Data;
using ManchesterUnitedApp.Data.Entities.Player;

namespace ManchesterUnitedApp.Repositories.PlayerRepository
{
    public class PlayerStatusRepository
    {
        private readonly ApplicationDbContext _context;
        public PlayerStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<PlayerStatus> GetAll()
        {
            return _context.PlayerStatuses.ToList();
        }
    }
}
