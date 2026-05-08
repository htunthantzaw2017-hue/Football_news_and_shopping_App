namespace ManchesterUnitedApp.Models.Player
{
    public class TeamModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string GenderTypeName { get; set; } = string.Empty; // add this
        public Guid GenderTypeId { get; set; }
    }
}
