using ManchesterUnitedApp.Data.Entities.MatchEntity;
using ManchesterUnitedApp.Models.ViewModel;
using ManchesterUnitedApp.Repositories.IMatchRepository;

namespace ManchesterUnitedApp.Services.IMatchService
{
    public class MatchService :IMatchService
    {
        private readonly IMatchRepository _repository;

        public MatchService(IMatchRepository repository)
        {
            _repository = repository;
        }

        // --- Read Operations (Mapping Entity to Display ViewModel) ---

        private MatchViewModel MapEntityToViewModel(MatchEntity entity)
        {
            return new MatchViewModel
            {
                Id = entity.Id,
                HomeTeamName = entity.HomeTeamName,
                AwayTeamName = entity.AwayTeamName,
                MatchDate = entity.MatchDate,
                HomeScore = entity.HomeScore,
                AwayScore = entity.AwayScore,
                IsFinished = entity.IsFinished,
                HomeTeamLogoPath = entity.HomeTeamLogoPath,
                AwayTeamLogoPath = entity.AwayTeamLogoPath
            };
        }

        public async Task<IEnumerable<MatchViewModel>> GetUpcomingMatchesAsync()
        {
            var all = await _repository.GetAllMatchesAsync();
            var upcomingEntities = all.Where(m => !m.IsFinished && m.MatchDate > DateTime.UtcNow).OrderBy(m => m.MatchDate);
            return upcomingEntities.Select(MapEntityToViewModel);
        }

        public async Task<IEnumerable<MatchViewModel>> GetMatchResultsAsync()
        {
            var all = await _repository.GetAllMatchesAsync();
            // Results: finished matches OR matches whose date has passed (potentially live/ongoing)
            var resultEntities = all.Where(m => m.IsFinished || m.MatchDate <= DateTime.UtcNow).OrderByDescending(m => m.MatchDate);
            return resultEntities.Select(MapEntityToViewModel);
        }

        public async Task<MatchViewModel?> GetMatchByIdAsync(int id)
        {
            var entity = await _repository.GetMatchByIdAsync(id);
            return entity == null ? null : MapEntityToViewModel(entity);
        }

        // Used specifically for loading data into the Edit form
        public async Task<MatchCreateViewModel?> GetMatchForEditAsync(int id)
        {
            var entity = await _repository.GetMatchByIdAsync(id);
            if (entity == null) return null;

            return new MatchCreateViewModel
            {
                Id = entity.Id,
                HomeTeamName = entity.HomeTeamName,
                AwayTeamName = entity.AwayTeamName,
                MatchDate = entity.MatchDate,
                HomeScore = entity.HomeScore,
                AwayScore = entity.AwayScore,
                IsFinished = entity.IsFinished,
               
            };
        }

        // --- Write Operations (Mapping Input ViewModel to Entity) ---

        public async Task CreateMatchAsync(MatchCreateViewModel viewModel, string webRootPath)
        {
            var entity = new MatchEntity
            {
                HomeTeamName = viewModel.HomeTeamName,
                AwayTeamName = viewModel.AwayTeamName,
                MatchDate = viewModel.MatchDate,
                HomeScore = viewModel.HomeScore,
                AwayScore = viewModel.AwayScore,
                IsFinished = viewModel.IsFinished,
            };

            if (viewModel.HomeLogoFile != null) entity.HomeTeamLogoPath = await SaveImage(viewModel.HomeLogoFile, webRootPath);
            if (viewModel.AwayLogoFile != null) entity.AwayTeamLogoPath = await SaveImage(viewModel.AwayLogoFile, webRootPath);

            await _repository.AddMatchAsync(entity);
        }

        public async Task UpdateMatchAsync(MatchCreateViewModel viewModel, string webRootPath)
        {
            var existing = await _repository.GetMatchByIdAsync(viewModel.Id);
            if (existing == null) return;

            existing.HomeTeamName = viewModel.HomeTeamName;
            existing.AwayTeamName = viewModel.AwayTeamName;
            existing.MatchDate = viewModel.MatchDate;
            existing.HomeScore = viewModel.HomeScore;
            existing.AwayScore = viewModel.AwayScore;
            existing.IsFinished = viewModel.IsFinished;

            // Replace old logos if new files uploaded
            if (viewModel.HomeLogoFile != null)
            {
                if (!string.IsNullOrEmpty(existing.HomeTeamLogoPath))
                {
                    var oldPath = Path.Combine(webRootPath, existing.HomeTeamLogoPath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }
                existing.HomeTeamLogoPath = await SaveImage(viewModel.HomeLogoFile, webRootPath);
            }

            if (viewModel.AwayLogoFile != null)
            {
                if (!string.IsNullOrEmpty(existing.AwayTeamLogoPath))
                {
                    var oldPath = Path.Combine(webRootPath, existing.AwayTeamLogoPath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }
                existing.AwayTeamLogoPath = await SaveImage(viewModel.AwayLogoFile, webRootPath);
            }

            await _repository.UpdateMatchAsync(existing);
        }


        public async Task DeleteMatchAsync(int id)
        {
            await _repository.DeleteMatchAsync(id);
        }

        // --- File Upload Helper ---
        private async Task<string> SaveImage(IFormFile file, string webRootPath)
        {
            string uploadsFolder = Path.Combine(webRootPath, "images");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return "/images/" + uniqueFileName;
        }
    }
}
