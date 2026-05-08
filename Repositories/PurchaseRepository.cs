using ManchesterUnitedApp.Data;
using ManchesterUnitedApp.Data.Entities;
using ManchesterUnitedApp.Enum;
using Microsoft.EntityFrameworkCore;

namespace ManchesterUnitedApp.Repositories
{
    public class PurchaseRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Purchase> GetAll()
        {
            return _context.Purchases
                .Include(i => i.Item)
                .OrderByDescending(x => x.PurchaseDate)
                .ToList();
        }

        public Purchase? GetById(Guid id)
        {
            return _context.Purchases.FirstOrDefault(x => x.Id == id);
        }

        public Purchase? GetDetailById(Guid id)
        {
            return _context.Purchases
                .Include(i => i.Item)
                .FirstOrDefault(p => p.Id == id);
        }
        public void Add(Purchase entity)
        {
            _context.Purchases.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Purchase entity)
        {
            _context.Purchases.Update(entity);
            _context.SaveChanges();
        }

        public List<Purchase> GetByStatus(PurchaseStatus status)
        {
            return _context.Purchases
                .Include(p => p.Item)
                .Where(p => p.Status == status)
                .OrderByDescending(p => p.PurchaseDate)
                .ToList();
        }


    }
}