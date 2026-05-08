using ManchesterUnitedApp.Models;
using ManchesterUnitedApp.Repositories;

namespace ManchesterUnitedApp.Services
{
    public class ItemTypeService
    {
        private readonly ItemTypeRepository _itemtypeRepo;
        public ItemTypeService(ItemTypeRepository itemtypeRepo)
        {
            _itemtypeRepo = itemtypeRepo;
        }
        public List<ItemTypeModel> GetAll()
        {
            return _itemtypeRepo.GetAll()
                .Select(t => new ItemTypeModel
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList();
        }
    }
}
