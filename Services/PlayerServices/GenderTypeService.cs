using ManchesterUnitedApp.Models.Player;
using ManchesterUnitedApp.Repositories.PlayerRepository;

namespace ManchesterUnitedApp.Services.PlayerServices
{
    public class GenderTypeService
    {
        private readonly GenderTypeRepository _gendertypeRepo;
        public GenderTypeService(GenderTypeRepository gendertypeRepo)
        {
            _gendertypeRepo = gendertypeRepo;
        }
        public List<GenderTypeModel> GetAll()
        {
            return _gendertypeRepo.GetAll()
                .Select(t => new GenderTypeModel
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList();
        }
    }
}
