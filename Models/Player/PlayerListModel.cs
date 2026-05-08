using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Models.Player
{
    public class PlayerListModel
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
        public string GenderTypeName { get; set; } = string.Empty;
        public string TeamName { get; set; } = string.Empty;
        // Optional (for MU-style UI)
        public string PositionName { get; set; } = string.Empty;     // e.g. Goalkeeper
        public string StatusName { get; set; } = string.Empty;       // e.g. On Loan
        public string CreatedBy { get; set; } = string.Empty;
        [Display(Name = "Created Date: ")]
        [DataType(DataType.Date)]
        public DateTimeOffset CreatedOn { get; set; }
    }
}