using ManchesterUnitedApp.Models.Player;
using ManchesterUnitedApp.Repositories.PlayerRepository;

namespace ManchesterUnitedApp.Services.PlayerServices
{
    public class PlayerPositionService
    {
        private readonly PlayerPositionRepository _playerpositionRepo;
        public PlayerPositionService(PlayerPositionRepository playerpositionRepo)
        {
            _playerpositionRepo = playerpositionRepo;
        }
        public List<PlayerPositionModel> GetAll()
        {
            return _playerpositionRepo.GetAll()
                .Select(t => new PlayerPositionModel
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList();
        }
    }
}
