using ManchesterUnitedApp.Data;
using ManchesterUnitedApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ManchesterUnitedApp.Repositories
{
    public class ItemRepository
    {
        private readonly ApplicationDbContext _context;
        public ItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Item> GetAll()
        {
            return _context.Items
                .Include(i => i.ItemType)
                .Where(p => !p.IsDeleted)
                .ToList();
        }
        public Item? GetById(Guid id)
        {
            return _context.Items.Where(p => !p.IsDeleted).FirstOrDefault(p => p.Id == id);
        }
        public Item? GetDetailById(Guid id)
        {
            return _context.Items
                .Where(p => !p.IsDeleted)
                .Include(i => i.ItemType)
                .FirstOrDefault(p => p.Id == id);
        }
        public void Add(Item entity)
        {
            _context.Items.Add(entity);
            _context.SaveChanges();
        }
        public void Edit(Item entity)
        {
            Item? exitingEntity = _context.Items.FirstOrDefault(p => p.Id == entity.Id);
            if (exitingEntity == null)
            {
                throw new InvalidOperationException("Item not found.");
            }
            exitingEntity.Name = entity.Name;
            exitingEntity.Price = entity.Price;
            exitingEntity.Qty = entity.Qty;
            exitingEntity.ItemImage = entity.ItemImage;
            exitingEntity.ImageType = entity.ImageType;
            exitingEntity.ItemTypeId = entity.ItemTypeId;
            exitingEntity.CreatedBy = entity.CreatedBy;
            exitingEntity.CreatedOn = DateTime.UtcNow;
            exitingEntity.UpdatedBy = entity.CreatedBy;
            exitingEntity.UpdatedOn = DateTimeOffset.UtcNow;
            _context.Items.Update(entity);
            _context.SaveChanges();
        }
        public void Delete(Guid id)
        {
            Item? entity = _context.Items.FirstOrDefault(p => p.Id == id);
            if (entity == null)
            {
                throw new InvalidOperationException("Item not found.");

            }
            entity.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}