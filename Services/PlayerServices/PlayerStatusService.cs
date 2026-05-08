using ManchesterUnitedApp.Models.Player;
using ManchesterUnitedApp.Repositories.PlayerRepository;

namespace ManchesterUnitedApp.Services.PlayerServices
{
    public class PlayerStatusService
    {
        private readonly PlayerStatusRepository _playerstatusRepo;
        public PlayerStatusService(PlayerStatusRepository playerstatusRepo)
        {
            _playerstatusRepo = playerstatusRepo;
        }
        public List<PlayerStatusModel> GetAll()
        {
            return _playerstatusRepo.GetAll()
                .Select(t => new PlayerStatusModel
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList();
        }
    }
}
