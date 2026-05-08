using ManchesterUnitedApp.Data;
using ManchesterUnitedApp.Data.Entities.Player;

namespace ManchesterUnitedApp.Repositories.PlayerRepository
{
    public class GenderTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public GenderTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<GenderType> GetAll()
        {
            return _context.GenderTypes.ToList();
        }
    }
}
