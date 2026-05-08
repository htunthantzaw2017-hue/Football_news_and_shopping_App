using ManchesterUnitedApp.Data.Entities.MatchEntity;

namespace ManchesterUnitedApp.Repositories.IMatchRepository
{
    public interface IMatchRepository
    {
        Task<IEnumerable<MatchEntity>> GetAllMatchesAsync();
        Task<MatchEntity?> GetMatchByIdAsync(int id);
        Task AddMatchAsync(MatchEntity match);
        Task UpdateMatchAsync(MatchEntity match);
        Task DeleteMatchAsync(int id);
    }
}
