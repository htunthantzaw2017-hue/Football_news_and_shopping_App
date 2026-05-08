namespace ManchesterUnitedApp.Data.Entities.Player
{
    public class Player : Base
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime DOB { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public int JerseyNumber { get; set; }
        public byte[]? ImageData { get; set; }
        public string ImageType { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public Guid GenderTypeId { get; set; }
        public Guid TeamId { get; set; }
        public Guid PlayerPositionId { get; set; }
        public Guid PlayerStatusId { get; set; }
        // Navigation property
        public GenderType GenderType { get; set; }
        public PlayerPosition PlayerPosition { get; set; }
        public PlayerStatus PlayerStatus { get; set; }
        public Team Team { get; set; }

    }
}