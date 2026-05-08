namespace ManchesterUnitedApp.Data.Entities.Player
{
    public class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        // Navigation property
        public ICollection<Player> Players { get; set; }
    }
}