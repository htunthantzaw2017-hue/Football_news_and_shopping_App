using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Models.Player
{
    public class PlayerDetailModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime DOB { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public int JerseyNumber { get; set; }
        public Guid GenderTypeId { get; set; }
        public Guid TeamId { get; set; }
        public Guid PlayerStatus { get; set; }
        public Guid PlayerPosition { get; set; }
        public byte[]? ImageData { get; set; }
        public string ImageType { get; set; } = string.Empty;
        public string GenderTypeName { get; set; } = string.Empty;
        public string TeamName { get; set; } = string.Empty;
        public string NationalityName { get; set; } = string.Empty;
        public string PositionName { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public string CreatedBy { get; set; }
        [Display(Name = "Created Date: ")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset CreatedOn { get; set; }
    }
}