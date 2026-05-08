using ManchesterUnitedApp.Data;
using ManchesterUnitedApp.Data.Entities.Player;
using Microsoft.EntityFrameworkCore;

namespace ManchesterUnitedApp.Repositories.PlayerRepository
{
    public class PlayerRepository
    {
        private readonly ApplicationDbContext _context;
        public PlayerRepository(ApplicationDbContext context) => _context = context;

        // Get all (not deleted) with required includes
        public async Task<List<Player>> GetAllAsync()
        {
            return await _context.Players
                .Where(p => !p.IsDeleted)
                .Include(p => p.GenderType)
                .Include(p => p.Team)
                .Include(p => p.PlayerPosition)
                .Include(p => p.PlayerStatus)
                .AsNoTracking()
                .ToListAsync();
        }

        // Combined filter (gender, team, search)
        public async Task<List<Player>> GetFilteredAsync(string? gender, string? teamName, string? search = null)
        {
            var query = _context.Players.Where(p => !p.IsDeleted);

            if (!string.IsNullOrWhiteSpace(gender))
                query = query.Where(p => p.GenderType != null && p.GenderType.Name == gender);

            if (!string.IsNullOrWhiteSpace(teamName))
                query = query.Where(p => p.Team != null && p.Team.Name == teamName);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name != null && EF.Functions.Like(p.Name, $"%{search}%"));

            return await query
                .Include(p => p.GenderType)
                .Include(p => p.Team)
                .Include(p => p.PlayerPosition)
                .Include(p => p.PlayerStatus)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Player?> GetByIdAsync(Guid id) =>
            await _context.Players.AsNoTracking().FirstOrDefaultAsync(p => !p.IsDeleted && p.Id == id);

        public async Task<Player?> GetDetailByIdAsync(Guid id)
        {
            return await _context.Players
                .Where(p => !p.IsDeleted && p.Id == id)
                .Include(p => p.GenderType)
                .Include(p => p.Team)
                .Include(p => p.PlayerPosition)
                .Include(p => p.PlayerStatus)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Player entity)
        {
            await _context.Players.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(Player entity)
        {
            var existing = await _context.Players.FirstOrDefaultAsync(p => p.Id == entity.Id);
            if (existing == null) throw new KeyNotFoundException($"Player {entity.Id} not found.");

            existing.Name = entity.Name;
            existing.DOB = entity.DOB;
            existing.Age = entity.Age;
            existing.Nationality = entity.Nationality;
            existing.Height = entity.Height;
            existing.Weight = entity.Weight;
            existing.ImageData = entity.ImageData;
            existing.ImageType = entity.ImageType;
            existing.JerseyNumber = entity.JerseyNumber;
            existing.GenderTypeId = entity.GenderTypeId;
            existing.TeamId = entity.TeamId;
            existing.PlayerPositionId = entity.PlayerPositionId;
            existing.PlayerStatusId = entity.PlayerStatusId;
            existing.UpdatedBy = entity.UpdatedBy;
            existing.UpdatedOn = entity.UpdatedOn;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Players.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Player {id} not found.");

            entity.IsDeleted = true;
            entity.UpdatedOn = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}