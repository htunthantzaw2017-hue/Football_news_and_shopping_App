using ManchesterUnitedApp.Models.Player;
using ManchesterUnitedApp.Repositories.PlayerRepository;

namespace ManchesterUnitedApp.Services.PlayerServices
{
    public class TeamService
    {
        private readonly TeamRepository _teamRepo;
        public TeamService(TeamRepository teamRepo)
        {
            _teamRepo = teamRepo;
        }
        public List<TeamModel> GetAll()
        {
            return _teamRepo.GetAll()
                .Select(t => new TeamModel
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToList();
        }
    }
}
