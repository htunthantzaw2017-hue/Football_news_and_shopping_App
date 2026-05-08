namespace ManchesterUnitedApp.Models.Player
{
    public class PlayerStatusModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
       
        public Guid PlayerId { get; set; }
    }
}
