namespace ManchesterUnitedApp.Data.Entities.Player
{
    public class PlayerPosition
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid PlayerId { get; set; }
        // Navigation property
        public ICollection<Player> Players { get; set; }
    }
}