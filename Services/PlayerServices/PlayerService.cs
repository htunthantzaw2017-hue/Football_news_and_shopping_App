using ManchesterUnitedApp.Data.Entities.Player;
using ManchesterUnitedApp.Models.Player;
using ManchesterUnitedApp.Repositories;
using ManchesterUnitedApp.Repositories.PlayerRepository;

namespace ManchesterUnitedApp.Services.PlayerServices
{
    public class PlayerService
    {
        private readonly PlayerRepository _repo;
        private readonly UserRepository _userRepo;

        public PlayerService(PlayerRepository repo, UserRepository userRepo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
        }

        // ✅ Get all players (Repo.GetAllAsync)
        public async Task<IEnumerable<PlayerListModel>> GetAllAsync()
        {
            var players = await _repo.GetAllAsync();
            return players.Select(MapToListModel).OrderBy(p => p.CreatedOn).ToList();
        }

        // ✅ Get filtered players (Repo.GetFilteredAsync)
        public async Task<IEnumerable<PlayerListModel>> GetFilteredAsync(string? gender, string? teamName, string? search = null)
        {
            var players = await _repo.GetFilteredAsync(gender, teamName, search);
            return players.Select(MapToListModel).OrderBy(p => p.CreatedOn).ToList();
        }

        // ✅ Get by Id (Repo.GetByIdAsync)
        public async Task<PlayerEntryModel?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            return new PlayerEntryModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Age = entity.Age,
                DOB = entity.DOB,
                Nationality = entity.Nationality ?? string.Empty,
                ImageType = entity.ImageType ?? string.Empty,
                ImageData = entity.ImageData ?? Array.Empty<byte>(),
                Height = entity.Height,
                Weight = entity.Weight,
                JerseyNumber = entity.JerseyNumber,
                GenderTypeId = entity.GenderTypeId,
                TeamId = entity.TeamId,
                PlayerPositionId = entity.PlayerPositionId,
                PlayerStatusId = entity.PlayerStatusId
            };
        }

        // ✅ Get detail by Id (Repo.GetDetailByIdAsync)
        public async Task<PlayerDetailModel?> GetDetailByIdAsync(Guid id)
        {
            var entity = await _repo.GetDetailByIdAsync(id);
            if (entity == null) return null;

            return new PlayerDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Age = entity.Age,
                DOB = entity.DOB,
                Nationality = entity.Nationality ?? string.Empty,
                Height = entity.Height,
                Weight = entity.Weight,
                ImageData = entity.ImageData ?? Array.Empty<byte>(),
                ImageType = entity.ImageType ?? string.Empty,
                JerseyNumber = entity.JerseyNumber,
                CreatedBy = _userRepo.GetUserName(entity.CreatedBy) ?? string.Empty,
                CreatedOn = entity.CreatedOn,
                GenderTypeName = entity.GenderType?.Name ?? string.Empty,
                TeamName = entity.Team?.Name ?? string.Empty,
                PositionName = entity.PlayerPosition?.Name ?? string.Empty,
                StatusName = entity.PlayerStatus?.Name ?? string.Empty
            };
        }

        // ✅ Add new player (Repo.AddAsync)
        public async Task AddAsync(PlayerEntryModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            var now = DateTimeOffset.UtcNow;
            var entity = new Player
            {
                Name = model.Name,
                Age = model.Age,
                DOB = model.DOB,
                Nationality = model.Nationality,
                Height = model.Height,
                Weight = model.Weight,
                JerseyNumber = model.JerseyNumber,
                GenderTypeId = model.GenderTypeId,
                TeamId = model.TeamId,
                PlayerPositionId = model.PlayerPositionId,
                PlayerStatusId = model.PlayerStatusId,
                ImageType = model.ImageType,
                ImageData = model.ImageData,
                CreatedBy = model.CreatedBy,
                CreatedOn = now,
                UpdatedBy = model.CreatedBy,
                UpdatedOn = now,
                IsDeleted = false
            };

            await _repo.AddAsync(entity);
        }

        // ✅ Update existing player (Repo.EditAsync)
        public async Task UpdateAsync(PlayerEntryModel model, string updatedBy)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (string.IsNullOrWhiteSpace(updatedBy)) throw new ArgumentException("UpdatedBy is required.", nameof(updatedBy));

            var now = DateTimeOffset.UtcNow;
            var existing = await _repo.GetByIdAsync(model.Id);
            if (existing == null) throw new InvalidOperationException("Player not found.");

            existing.Name = model.Name;
            existing.Age = model.Age;
            existing.DOB = model.DOB;
            existing.Nationality = model.Nationality;
            existing.Height = model.Height;
            existing.Weight = model.Weight;
            existing.JerseyNumber = model.JerseyNumber;
            existing.GenderTypeId = model.GenderTypeId;
            existing.TeamId = model.TeamId;
            existing.PlayerPositionId = model.PlayerPositionId;
            existing.PlayerStatusId = model.PlayerStatusId;
            existing.ImageData = model.ImageData;
            existing.ImageType = model.ImageType;
            existing.UpdatedBy = updatedBy;
            existing.UpdatedOn = now;
            existing.IsDeleted = false;

            await _repo.EditAsync(existing);
        }

        // ✅ Delete player (Repo.DeleteAsync)
        public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);

        // 🔧 Mapping helper
        private PlayerListModel MapToListModel(Player p)
        {
            return new PlayerListModel
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age,
                DOB = p.DOB,
                Nationality = p.Nationality ?? string.Empty,
                Height = p.Height,
                Weight = p.Weight,
                ImageData = p.ImageData ?? Array.Empty<byte>(),
                ImageType = p.ImageType ?? "image/jpeg",
                JerseyNumber = p.JerseyNumber,
                CreatedBy = _userRepo.GetUserName(p.CreatedBy) ?? string.Empty,
                CreatedOn = p.CreatedOn,
                GenderTypeName = p.GenderType?.Name ?? string.Empty,
                TeamName = p.Team?.Name ?? string.Empty,
                PositionName = p.PlayerPosition?.Name ?? string.Empty,
                StatusName = p.PlayerStatus?.Name ?? string.Empty
            };
        }
    }
}