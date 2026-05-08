using ManchesterUnitedApp.Models.ViewModel;

namespace ManchesterUnitedApp.Services.IMatchService
{
    public interface IMatchService
    {
        Task<IEnumerable<MatchViewModel>> GetUpcomingMatchesAsync();
        Task<IEnumerable<MatchViewModel>> GetMatchResultsAsync();
        Task<MatchCreateViewModel?> GetMatchForEditAsync(int id);
        Task<MatchViewModel?> GetMatchByIdAsync(int id);
        Task CreateMatchAsync(MatchCreateViewModel viewModel, string webRootPath);
        Task UpdateMatchAsync(MatchCreateViewModel viewModel, string webRootPath);
        Task DeleteMatchAsync(int id);
    }
}
