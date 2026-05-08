using ManchesterUnitedApp.Data.Entities;
using ManchesterUnitedApp.Models.Shop;
using ManchesterUnitedApp.Repositories;

namespace ManchesterUnitedApp.Services
{
    public class ItemService
    {
        private readonly ItemRepository _repo;
        private readonly UserRepository _userRepo;

        public ItemService(ItemRepository repo, UserRepository userRepo)
        {
            _repo = repo;
            _userRepo = userRepo;
        }

        public List<ItemListModel> GetAll()
        {
            return _repo.GetAll()
                .Select(p => new ItemListModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Qty = p.Qty,
                    ItemImage = p.ItemImage,
                    ImageType = p.ImageType,
                    ItemTypeName = p.ItemType != null ? p.ItemType.Name : string.Empty,
                    CreatedBy = _userRepo.GetUserName(p.CreatedBy) ?? string.Empty,
                    CreatedOn = p.CreatedOn
                })
                .OrderBy(p => p.CreatedOn)
                .ToList();
        }

        public ItemEntryModel? GetById(Guid id)
        {
            Item? entity = _repo.GetById(id);
            if (entity == null)
                return null;

            return new ItemEntryModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                Qty = entity.Qty,
                ItemImage = entity.ItemImage,
                ImageType = entity.ImageType,
                ItemTypeId = entity.ItemTypeId
            };
        }
        public ItemDetailHistory? GetDetailById(Guid id)
        {
            Item? entity = _repo.GetDetailById(id);
            if (entity == null)
                return null;

            return new ItemDetailHistory
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                Qty = entity.Qty,
                ItemImage = entity.ItemImage,
                ImageType = entity.ImageType,
                ItemTypeId = entity.ItemTypeId,
                ItemTypeName = entity.ItemType != null ? entity.ItemType.Name : string.Empty,
                CreatedBy = _userRepo.GetUserName(entity.CreatedBy) ?? string.Empty,
                CreatedOn = entity.CreatedOn
            };
        }

        public void Add(ItemEntryModel model)
        {
            var now = DateTimeOffset.UtcNow;

            Item entity = new Item
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Price = model.Price,
                Qty = model.Qty,
                ItemImage = model.ItemImage,
                ImageType = model.ImageType,
                ItemTypeId = model.ItemTypeId,
                CreatedBy = model.CreatedBy,
                CreatedOn = now,
                UpdatedBy = model.CreatedBy,
                UpdatedOn = now,
                IsDeleted = false
            };

            _repo.Add(entity);
        }

        public void Edit(ItemEntryModel model)
        {
            Item? existing = _repo.GetById(model.Id);
            if (existing == null)
            {
                throw new Exception("Item not found");
            }
            existing.Name = model.Name;
            existing.Price = model.Price;
            existing.Qty = model.Qty;
            existing.ItemImage = model.ItemImage;
            existing.ImageType = model.ImageType;
            existing.ItemTypeId = model.ItemTypeId;
            existing.CreatedBy = model.CreatedBy;
            existing.CreatedOn = DateTime.UtcNow;
            existing.UpdatedBy = model.CreatedBy;
            existing.UpdatedOn = DateTimeOffset.UtcNow;
            _repo.Edit(existing);
        }

        public void Delete(Guid id)
        {
            _repo.Delete(id);
        }
    }
}