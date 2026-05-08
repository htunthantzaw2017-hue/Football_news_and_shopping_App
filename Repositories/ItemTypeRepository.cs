using ManchesterUnitedApp.Data;
using ManchesterUnitedApp.Data.Entities;

namespace ManchesterUnitedApp.Repositories
{
    public class ItemTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public ItemTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<ItemType> GetAll()
        {
            return _context.ItemTypes.ToList();
        }
    }
}
