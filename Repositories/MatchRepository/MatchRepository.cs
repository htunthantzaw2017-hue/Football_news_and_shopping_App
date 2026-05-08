using ManchesterUnitedApp.Data;
using ManchesterUnitedApp.Data.Entities.MatchEntity;
using Microsoft.EntityFrameworkCore;

namespace ManchesterUnitedApp.Repositories.IMatchRepository
{
    public class MatchRepository : IMatchRepository
    {
        private readonly ApplicationDbContext _context;

        public MatchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MatchEntity>> GetAllMatchesAsync()
        {
            return await _context.Matches.OrderByDescending(m => m.MatchDate).ToListAsync();
        }

        public async Task<MatchEntity?> GetMatchByIdAsync(int id)
        {
            return await _context.Matches.FindAsync(id);
        }

        public async Task AddMatchAsync(MatchEntity match)
        {
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMatchAsync(MatchEntity match)
        {
            _context.Matches.Update(match);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMatchAsync(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match != null)
            {
                _context.Matches.Remove(match);
                await _context.SaveChangesAsync();
            }
        }
    }
}
